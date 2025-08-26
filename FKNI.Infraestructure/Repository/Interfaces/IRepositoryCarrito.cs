using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryCarrito
    {
        Task<ICollection<Carrito>> FindByIdAsync(int id_usuario);
        Task<Carrito> FindByIdCarritoAsync(int id_carrito);
        Task<int> AddAsync(Carrito entity);
        Task UpdateAsync(Carrito entity);
    }
}

