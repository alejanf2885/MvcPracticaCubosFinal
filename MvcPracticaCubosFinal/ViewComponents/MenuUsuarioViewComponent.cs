using Microsoft.AspNetCore.Mvc;
using MvcPracticaCubosFinal.Models;
using System.Security.Claims;

namespace MvcPracticaCubosFinal.ViewComponents
{
    public class MenuUsuarioViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            Usuario usuario = null;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                usuario = new Usuario();

                string idString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(idString))
                {
                    usuario.Id = int.Parse(idString);
                }

                usuario.Nombre = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                usuario.Email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

                usuario.Foto = HttpContext.User.FindFirst("Foto")?.Value;
            }

            return View(usuario);
        }
    }
}