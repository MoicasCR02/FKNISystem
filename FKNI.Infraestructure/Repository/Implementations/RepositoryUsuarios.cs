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
    public class RepositoryUsuarios : IRepositoryUsuarios
    {
        private readonly FKNIContext _context;
        public RepositoryUsuarios(FKNIContext context)
        {
            _context = context;
        }
        public async Task<Usuarios> FindByIdAsync(int IdUsuario)
        {
            var @object = await _context.Set<Usuarios>().FindAsync(IdUsuario);

            return @object!;
        }
        public async Task<ICollection<Usuarios>> ListAsync()
        {
            var collection = await _context.Set<Usuarios>().AsNoTracking().ToListAsync();
            return collection;
        }

        public async Task<int> AddAsync(Usuarios entity)
        {
            // Agregar la entidad al contexto
            await _context.Usuarios.AddAsync(entity);

            // Guardar en la base de datos
            await _context.SaveChangesAsync();

            // Retornar el Id generado
            return entity.IdUsuario;
        }

        public async Task<Usuarios> LoginNameAsync(string id)
        {
            var @object = await _context.Set<Usuarios>()
                                        .Include(b => b.IdRolNavigation)
                                        .Where(p => p.Correo == id)
                                        .FirstOrDefaultAsync();
            return @object!;
        }
    }
}
