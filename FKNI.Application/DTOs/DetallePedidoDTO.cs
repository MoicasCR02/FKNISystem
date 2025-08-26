using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.DTOs
{
    public record DetallePedidoDTO
    {
        public int IdDetallePedido { get; set; }

        public int? IdPedido { get; set; }

        public int? IdDetalleCarrito { get; set; }

        public virtual DetalleCarrito? IdDetalleCarritoNavigation { get; set; }

        public virtual Pedidos? IdPedidoNavigation { get; set; }
    }
}
