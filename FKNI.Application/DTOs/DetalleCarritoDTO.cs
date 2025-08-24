using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.DTOs
{
    public record DetalleCarritoDTO
    {
        public int IdDetalleCarrito { get; set; }
        public int IdCarrito { get; set; }

        public int? IdProducto { get; set; }

        public int Cantidad { get; set; }

        public double Subtotal { get; set; }

        public double Impuesto { get; set; }

        public double Total { get; set; }

        public double PrecioUnitario { get; set; }

        public double TotalImpuesto { get; set; }

        public string? Talla { get; set; }

        public virtual Carrito IdCarritoNavigation { get; set; } = null!;

        public virtual Productos? IdProductoNavigation { get; set; }
    }
}
 