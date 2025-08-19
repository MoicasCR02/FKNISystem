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

        public async Task<int> AddAsync(DetalleCarrito entity)
        {
            await _context.Set<DetalleCarrito>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdCarrito;
        }
    }
}
