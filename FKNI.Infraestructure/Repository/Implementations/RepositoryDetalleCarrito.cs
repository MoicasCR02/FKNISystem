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


        public async Task<DetalleCarrito?> FindByIdExists(int id_carrito, int id_producto, string talla)
        {
            var @object = await _context.Set<DetalleCarrito>()
                .Where(x => x.IdCarrito == id_carrito && x.IdProducto == id_producto && x.Talla == talla)
                .Include(x => x.IdProductoNavigation).ThenInclude(r => r.IdImagen)
                .FirstOrDefaultAsync();

            return @object;
        }


        public async Task<int> AddAsync(DetalleCarrito entity)
        {
            await _context.Set<DetalleCarrito>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdDetalleCarrito;
        }

        public async Task UpdateAsync(DetalleCarrito entity)
        {
            var existing = await _context.DetalleCarrito
                .FirstOrDefaultAsync(x => x.IdCarrito == entity.IdCarrito && x.IdProducto == entity.IdProducto && x.Talla == entity.Talla);

            if (existing == null)
            {
                // Opcional: lanzar excepción o manejar el caso
                throw new InvalidOperationException("No existe el detalle del carrito a actualizar.");
            }

            // Actualizar solo los campos deseados
            existing.Cantidad = entity.Cantidad;
            existing.Subtotal = entity.Subtotal;
            existing.TotalImpuesto = entity.TotalImpuesto;
            existing.Total = entity.Total;

            await _context.SaveChangesAsync();
        }

        public async Task<DetalleCarrito> DeleteAsync(int id_producto ,int id_carrito, string talla)
        {
            var existente = await FindByIdExists(id_carrito, id_producto, talla);
            if(existente != null){
            


            }
            if(existente.Cantidad <= 1)
            {   
                // Raw Query
                //https://www.learnentityframeworkcore.com/raw-sql/execute-sql
                int rowAffected = _context.Database.ExecuteSql($"Delete DetalleCarrito where id_producto = {id_producto} and id_carrito = {id_carrito} and talla = {talla}");
                await Task.FromResult(1);
            }
            else
            {
                existente.Cantidad = existente.Cantidad - 1;
                existente.Subtotal = existente.Subtotal-existente.PrecioUnitario;
                existente.TotalImpuesto  = existente.Subtotal * existente.Impuesto;
                existente.Total = existente.Subtotal + existente.TotalImpuesto;
                await UpdateAsync(existente);
            }
            var actualizado = await FindByIdExists(id_carrito, id_producto, talla);
            return actualizado;
        }
    }
}
