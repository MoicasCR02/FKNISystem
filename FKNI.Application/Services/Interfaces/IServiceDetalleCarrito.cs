using FKNI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.Services.Interfaces
{
    public interface IServiceDetalleCarrito
    {
        Task<int> AddAsync(DetalleCarritoDTO dto);
    }
}
