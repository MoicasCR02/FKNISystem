using FKNI.Application.DTOs;
using FKNI.Application.Services.Implementations;
using FKNI.Application.Services.Interfaces;
using FKNI.Infraestructure.Models;
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
        private readonly IServiceUsuarios _serviceUsuarios;

        //private readonly FKNIContext _context;

        public CarritoController(IServiceCarrito servicePCarrito, IServiceDetalleCarrito serviceDetalleCarrito, IServiceProductos serviceProductos, IServiceUsuarios serviceUsuarios)
        {
            _serviceCarrito = servicePCarrito;
            _serviceDetalleCarrito = serviceDetalleCarrito;
            _serviceProductos = serviceProductos;
            _serviceUsuarios = serviceUsuarios;
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
                var objectcarrito = await _serviceCarrito.FindByIdAsync(id);
                var id_carrito=0;
                foreach (var item in objectcarrito.Where(c => c.Estado == true))
                {
                    // Solo entra si Estado == true
                    id_carrito = item.IdCarrito;
                }
                objectcarrito = objectcarrito.Where(c => c.Estado == true).ToList();

                var detalles = await _serviceDetalleCarrito.FindByIdAsync(id_carrito);
                var usuario = await _serviceUsuarios.FindByIdAsync(id);
                if (detalles.Count == 0)
                {
                    detalles = null;
                    ViewBag.ListDetalleCarrito = detalles;
                }
                else
                {

                    ViewBag.ListDetalleCarrito = detalles.Any() ? detalles : null;
                    ViewBag.Usuario = usuario;

                }
                if (objectcarrito == null)    
                {
                    throw new Exception("Carrito no existente");

                }
                return View();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> AgregarCarrito(int id_producto, int id_usuario, string talla)
        {
            try
            {
                var carrito = await _serviceCarrito.FindByIdAsync(id_usuario);
                if(id_usuario == 0)
                {
                    return Json(new { success = true, mensaje = "Debes iniciar sesion" });
                }

                var id_carrito = 0;
                foreach (var item in carrito)
                {
                    if (item.Estado == true)
                    {
                        id_carrito = item.IdCarrito;
                    }
                }


                var existe = await _serviceDetalleCarrito.FindByExist(id_carrito, id_producto, talla);
                var producto = await _serviceProductos.FindByIdAsync(id_producto);
                if (existe == null)
                {
                    var detallecarritoDTO = new DetalleCarritoDTO
                    {
                        IdCarrito = id_carrito,
                        IdProducto = id_producto,
                        Cantidad = 1,
                        PrecioUnitario = (int)producto.Precio,
                        Talla = talla
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
                        Talla = existe.Talla
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
        public async Task<IActionResult> Delete(int id_producto, int id_carrito, string talla)
        {
            var Subtotal = 0d;
            var TotalImpuesto = 0d;
            var Total = 0d;


            var eliminado = await _serviceDetalleCarrito.DeleteAsync(id_producto, id_carrito,talla);

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
                return Json(new { success = true, mensaje = "Cantidad Eliminada  del carrito", cantidadRestante = eliminado.Cantidad, subtotal = eliminado.Subtotal
                    , total = eliminado.Total, totalImpuesto = eliminado.TotalImpuesto, SubtotalProductos = Subtotal, TotalImpuestoProductos = TotalImpuesto, TotalProductos = Total});
            }
        }

        public async Task<IActionResult> ResumenCarrito(int id_usuario)
        {
            var usuario = await _serviceUsuarios.FindByIdAsync(id_usuario);
            var @object = await _serviceCarrito.FindByIdAsync(id_usuario);
            var id_carrito = 0;
            foreach (var item in @object)
            {
                if (item.Estado == true)
                {
                    id_carrito = item.IdCarrito;
                }
            }

            var detalleCarrito = await _serviceDetalleCarrito.FindByIdAsync(id_carrito);
            ViewBag.ListDetalleCarrito = detalleCarrito;
            ViewBag.Usuario = usuario;
            return PartialView("ResumenCarrito");
        }
    }
}
