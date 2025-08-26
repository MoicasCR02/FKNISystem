using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FKNI.Application.DTOs
{
    public record ProductosDTO
    {
        public int IdProducto { get; set; }

        public string NombreProducto { get; set; } = null!;

        public string? Descripcion { get; set; }

        public double Precio { get; set; }

        public int Stock { get; set; }

        public decimal? PromedioValoracion { get; set; }

        public int? IdCategoria { get; set; }

        public bool? Estado { get; set; }

        [JsonIgnore]
        public virtual Categorias? IdCategoriaNavigation { get; set; }

        public virtual ICollection<Resenas> Resenas { get; set; } = new List<Resenas>();

        public virtual ICollection<Etiquetas> IdEtiqueta { get; set; } = new List<Etiquetas>();

        public virtual ICollection<Imagenes> IdImagen { get; set; } = new List<Imagenes>();

        [NotMapped]
        public decimal Descuento { get; set; } = 0;

        public List<int> ExistingImageIds { get; set; } = new List<int>(); // ← IDs de imágenes ya guardadas

        public virtual ICollection<DetalleCarrito> DetalleCarrito { get; set; } = new List<DetalleCarrito>();


    }
}
