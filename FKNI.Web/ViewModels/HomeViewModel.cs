using System.Collections.Generic;

namespace FKNI.Web.ViewModels
{
    public class HomeViewModel
    {
        // Para el carrusel (destacados)
        public List<ProductCardVM> Destacados { get; set; } = new();

        // Para el mini-grid (recientes)
        public List<ProductCardVM> Recientes { get; set; } = new();
    }
}
