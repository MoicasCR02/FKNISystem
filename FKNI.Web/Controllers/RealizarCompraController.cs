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
        private readonly IServiceDetalleCarrito _serviceDetalleCarrito;

        //private readonly FKNIContext _context;

        public RealizarCompraController(IServicePagos servicePagos, IServicePedidos servidePedidos, IServiceDetallePedido serviceDetallePedido,IServiceDetalleCarrito serviceDetalleCarrito )
        {
            _servicePagos = servicePagos;
            _servicePedidos = servidePedidos;
            _serviceDetallePedido = serviceDetallePedido;
            _serviceDetalleCarrito = serviceDetalleCarrito;
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
                    //await _serviceDetalleCarrito.
                }

                return Ok(new { mensaje = "Compra simulada" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error interno: " + ex.Message);
                return StatusCode(500, new { mensaje = "Error interno", detalle = ex.Message });
            }
        }


        [HttpPost]
        public IActionResult Test()
        {
            Console.WriteLine("✅ Entró al método Test");
            return Ok("Test OK");
        }

    }
}
