using FKNI.Application.Config;
using FKNI.Application.Services.Implementations;
using FKNI.Application.Services.Interfaces;
using FKNI.Web;
using FKNI.Web.Util;
using FKNI.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FKNI.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IServiceUsuarios _serviceUsuarios;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IServiceUsuarios serviceUsuarios, ILogger<LoginController> logger)
        {
            _serviceUsuarios = serviceUsuarios;
            _logger = logger;
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> LogIn(ViewModelLogin viewModelLogin)
        {

            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));

                ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Login", $"Error de Acceso {errors}", SweetAlertMessageType.error);

                _logger.LogInformation($"Error en login de {viewModelLogin}, Errores --> {errors}");
                return View("Index");
            }
            //Verificar sí el usuario existe
            var usuarioDTO = await _serviceUsuarios.LoginAsync(viewModelLogin.User, viewModelLogin.Password);
            if (usuarioDTO == null)
            {
                ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Login", "Error de Acceso", SweetAlertMessageType.error);
                _logger.LogInformation($"Error en login de {viewModelLogin.User}, Error --> Error en acceso");
                return View("Index");
            }

            //Claim almacena información del usuario como nombre, rol y otros.
            List<Claim> claims = new List<Claim>() {

                new Claim(ClaimTypes.Name, usuarioDTO.NombreUsuario),
                new Claim(ClaimTypes.Role, usuarioDTO.IdRolNavigation.NombreRol!)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties);

            _logger.LogInformation($"Conexion correcta de {viewModelLogin.User}");
            TempData["Message"] = Util.SweetAlertHelper.Mensaje("Login", "Usuario identificado", SweetAlertMessageType.success);

            return RedirectToAction("Index", "Home");
        }

        /* Solo la usuario conectada puede cerrar sesión */
        [Authorize]
        public async Task<IActionResult> LogOff()
        {
            _logger.LogInformation($"Desconexion correcta de {User!.Identity!.Name}");
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Forbidden()
        {
            return View();
        }
    }
}
