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

        public async Task<int> AddAsync(DetalleCarrito entity)
        {
            await _context.Set<DetalleCarrito>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdCarrito;
        }
    }
}
