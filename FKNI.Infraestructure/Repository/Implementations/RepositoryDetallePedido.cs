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
    public class RepositoryDetallePedido : IRepositoryDetallePedido
    {
        private readonly FKNIContext _context;
        public RepositoryDetallePedido(FKNIContext context)
        {
            _context = context;
        }
        public async Task<ICollection<DetallePedido>> FindByIdAsync(int id_pedido)
        {
            var @object = await _context.Set<DetallePedido>()
                .Where(x => x.IdPedido == id_pedido)
                .Include(x => x.IdDetalleCarritoNavigation).ThenInclude(p => p.IdProductoNavigation)
                .ThenInclude(i => i.IdImagen)
                .Include(x => x.IdPedidoNavigation).ThenInclude(p => p.IdPagoNavigation)
                .Include(x => x.IdPedidoNavigation).ThenInclude(p => p.IdClienteNavigation)
                .Include(x => x.IdPedidoNavigation).ThenInclude(p => p.IdEstadoNavigation)
                .ToListAsync();
            return @object!;
        }
        public async Task<ICollection<DetallePedido>> ListAsync()
        {
            //Listar los libros incluyendo su autor, ordenado de forma descendente
            var collection = await _context.Set<DetallePedido>().ToListAsync();
            return collection;
        }

        public async Task<int> AddAsync(DetallePedido entity)
        {
            await _context.Set<DetallePedido>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdDetallePedido;
        }
    }
}
