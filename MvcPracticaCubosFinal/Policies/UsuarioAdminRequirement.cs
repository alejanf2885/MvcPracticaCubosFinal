using Microsoft.AspNetCore.Authorization;
using MvcPracticaCubosFinal.Repositories;

namespace MvcPracticaCubosFinal.Policies
{
    public class UsuarioAdminRequirement : IAuthorizationRequirement { }


    public class UsuarioAdminHandler : AuthorizationHandler<UsuarioAdminRequirement>
    {
        private UsuarioRepository _usuarioRepository;
        public UsuarioAdminHandler(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            UsuarioAdminRequirement requirement)
        {
            string idString = context.User.FindFirst("IdUsuario")?.Value;
            if (!string.IsNullOrEmpty(idString))
            {
                int idUsuario = int.Parse(idString);
                if (idUsuario == 1)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
