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
        public int IdDetalle { get; set; }

        public int? IdPedido { get; set; }

        public decimal? Subtotal { get; set; }

        public decimal? Impuesto { get; set; }

        public decimal? TotalConImpuesto { get; set; }

        public virtual Pedidos? IdPedidoNavigation { get; set; }
    }
}
