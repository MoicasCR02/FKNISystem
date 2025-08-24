using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryDetallePedidoProducto
    {
        Task<DetallePedidoProducto> FindByIdAsync(int id_Detalle);
        Task<ICollection<DetallePedidoProducto>> ListAsync();
    }
}
