using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.DTOs
{
    public record PromocionesDTO
    {
        public int IdPromocion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de promoción")]
        public string? TipoPromocion { get; set; }

        [Display(Name = "Producto")]
        public int? IdProducto { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una Categoria")]
        public int? IdCategoria { get; set; }

       
        public double Descuento { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public virtual Categorias? IdCategoriaNavigation { get; set; }

        public virtual Productos? IdProductoNavigation { get; set; }

        [NotMapped]
        [Required]

        [Display(Name = "Producto")]
        public string BuscarProducto { get; set; } = null!;

        public string NombreCategoria { get; set; } = null!;

    }
}
