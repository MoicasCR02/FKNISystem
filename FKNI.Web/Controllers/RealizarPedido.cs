using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FKNI.Web.Controllers
{
    public class RealizarPedido : Controller
    {
        // GET: RealizarPedido
        public ActionResult Index()
        {
            return View();
        }

        // GET: RealizarPedido/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RealizarPedido/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RealizarPedido/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: RealizarPedido/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RealizarPedido/Edit/5
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

        // GET: RealizarPedido/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RealizarPedido/Delete/5
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
