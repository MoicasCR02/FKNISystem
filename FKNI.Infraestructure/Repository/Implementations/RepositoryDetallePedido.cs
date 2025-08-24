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
        public async Task<DetallePedido> FindByIdAsync(int id_Detalle)
        {
            //Obtener un Libro con su autor y las lista de categorías
            var @object = await _context.Set<DetallePedido>()
                                .Where(x => x.IdDetalle == id_Detalle).FirstAsync();
            return @object!;
        }
        public async Task<ICollection<DetallePedido>> ListAsync()
        {
            //Listar los libros incluyendo su autor, ordenado de forma descendente
            var collection = await _context.Set<DetallePedido>().ToListAsync();
            return collection;
        }
    }
}
