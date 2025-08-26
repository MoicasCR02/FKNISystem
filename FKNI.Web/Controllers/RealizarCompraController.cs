using FKNI.Application.DTOs;
using FKNI.Application.Services.Implementations;
using FKNI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Text.Json;

namespace FKNI.Web.Controllers
{
    public class RealizarCompraController : Controller
    {

        private readonly IServicePagos _servicePagos;
        private readonly IServicePedidos _servicePedidos;
        private readonly IServiceDetallePedido _serviceDetallePedido;
        private readonly IServiceCarrito _serviceCarrito;

        //private readonly FKNIContext _context;

        public RealizarCompraController(IServicePagos servicePagos, IServicePedidos servidePedidos, IServiceDetallePedido serviceDetallePedido, IServiceCarrito serviceCarrito)
        {
            _servicePagos = servicePagos;
            _servicePedidos = servidePedidos;
            _serviceDetallePedido = serviceDetallePedido;
            _serviceCarrito = serviceCarrito;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var rawJson = await reader.ReadToEndAsync();

                Console.WriteLine("📦 JSON recibido:");
                Console.WriteLine(rawJson);

                // Deserializar manualmente
                var compra = JsonSerializer.Deserialize<CompraDTO>(rawJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (compra == null)
                {
                    Console.WriteLine("⚠️ El modelo compra sigue siendo null");
                    return BadRequest(new { mensaje = "Datos inválidos" });
                }
                else
                {
                    var idPago = await _servicePagos.AddAsync(compra.Pago);
                    compra.Pedido.IdPago = idPago;
                    var idPedido = await _servicePedidos.AddAsync(compra.Pedido);
                    foreach(var item in compra.Detalles)
                    {
                        item.IdPedido = idPedido;
                        await _serviceDetallePedido.AddAsync(item);
                    }
                    //Crear nuevon carrito 


                    var nuevoCarrito = new CarritoDTO
                    {
                        IdUsuario = compra.Pedido.IdCliente,
                        Estado = true 
                    };

                    var carrito = await _serviceCarrito.FindByIdAsync((int)compra.Pedido.IdCliente);
                    var id_carrito = 0;
                    foreach (var item in carrito)
                    {
                        if (item.Estado == true)
                        {
                            id_carrito = item.IdCarrito;
                        }
                    }

                    await _serviceCarrito.AddAsync(nuevoCarrito);
                    

                    var Editarcarrito = new CarritoDTO
                    {
                        IdCarrito = id_carrito,
                        IdUsuario = compra.Pedido.IdCliente,
                        Estado = false
                    };
                    await _serviceCarrito.UpdateAsync(Editarcarrito);

                }

                return RedirectToAction("Index", "Pedidos");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error interno: " + ex.Message);
                return StatusCode(500, new { mensaje = "Error interno", detalle = ex.Message });
            }
        }
    }
}
