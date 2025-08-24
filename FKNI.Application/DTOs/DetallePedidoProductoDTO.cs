using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.DTOs
{
    public record DetallePedidoProductoDTO
    {
        public int IdDetallePedido { get; set; }

        public int? IdPedido { get; set; }

        public int? IdProducto { get; set; }

        public int? Cantidad { get; set; }

        public virtual Pedidos? IdPedidoNavigation { get; set; }

        public virtual Productos? IdProductoNavigation { get; set; }
    }
}
