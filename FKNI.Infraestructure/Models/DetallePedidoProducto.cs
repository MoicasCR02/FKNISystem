using System;
using System.Collections.Generic;

namespace FKNI.Infraestructure.Models;

public partial class DetallePedidoProducto
{
    public int IdDetallePedido { get; set; }

    public int? IdPedido { get; set; }

    public int? IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public virtual Pedidos? IdPedidoNavigation { get; set; }

    public virtual Productos? IdProductoNavigation { get; set; }
}

