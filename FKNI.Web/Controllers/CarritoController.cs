using FKNI.Application.DTOs;
using FKNI.Application.Services.Implementations;
using FKNI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FKNI.Web.Controllers
{
    public class CarritoController : Controller
    {
        private readonly IServiceCarrito _serviceCarrito;
        private readonly IServiceDetalleCarrito _serviceDetalleCarrito;
        private readonly IServiceProductos _serviceProductos;

        //private readonly FKNIContext _context;

        public CarritoController(IServiceCarrito servicePCarrito, IServiceDetalleCarrito serviceDetalleCarrito, IServiceProductos serviceProductos)
        {
            _serviceCarrito = servicePCarrito;
            _serviceDetalleCarrito = serviceDetalleCarrito;
            _serviceProductos = serviceProductos;
        }
        [HttpGet]
        // GET: CarritoController
        public async Task<ActionResult> Index(int id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("IndexAdmin");
                }
                var @object = await _serviceCarrito.FindByIdAsync(id);
                if (@object == null)
                {
                    throw new Exception("Carrito no existente");

                }

                return View(@object);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> AgregarCarrito(int id_producto, int id_usuario)
        {
            try
            {
                var carrito = await _serviceCarrito.FindByIdAsync(id_usuario);

                var detallecarritoDTO = new DetalleCarritoDTO
                {
                    IdCarrito = carrito.IdCarrito,
                    IdProducto = id_producto,
                    Cantidad = 1,
                };

                await _serviceDetalleCarrito.AddAsync(detallecarritoDTO);

                return Json(new { success = true, mensaje = "Producto agregado al carrito" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error al agregar al carrito", error = ex.Message });
            }
        }
    }
}
