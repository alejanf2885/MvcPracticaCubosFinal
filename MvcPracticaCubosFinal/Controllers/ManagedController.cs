using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcPracticaCubosFinal.Models;
using MvcPracticaCubosFinal.Repositories;
using System.Security.Claims;

namespace MvcPracticaCubosFinal.Controllers
{
    public class ManagedController : Controller
    {
        private UsuarioRepository _usuarioRepository;
        public ManagedController(UsuarioRepository usuarioRepository)
        {
            this._usuarioRepository = usuarioRepository;
        }

        public IActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            Usuario usuario = await this._usuarioRepository.LogInUsuarioAsync(email, password);
            if (usuario != null)
            {
                ClaimsIdentity identity =
                    new ClaimsIdentity(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role
                    );
                Claim claimName = new Claim(ClaimTypes.Name, usuario.Nombre);
                identity.AddClaim(claimName);
                Claim claimIdEmp = new Claim("IdUsuario", usuario.Id.ToString());
                identity.AddClaim(claimIdEmp);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

                string controller = TempData["controller"]?.ToString();
                string action = TempData["action"]?.ToString();
                string id = TempData["id"]?.ToString();

                if (!string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(action))
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        return RedirectToAction(action, controller, new { id = id });
                    }
                    return RedirectToAction(action, controller);
                }

                return RedirectToAction("Index", "Home");
            }

            ViewData["ERROR"] = "Email o contraseña invalidos";
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                await this._usuarioRepository.CreateUsuarioAsync(usuario);
                return RedirectToAction("Index", "Home");
            }
            return View(usuario);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Denied()
        {
            return View();
        }
    }
}