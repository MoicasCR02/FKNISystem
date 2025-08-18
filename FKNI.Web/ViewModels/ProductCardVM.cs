namespace FKNI.Web.ViewModels
{
    public class ProductCardVM
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string? DescripcionCorta { get; set; }
        public decimal Precio { get; set; }
        public decimal? PrecioConPromo { get; set; }
        public string ImagenUrl { get; set; } = "";
        public bool TienePromo => PrecioConPromo.HasValue && PrecioConPromo.Value < Precio;
    }
}
