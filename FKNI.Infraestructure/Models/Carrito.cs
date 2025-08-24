using System;
using System.Collections.Generic;

namespace FKNI.Infraestructure.Models;

public partial class Carrito
{
    public int IdCarrito { get; set; }

    public int? IdUsuario { get; set; }

    public virtual ICollection<DetalleCarrito> DetalleCarrito { get; set; } = new List<DetalleCarrito>();

    public virtual Usuarios? IdUsuarioNavigation { get; set; }
}
