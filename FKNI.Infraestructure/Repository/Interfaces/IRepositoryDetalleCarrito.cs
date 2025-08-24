using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryDetalleCarrito
    {
        Task<DetalleCarrito?> FindByIdExists(int id_carrito, int id_producto, string talla);
        Task<ICollection<DetalleCarrito>> FindByIdAsync(int id_carrito);
        Task<int> AddAsync(DetalleCarrito entity);

        Task UpdateAsync(DetalleCarrito entity);
        Task<DetalleCarrito> DeleteAsync(int id_producto, int id_carrito, string talla);
    }
}
