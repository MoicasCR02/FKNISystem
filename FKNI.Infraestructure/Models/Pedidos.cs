using System;
using System.Collections.Generic;

namespace FKNI.Infraestructure.Models;

public partial class Pedidos
{
    public int IdPedido { get; set; }

    public int? IdCliente { get; set; }

    public string? MetodoEntrega { get; set; }

    public string? DireccionEnvio { get; set; }

    public int? IdEstado { get; set; }

    public int? IdPago { get; set; }

    public virtual ICollection<DetallePedido> DetallePedido { get; set; } = new List<DetallePedido>();

    public virtual Usuarios? IdClienteNavigation { get; set; }

    public virtual Estados? IdEstadoNavigation { get; set; }

    public virtual Pagos? IdPagoNavigation { get; set; }
}
