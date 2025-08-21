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
    public class RepositoryDetalleCarrito : IRepositoryDetalleCarrito
    {
        private readonly FKNIContext _context;
        //Alt+Enter
        public RepositoryDetalleCarrito(FKNIContext context)
        {
            _context = context;
        }

        public async Task<ICollection<DetalleCarrito>> FindByIdAsync(int id_carrito)
        {
            var @object = await _context.Set<DetalleCarrito>()
                .Where(x => x.IdCarrito == id_carrito)
                .Include(x => x.IdProductoNavigation).ThenInclude(r => r.IdImagen)
                .ToListAsync();
            return @object!;
        }


        public async Task<DetalleCarrito?> FindByIdExists(int id_carrito, int id_producto)
        {
            var @object = await _context.Set<DetalleCarrito>()
                .Where(x => x.IdCarrito == id_carrito && x.IdProducto == id_producto)
                .Include(x => x.IdProductoNavigation).ThenInclude(r => r.IdImagen)
                .FirstOrDefaultAsync();

            return @object;
        }


        public async Task<int> AddAsync(DetalleCarrito entity)
        {
            await _context.Set<DetalleCarrito>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdCarrito;
        }

        public async Task UpdateAsync(DetalleCarrito entity)
        {
            var local = _context.Set<DetalleCarrito>()
                .Local
                .FirstOrDefault(x => x.IdCarrito == entity.IdCarrito && x.IdProducto == entity.IdProducto);

            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            _context.Attach(entity);

            // Evitar modificar claves o relaciones
            _context.Entry(entity).Property(e => e.IdCarrito).IsModified = false;
            _context.Entry(entity).Property(e => e.IdProducto).IsModified = false;
            _context.Entry(entity).Reference(e => e.IdProductoNavigation).IsModified = false;
            _context.Entry(entity).Reference(e => e.IdCarritoNavigation).IsModified = false;

            // Marcar solo la cantidad como modificada
            _context.Entry(entity).Property(e => e.Cantidad).IsModified = true;

            await _context.SaveChangesAsync();
        }

        public async Task<DetalleCarrito> DeleteAsync(int id_producto ,int id_carrito)
        {
            var existente = await FindByIdExists(id_carrito, id_producto);
            if(existente.Cantidad == 0)
            {   
                // Raw Query
                //https://www.learnentityframeworkcore.com/raw-sql/execute-sql
                int rowAffected = _context.Database.ExecuteSql($"Delete DetalleCarrito where id_producto = {id_producto} and id_carrito = {id_carrito}");
                await Task.FromResult(1);
            }
            else
            {
                existente.Cantidad = existente.Cantidad - 1;
                await UpdateAsync(existente);
            }
            return existente;
        }
    }
}
