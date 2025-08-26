using FKNI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.Services.Interfaces
{
    public interface IServicePedidos
    {
        Task<ICollection<PedidosDTO>> ListAsync();
        Task<ICollection<PedidosDTO>> FindByIdAsync(int id_usuario);

        Task<int> AddAsync(PedidosDTO dto);
    }
}
