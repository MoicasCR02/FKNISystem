using FKNI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.Services.Interfaces
{
    public interface IServicePagos
    {
        Task<PagosDTO> FindByIdAsync(int id_pago);
        Task<ICollection<PagosDTO>> ListAsync();
        Task<int> AddAsync(PagosDTO dto);
    }
}
