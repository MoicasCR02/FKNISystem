using FKNI.Infraestructure.Data;
using FKNI.Infraestructure.Models;
using FKNI.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Infraestructure.Repository.Implementations
{
    public class RepositoryProductos : IRepositoryProductos
    {
        private readonly FKNIContext _context;
        public RepositoryProductos(FKNIContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Productos>> FindByNameAsync(string nombre)
        {
            var collection = await _context.Set<Productos>().Where(p => p.NombreProducto.Contains(nombre) && !_context.Set<Promociones>().Any(pr => pr.IdProducto == p.IdProducto))
                .Include(p => p.IdImagen)
                .ToListAsync();
            return collection;

        }

        public async Task<Productos> FindByIdAsync(int id_producto)
        {
            //Obtener un Libro con su autor y las lista de categorías
            var @object = await _context.Set<Productos>()
                                .Where(x => x.IdProducto == id_producto)
                                .Include(x => x.IdCategoriaNavigation)
                                .Include(x => x.IdEtiqueta)
                                .Include(x => x.Resenas).ThenInclude(r => r.IdUsuarioNavigation)
                                .Include(x => x.IdImagen)
                                .FirstAsync();
            return @object!;
        }
        public async Task<ICollection<Productos>> ListAsync()
        {
            var collection = await _context.Set<Productos>()
                                .Include(x => x.IdCategoriaNavigation)
                                .Include(x => x.IdEtiqueta)
                                .Include(x => x.Resenas).ThenInclude(r => r.IdUsuarioNavigation)
                                .Include(x => x.IdImagen)
                                .ToListAsync();
            var hoy = DateTime.Today;
            var promociones = await _context.Set<Promociones>().ToListAsync();
            var promocionesVigentes = promociones.Where(p => p.FechaInicio <= hoy && p.FechaFin >= hoy).ToList();
            foreach (var producto in collection)
            {
                var promo = promocionesVigentes.FirstOrDefault(pr => pr.IdProducto == producto.IdProducto)
                         ?? promocionesVigentes.FirstOrDefault(pr => pr.IdCategoria == producto.IdCategoria);

                producto.Descuento = promo?.Descuento ?? 0;
            }

            return collection;
        }

        public async Task<int> AddAsync(Productos entity, string[] selectedEtiquetas)
        {
            //Relación de muchos a muchos solo con llave primaria compuesta
            var etiquetas = await getEtiquetas(selectedEtiquetas);
            entity.IdEtiqueta = etiquetas;
           // entity.IdEtiqueta = etiquetas;
            await _context.Set<Productos>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdProducto;
        }

        private async Task<ICollection<Etiquetas>> getEtiquetas(string[] selectedEtiquetas)
        {
            var etiquetas = await _context.Set<Etiquetas>()
                .Where(c => selectedEtiquetas.Contains(c.IdEtiqueta.ToString()))
                .ToListAsync();
            return etiquetas;

        }

        public async Task UpdateAsync(int id, Productos dto, string[] selectedEtiquetas)
        {
            var productoEnDb = await _context.Productos
                .Include(p => p.IdEtiqueta)
                .Include(p => p.IdImagen)
                .FirstOrDefaultAsync(p => p.IdProducto == id);

            if (productoEnDb == null) throw new Exception("Producto no encontrado");

            // Actualizar propiedades simples
            productoEnDb.NombreProducto = dto.NombreProducto;
            productoEnDb.Descripcion = dto.Descripcion;
            productoEnDb.Precio = dto.Precio;
            productoEnDb.Stock = dto.Stock;
            productoEnDb.IdCategoria = dto.IdCategoria;

            // Actualizar categoría
            var categoria = await _context.Categorias.FindAsync(dto.IdCategoria);
            productoEnDb.IdCategoriaNavigation = categoria!;

            // Sincronizar etiquetas
            var nuevasEtiquetas = await getEtiquetas(selectedEtiquetas);
            productoEnDb.IdEtiqueta.Clear();
            foreach (var etiqueta in nuevasEtiquetas)
            {
                productoEnDb.IdEtiqueta.Add(etiqueta);
            }

            // Sincronizar imágenes:
            // - Eliminar las imágenes que no están en dto.IdImagen
            var idsDto = dto.IdImagen.Select(i => i.IdImagen).ToList();
            var imagenesEliminar = productoEnDb.IdImagen.Where(i => !idsDto.Contains(i.IdImagen)).ToList();
            foreach (var imgEliminar in imagenesEliminar)
            {
                _context.Imagenes.Remove(imgEliminar);
            }

            // - Agregar imágenes nuevas (IdImagen == 0)
            foreach (var imgNueva in dto.IdImagen.Where(i => i.IdImagen == 0))
            {
                productoEnDb.IdImagen.Add(imgNueva);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // Raw Query
            //https://www.learnentityframeworkcore.com/raw-sql/execute-sql
            int rowAffected = _context.Database.ExecuteSql($"Delete Productos where id_producto = {id}");
            await Task.FromResult(1);
        }
    }
}
