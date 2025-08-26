using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FKNI.Infraestructure.Models;

public partial class Promociones
{
    public int IdPromocion { get; set; }
    [Required(ErrorMessage = "Debe seleccionar un tipo de promoción")]
    public string? TipoPromocion { get; set; }

    public int? IdProducto { get; set; }

    public int? IdCategoria { get; set; }

    public double Descuento { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public virtual Categorias? IdCategoriaNavigation { get; set; }

    public virtual Productos? IdProductoNavigation { get; set; }

    [NotMapped]
    public string BuscarProducto { get; set; } = null!;
}
