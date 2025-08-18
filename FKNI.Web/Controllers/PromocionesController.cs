using FKNI.Application.DTOs;
using FKNI.Application.Services.Implementations;
using FKNI.Application.Services.Interfaces;
using FKNI.Infraestructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FKNI.Web.Controllers
{
    public class PromocionesController : Controller
    {
        private readonly IServicePromociones _servicePromociones;
        private readonly IServiceCategorias _serviceCategorias;
        public PromocionesController(IServicePromociones servicePromociones, IServiceCategorias serviceCategorias)
        {
            _servicePromociones = servicePromociones;
            _serviceCategorias = serviceCategorias;
        }
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            var collection = await _servicePromociones.ListAsync();
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
                var @object = await _servicePromociones.FindByIdAsync(id.Value);
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

        public async Task<IActionResult> Create()
        {
            ViewBag.ListCategoria = await _serviceCategorias.SinPromo();
            ViewBag.FechaInicio = DateTime.Today.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = DateTime.Today.ToString("yyyy-MM-dd");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PromocionesDTO dto)
        {
            ModelState.Remove("NombreCategoria");
            if (dto.IdCategoria != null)
            {
                ModelState.Remove("BuscarProducto");

            }
            else
            {
                ModelState.Remove("IdCategoria");
            }

            if (dto.FechaInicio < DateTime.Today)
            {
                ModelState.AddModelError("FechaInicio", "La fecha seleccionada no puede ser menor que la fecha de hoy.");
            }
            else
            {
                if (dto.FechaFin < dto.FechaInicio)
                {
                    ModelState.AddModelError("FechaFin", "La fecha seleccionada no puede ser menor que la fecha de Inicio.");
                }
                else
                {
                    if (dto.FechaFin == dto.FechaInicio)
                    {
                        ModelState.AddModelError("FechaFin", "La fecha seleccionada no puede ser igual que la fecha de Inicio, tiene que ser una promocion de un día para otro");                   
                    }
                }
            }

            if(dto.TipoPromocion == "")
            {
                ModelState.AddModelError(nameof(dto.TipoPromocion), "Debe seleccionar un tipo de promoción.");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.ListCategoria = await _serviceCategorias.SinPromo(); // 🔹 Recargar lista
                ViewBag.FechaInicio = DateTime.Today.ToString("yyyy-MM-dd");
                ViewBag.FechaFin = DateTime.Today.ToString("yyyy-MM-dd");
                dto.IdCategoria = null;
                dto.IdProducto = null;

                // 1) eliminar la entrada de ModelState para que el helper use el valor del modelo
                // (buscamos la clave porque a veces viene con prefijo)
                var key = ModelState.Keys.FirstOrDefault(k => k.EndsWith(nameof(dto.TipoPromocion), StringComparison.OrdinalIgnoreCase));
                if (key != null) ModelState.Remove(key);

                // 2) forzar el valor en el DTO a vacío (opcional pero claro)
                dto.TipoPromocion = "";

                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));

                return View(dto);
            }

            try
            {
                if(dto.BuscarProducto != null)
                {
                    dto.IdProducto = int.Parse(dto.BuscarProducto);
                }
                
                await _servicePromociones.AddAsync(dto);
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {

                ViewBag.ListCategoria = await _serviceCategorias.ListAsync(); // 🔹 Recargar lista
                return View(dto);
            }
        }


        public async Task<IActionResult> Edit(int id)
        {
            var @object = await _servicePromociones.FindByIdAsync(id);
            if (@object.IdCategoria != null)
            {
                if (@object.IdCategoria.HasValue)
                {
                    var categoria = await _serviceCategorias.FindByIdAsync(@object.IdCategoria.Value);
                    @object.NombreCategoria = categoria?.NombreCategoria;

                }   
            }
            
            if (@object.IdProducto != null)
            {

            }
            return View(@object);
        }


        // POST: ProductosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PromocionesDTO dto)
        {

            if(dto.IdCategoria == null|| dto.BuscarProducto == null)
            {
                ModelState.Remove("BuscarProducto");
                ModelState.Remove("IdCategoria");
                ModelState.Remove("NombreCategoria");
            }

            if(dto.IdProducto == null)
            {
                ModelState.Remove("IdProducto");
            }
            if (dto.FechaInicio < DateTime.Today)
            {
                ModelState.AddModelError("FechaInicio", "La fecha seleccionada no puede ser menor que la fecha de hoy.");
            }
            else
            {
                if (dto.FechaFin < dto.FechaInicio)
                {
                    ModelState.AddModelError("FechaFin", "La fecha seleccionada no puede ser menor que la fecha de Inicio.");
                }
                else
                {
                    if (dto.FechaFin == dto.FechaInicio)
                    {
                        ModelState.AddModelError("FechaFin", "La fecha seleccionada no puede ser igual que la fecha de Inicio, tiene que ser una promocion de un día para otro");
                    }
                }
            }
            if (!ModelState.IsValid)
            {
                dto.IdProducto = dto.IdProducto;
                
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));
                ViewBag.ErrorMessage = errors;
                return View(dto);
            }
            // 5. Actualizar producto y colecciones con método que cargue bien el entity desde BD
            await _servicePromociones.UpdateAsync(id, dto);

            return RedirectToAction("Index");
        }


        // GET: LibroController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var @object = await _servicePromociones.FindByIdAsync(id);
            return View(@object);
        }

        // POST: LibroController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            await _servicePromociones.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
