using Microsoft.AspNetCore.Authorization;
using MvcPracticaCubosFinal.Models;
using MvcPracticaCubosFinal.Repositories;

namespace MvcPracticaCubosFinal.Policies
{
    public class HasComprasRequirement : IAuthorizationRequirement { }

    public class HasComprasHandler : AuthorizationHandler<HasComprasRequirement>
    {
        private CompraRepository _compraRepository;
        public HasComprasHandler(CompraRepository compraRepository)
        {
            _compraRepository = compraRepository;
        }
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            HasComprasRequirement requirement)
        {
            string idString = context.User.FindFirst("IdUsuario")?.Value;
            if (!string.IsNullOrEmpty(idString))
            {
                int idUsuario = int.Parse(idString);
                List<Compra> compras = await _compraRepository.GetComprasAsync(idUsuario);
                if (compras != null && compras.Count > 0)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}