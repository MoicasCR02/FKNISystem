using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using FKNI.Web.Models;           // ErrorViewModel
using FKNI.Web.ViewModels;       // HomeViewModel, ProductCardVM
using FKNI.Infraestructure.Data; // FKNIContext

namespace FKNI.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FKNIContext _ctx;

        public HomeController(ILogger<HomeController> logger, FKNIContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        public async Task<IActionResult> Index()
        {
            var ahora = DateTime.Now; // tu SQL usa GETDATE()

            // ---------- DESTACADOS: traemos bytes y luego convertimos en memoria ----------
            var destacadosRaw = await _ctx.Productos
                .AsNoTracking()
                .Where(p => p.Estado == true)
                .OrderByDescending(p => p.PromedioValoracion)
                .ThenByDescending(p => p.IdProducto)
                .Select(p => new
                {
                    p.IdProducto,
                    p.NombreProducto,
                    p.Descripcion,
                    p.Precio,
                    p.IdCategoria,
                    ImagenBytes = p.IdImagen
                        .OrderBy(i => i.IdImagen)
                        .Select(i => i.UrlImagen) // byte[]
                        .FirstOrDefault(),
                    PrecioConPromo = (
                        from promo in _ctx.Promociones
                        where promo.FechaInicio <= ahora && ahora <= promo.FechaFin
                              && (
                                   (promo.TipoPromocion == "producto"  && promo.IdProducto  == p.IdProducto) ||
                                   (promo.TipoPromocion == "categoria" && promo.IdCategoria == p.IdCategoria)
                                 )
                        orderby promo.Descuento descending
                        select Math.Round((decimal)p.Precio * (1m - (promo.Descuento / 100m)), 2)).FirstOrDefault()
                })
                .Take(5)
                .ToListAsync();

            var destacados = destacadosRaw
                .Select(x => new ProductCardVM
                {
                    Id = x.IdProducto,
                    Nombre = x.NombreProducto,
                    DescripcionCorta = !string.IsNullOrEmpty(x.Descripcion) && x.Descripcion.Length > 90
                        ? x.Descripcion.Substring(0, 90) + "…"
                        : x.Descripcion,
                    Precio = (decimal)x.Precio,
                    PrecioConPromo = x.PrecioConPromo == 0 ? (decimal?)null : x.PrecioConPromo,
                    ImagenUrl = (x.ImagenBytes != null && x.ImagenBytes.Length > 0)
                        ? "data:image/jpeg;base64," + Convert.ToBase64String(x.ImagenBytes)
                        : "https://source.unsplash.com/1600x900/?tshirt,apparel"
                })
                .ToList();

            // ---------- RECIENTES: mismo patrón ----------
            var recientesRaw = await _ctx.Productos
                .AsNoTracking()
                .Where(p => p.Estado == true)
                .OrderByDescending(p => p.IdProducto)
                .Select(p => new
                {
                    p.IdProducto,
                    p.NombreProducto,
                    p.Descripcion,
                    p.Precio,
                    p.IdCategoria,
                    ImagenBytes = p.IdImagen
                        .OrderBy(i => i.IdImagen)
                        .Select(i => i.UrlImagen) // byte[]
                        .FirstOrDefault(),
                    PrecioConPromo = (
                        from promo in _ctx.Promociones
                        where promo.FechaInicio <= ahora && ahora <= promo.FechaFin
                              && (
                                   (promo.TipoPromocion == "producto"  && promo.IdProducto  == p.IdProducto) ||
                                   (promo.TipoPromocion == "categoria" && promo.IdCategoria == p.IdCategoria)
                                 )
                        orderby promo.Descuento descending
                        select Math.Round((decimal)p.Precio * (1m - (promo.Descuento / 100m)), 2)
                    ).FirstOrDefault()
                })
                .Take(8)
                .ToListAsync();

            var recientes = recientesRaw
                .Select(x => new ProductCardVM
                {
                    Id = x.IdProducto,
                    Nombre = x.NombreProducto,
                    DescripcionCorta = !string.IsNullOrEmpty(x.Descripcion) && x.Descripcion.Length > 70
                        ? x.Descripcion.Substring(0, 70) + "…"
                        : x.Descripcion,
                    Precio = (decimal)x.Precio,
                    PrecioConPromo = x.PrecioConPromo == 0 ? (decimal?)null : x.PrecioConPromo,
                    ImagenUrl = (x.ImagenBytes != null && x.ImagenBytes.Length > 0)
                        ? "data:image/jpeg;base64," + Convert.ToBase64String(x.ImagenBytes)
                        : "https://source.unsplash.com/600x600/?tshirt,streetwear"
                })
                .ToList();

            var vm = new HomeViewModel
            {
                Destacados = destacados,
                Recientes  = recientes
            };

            return View(vm);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
