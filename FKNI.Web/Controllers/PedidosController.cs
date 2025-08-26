using FKNI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FKNI.Web.Controllers
{
    public class PedidosController : Controller
    {
        private readonly IServicePedidos _servicePedidos;
        private readonly IServiceDetallePedido _serviceDetallePedido;
        public PedidosController(IServicePedidos servicePedidos, IServiceDetallePedido serviceDetallePedido)
        {
            _servicePedidos = servicePedidos;
            _serviceDetallePedido = serviceDetallePedido;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var collection = await _servicePedidos.ListAsync();
            return View(collection);
        }

        public async Task<IActionResult> MisPedidos(int id)
        {
            var collection = await _servicePedidos.FindByIdAsync(id);
            return View(collection);
        }
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("IndexAdmin");
                }
                var @object = await _servicePedidos.FindByIdAsync(id.Value);
                if (@object == null)
                {
                    throw new Exception("Producto no existente");

                }

                return View(@object);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IActionResult> DetallePedidos(int pedido)
        {
            var collection = await _serviceDetallePedido.FindByIdAsync(pedido);
            return View(collection);
        }
    }
}
