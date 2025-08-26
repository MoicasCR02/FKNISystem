using System;
using System.Collections.Generic;

namespace FKNI.Infraestructure.Models;

public partial class DetalleCarrito
{
    public int IdDetalleCarrito { get; set; }

    public int? IdCarrito { get; set; }

    public int? IdProducto { get; set; }

    public int Cantidad { get; set; }

    public double Subtotal { get; set; }

    public double Impuesto { get; set; }

    public double Total { get; set; }

    public double PrecioUnitario { get; set; }

    public double TotalImpuesto { get; set; }

    public string? Talla { get; set; }

    public virtual ICollection<DetallePedido> DetallePedido { get; set; } = new List<DetallePedido>();

    public virtual Carrito? IdCarritoNavigation { get; set; }

    public virtual Productos? IdProductoNavigation { get; set; }
}
