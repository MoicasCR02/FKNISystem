using FKNI.Application.DTOs;
using FKNI.Application.Services.Implementations;
using FKNI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.InteropServices;

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
                var detalles = await _serviceDetalleCarrito.FindByIdAsync(@object.IdCarrito);

                if (detalles.Count == 0)
                {
                    detalles = null;
                    ViewBag.ListDetalleCarrito = detalles;
                }
                else
                {
                    ViewBag.ListDetalleCarrito = await _serviceDetalleCarrito.FindByIdAsync(@object.IdCarrito);

                }


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
                var producto = await _serviceProductos.FindByIdAsync(id_producto);
                if (existe == null)
                {
                    var detallecarritoDTO = new DetalleCarritoDTO
                    {
                        IdCarrito = carrito.IdCarrito,
                        IdProducto = id_producto,
                        Cantidad = 1,
                        PrecioUnitario = (int)producto.Precio,
                    };

                    detallecarritoDTO.Subtotal = (int)producto.Precio;
                    detallecarritoDTO.Impuesto = 0.13;
                    detallecarritoDTO.TotalImpuesto = detallecarritoDTO.Subtotal * detallecarritoDTO.Impuesto;
                    detallecarritoDTO.Total = detallecarritoDTO.Subtotal + detallecarritoDTO.TotalImpuesto;

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
                    detallecarritoDTO.Subtotal = existe.Subtotal + existe.PrecioUnitario;
                    detallecarritoDTO.TotalImpuesto = detallecarritoDTO.Subtotal * detallecarritoDTO.Impuesto;
                    detallecarritoDTO.Total = detallecarritoDTO.Subtotal + detallecarritoDTO.TotalImpuesto;
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
            var Subtotal = 0d;
            var TotalImpuesto = 0d;
            var Total = 0d;
            var eliminado = await _serviceDetalleCarrito.DeleteAsync(id_producto, id_carrito);

            ViewBag.Mensaje = eliminado == null
                    ? "Se eliminó el producto del carrito"
                    : "Se eliminó una cantidad del producto";

            ViewBag.ListDetalleCarrito = await _serviceDetalleCarrito.FindByIdAsync(id_carrito);

            var detalles = await _serviceDetalleCarrito.FindByIdAsync(id_carrito);
            foreach(var item in detalles)
            {
                Subtotal = Subtotal + item.Subtotal;
                TotalImpuesto = TotalImpuesto  + item.TotalImpuesto;
                Total = Total + item.Total;
            }

            if (eliminado == null)
            {

                return Json(new { success = true, mensaje = "Producto Eliminado del Carrito", cantidadRestante = 0, SubtotalProductos = Subtotal, TotalImpuestoProductos = TotalImpuesto, TotalProductos = Total });
            }
            else
            {
                return Json(new { success = true, mensaje = "Cantidad eliminada  del carrito", cantidadRestante = eliminado.Cantidad, subtotal = eliminado.Subtotal
                    , total = eliminado.Total, totalImpuesto = eliminado.TotalImpuesto, SubtotalProductos = Subtotal, TotalImpuestoProductos = TotalImpuesto, TotalProductos = Total});
            }
        }
    }
}
