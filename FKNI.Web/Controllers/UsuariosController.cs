using FKNI.Application.DTOs;
using FKNI.Application.Services.Implementations;
using FKNI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace FKNI.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IServiceUsuarios _serviceUsuarios;
        public UsuariosController(IServiceUsuarios serviceUsuarios)
        {
            _serviceUsuarios = serviceUsuarios;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var collection = await _serviceUsuarios.ListAsync();
            return View(collection);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuariosDTO dto)
        {

            // Si el usuario NO es administrador, asignar rol cliente por defecto
            if (!User.IsInRole("Administrador"))
            {
                dto.IdRol = 2; // Rol Cliente
            }

            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));

                return BadRequest(errors);
            }

            try
            {
                await _serviceUsuarios.AddAsync(dto);
                
                if (User.IsInRole("Administrador"))
                {
                    return RedirectToAction("Index", "Usuarios");
                }
                return RedirectToAction("Index", "Login");
            }
            catch (DbUpdateException ex)
            {
                // Extraer el mensaje de error más interno
                var innerMessage = ex.InnerException?.Message ?? ex.Message;

                // Agregar el error al ModelState para mostrarlo en la vista
                ModelState.AddModelError(string.Empty, $"Error al guardar en la base de datos: {innerMessage}");

                // Retornar la vista con el dto para que el usuario pueda corregir
                return View(dto);
            }
            catch (Exception ex)
            {
                // Manejo genérico de cualquier otra excepción
                ModelState.AddModelError(string.Empty, $"Ocurrió un error inesperado: {ex.Message}");
                return View(dto);
            }
        }




        // GET:  
        public async Task<IActionResult> Login(string id, string password)
        {
            var @object = await _serviceUsuarios.LoginAsync(id, password);
            if (@object == null)
            {
                ViewBag.Message = "Error en Login o Password";
                return View("Login");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }

    }
}
