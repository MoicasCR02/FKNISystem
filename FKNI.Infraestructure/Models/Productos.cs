using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FKNI.Infraestructure.Models;

public partial class Productos
{
    public int IdProducto { get; set; }

    public string NombreProducto { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public decimal? PromedioValoracion { get; set; }

    public int? IdCategoria { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<DetalleCarrito> DetalleCarrito { get; set; } = new List<DetalleCarrito>();

    public virtual ICollection<DetallePedidoProducto> DetallePedidoProducto { get; set; } = new List<DetallePedidoProducto>();

    public virtual Categorias? IdCategoriaNavigation { get; set; }

    public virtual ICollection<Imagenes> IdImagen { get; set; } = new List<Imagenes>();

    public virtual ICollection<Promociones> Promociones { get; set; } = new List<Promociones>();

    public virtual ICollection<Resenas> Resenas { get; set; } = new List<Resenas>();

    public virtual ICollection<Etiquetas> IdEtiqueta { get; set; } = new List<Etiquetas>();
    [NotMapped]
    public decimal Descuento { get; set; } = 0;
}
