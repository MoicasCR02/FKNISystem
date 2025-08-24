using FKNI.Application.DTOs;
using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.Services.Interfaces
{
    public interface IServiceDetalleCarrito
    {
        Task<DetalleCarritoDTO> FindByExist(int id_carrito, int id_producto, string talla);
        Task<ICollection<DetalleCarritoDTO>> FindByIdAsync(int id_carrito);
        Task<int> AddAsync(DetalleCarritoDTO dto);
        Task UpdateAsync(DetalleCarritoDTO dto);
        Task<DetalleCarritoDTO> DeleteAsync(int id_producto, int id_carrito, string talla);
    }
}
