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

        public int? Vuelto { get; set; }

        public decimal? CostoEnvio { get; set; }

        public string? DireccionEntrega { get; set; }

        public DateOnly? FechaPago { get; set; }

        public virtual ICollection<Pedidos> Pedidos { get; set; } = new List<Pedidos>();
    }
}
