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
                ViewBag.ListDetalleCarrito = await _serviceDetalleCarrito.FindByIdAsync(@object.IdCarrito);

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
                var existe = await _serviceDetalleCarrito.FindByExist(carrito.IdCarrito, id_producto);
                if (existe == null)
                {
                    var detallecarritoDTO = new DetalleCarritoDTO
                    {
                        IdCarrito = carrito.IdCarrito,
                        IdProducto = id_producto,
                        Cantidad = 1,
                    };

                    await _serviceDetalleCarrito.AddAsync(detallecarritoDTO);
                }
                else
                {
                    var detallecarritoDTO = new DetalleCarritoDTO
                    {
                        IdCarrito = existe.IdCarrito,
                        IdProducto = id_producto,
                        Cantidad = existe.Cantidad + 1,
                    };
                await _serviceDetalleCarrito.UpdateAsync(detallecarritoDTO);
                }




                    return Json(new { success = true, mensaje = "Producto agregado al carrito" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error al agregar al carrito", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id_producto, int id_carrito)
        {
            var eliminado = await _serviceDetalleCarrito.DeleteAsync(id_producto, id_carrito);

            ViewBag.Mensaje = eliminado.Cantidad == 0
                ? "Se eliminó el producto del carrito"
                : "Se eliminó una cantidad del producto";

            return View("Index");
        }


    }
}
