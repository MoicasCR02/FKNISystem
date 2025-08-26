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
    public class RepositoryPagos : IRepositoryPagos
    {
        private readonly FKNIContext _context;
        public RepositoryPagos(FKNIContext context)
        {
            _context = context;
        }
        public async Task<Pagos> FindByIdAsync(int id_pago)
        {
            //Obtener un Libro con su autor y las lista de categorías
            var @object = await _context.Set<Pagos>()
                                .Where(x => x.IdPago == id_pago).FirstAsync();
            return @object!;
        }
        public async Task<ICollection<Pagos>> ListAsync()
        {
            //Listar los libros incluyendo su autor, ordenado de forma descendente
            var collection = await _context.Set<Pagos>().ToListAsync();
            return collection;
        }

        public async Task<int> AddAsync(Pagos entity)
        {
            await _context.Set<Pagos>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdPago;
        }
    }
}
