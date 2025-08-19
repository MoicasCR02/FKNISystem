using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.DTOs
{
    public record CarritoDTO
    {
        public int IdCarrito { get; set; }

        public int? IdUsuario { get; set; }

        public virtual ICollection<DetalleCarrito> DetalleCarrito { get; set; } = new List<DetalleCarrito>();

        public virtual Usuarios? IdUsuarioNavigation { get; set; }
    }
}
