using FKNI.Application.DTOs;
using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.Services.Interfaces
{
    public interface IServiceCarrito
    {

        Task<ICollection<CarritoDTO>> FindByIdAsync(int id_usuario);
        Task<CarritoDTO> FindByIdCarritoAsync(int id_carrito);
        Task<int> AddAsync(CarritoDTO dto);
        Task UpdateAsync(CarritoDTO dto);
    }
}
