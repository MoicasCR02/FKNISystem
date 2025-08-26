using System;
using System.Collections.Generic;

namespace FKNI.Infraestructure.Models;

public partial class DetallePedido
{
    public int IdDetallePedido { get; set; }

    public int? IdPedido { get; set; }

    public int? IdDetalleCarrito { get; set; }

    public virtual DetalleCarrito? IdDetalleCarritoNavigation { get; set; }

    public virtual Pedidos? IdPedidoNavigation { get; set; }
}
