using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.DTOs
{
    public class CompraDTO
    {
        public PagosDTO Pago { get; set; } 
        public PedidosDTO Pedido { get; set; }
        public List<DetallePedidoDTO> Detalles { get; set; }
    }
}
