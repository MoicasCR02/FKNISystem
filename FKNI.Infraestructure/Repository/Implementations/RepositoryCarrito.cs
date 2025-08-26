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
    public class RepositoryCarrito : IRepositoryCarrito
    {
        private readonly FKNIContext _context;
        public RepositoryCarrito(FKNIContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Carrito>> FindByIdAsync(int id_usuario)
        {
            var @object = await _context.Set<Carrito>()
                .Where(x => x.IdUsuario == id_usuario)
                .Include(x => x.IdUsuarioNavigation)
                .ToListAsync();
            return @object!;
        }

        public async Task<Carrito> FindByIdCarritoAsync(int id_carrito)
        {
            var @object = await _context.Set<Carrito>()
                .Where(x => x.IdCarrito == id_carrito)
                .Include(x => x.IdUsuarioNavigation)
                .FirstOrDefaultAsync();
            return @object!;
        }
        public async Task<int> AddAsync(Carrito entity)
        {
            await _context.Set<Carrito>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdCarrito;
        }

        public async Task UpdateAsync(Carrito entity)
        {
            var existing = await _context.Carrito
                .FirstOrDefaultAsync(c => c.IdCarrito == entity.IdCarrito && c.IdUsuario == entity.IdUsuario);

            if (existing != null)
            {
                existing.Estado = false; // Cambiar el bit a 0

                // No tocamos navegación ni otros campos
                _context.Entry(existing).Property(e => e.Estado).IsModified = true;
                _context.Entry(existing).Reference(e => e.IdUsuarioNavigation).IsModified = false;

                await _context.SaveChangesAsync();
            }
        }

    }
}
