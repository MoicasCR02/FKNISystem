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
        public int IdCarrito { get; set; }

        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        public virtual Carrito IdCarritoNavigation { get; set; } = null!;

        public virtual Productos IdProductoNavigation { get; set; } = null!;
    }
}
 