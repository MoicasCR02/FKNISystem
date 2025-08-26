using FKNI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.Services.Interfaces
{
    public interface IServiceDetallePedido
    {
        Task<ICollection<DetallePedidoDTO>> FindByIdAsync(int id_pedido);
        Task<ICollection<DetallePedidoDTO>> ListAsync();
        Task<int> AddAsync(DetallePedidoDTO dto);
    }
}
