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
    public class RepositoryPromociones : IRepositoryPromociones
    {
        private readonly FKNIContext _context;
        public RepositoryPromociones(FKNIContext context)
        {
            _context = context;
        }

        public async Task<Promociones> FindByIdAsync(int id_promocion)
        {
            //Obtener un Libro con su autor y las lista de categorías
            var @object = await _context.Set<Promociones>()
                                .Where(x => x.IdPromocion == id_promocion)
                                .Include(x => x.IdProductoNavigation)
                                .Include(x => x.IdCategoriaNavigation)
                                .FirstAsync();
            return @object;
        }

        public async Task<ICollection<Promociones>> ListAsync()
        {
            //Listar los libros incluyendo su autor, ordenado de forma descendente
            var collection = await _context.Set<Promociones>()
                                .Include(x => x.IdProductoNavigation)
                                .Include(x => x.IdCategoriaNavigation)
                                .OrderBy(x => x.IdPromocion)
                                .ToListAsync();
            return collection;
        }

        public async Task<int> AddAsync(Promociones entity)
        {
            await _context.Set<Promociones>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdPromocion;
        }

        public async Task UpdateAsync(Promociones entity)
        {
            var existing = await _context.Promociones.FindAsync(entity.IdPromocion);
                    // No tocamos IdProducto ni FechaCreacion
                    if (entity.IdProducto != null)
                    {
                        _context.Entry(entity).Reference(e => e.IdProductoNavigation).IsModified = false;
                    }
                    if (entity.IdCategoria != null)
                    {
                        _context.Entry(entity).Reference(e => e.IdCategoriaNavigation).IsModified = false;
                    }             
                    await _context.SaveChangesAsync();  
        }

        public async Task DeleteAsync(int id)
        {
            // Raw Query
            //https://www.learnentityframeworkcore.com/raw-sql/execute-sql
            int rowAffected = _context.Database.ExecuteSql($"Delete Promociones where id_promocion = {id}");
            await Task.FromResult(1);
        }


    }
}
