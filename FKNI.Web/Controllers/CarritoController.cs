using FKNI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FKNI.Web.Controllers
{
    public class CarritoController : Controller
    {
        private readonly IServiceCarrito _serviceCarrito;
        //private readonly FKNIContext _context;

        public CarritoController(IServiceCarrito servicePCarrito)
        {
            _serviceCarrito = servicePCarrito;


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

        // GET: CarritoController/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        // POST: CarritoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CarritoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CarritoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CarritoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CarritoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
