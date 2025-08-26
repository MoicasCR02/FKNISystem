using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.DTOs
{
    public record PagosDTO
    {
        public int IdPago { get; set; }

        public string? MetodoPago { get; set; }

        public decimal? CostoEnvio { get; set; }

        public DateTime FechaPago { get; set; }

        public decimal? Subtotal { get; set; }

        public decimal? Impuesto { get; set; }

        public decimal? TotalConImpuesto { get; set; }

        public virtual ICollection<Pedidos> Pedidos { get; set; } = new List<Pedidos>();
    }
}
