using FKNI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.Services.Interfaces
{
    public interface IServiceDetallePedidoProducto
    {
        Task<DetallePedidoProductoDTO> FindByIdAsync(int id_detallePedido);
        Task<ICollection<DetallePedidoProductoDTO>> ListAsync();
    }
}
