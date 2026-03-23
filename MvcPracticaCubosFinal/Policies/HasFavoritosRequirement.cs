using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using MvcPracticaCubosFinal.Models;

namespace MvcPracticaCubosFinal.Policies
{
    public class HasFavoritosRequirement : IAuthorizationRequirement { }


    public class HasFavoritosHandler : AuthorizationHandler<HasFavoritosRequirement>
    {
        private IMemoryCache memoryCache;

        public HasFavoritosHandler(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            HasFavoritosRequirement requirement)
        {
            List<Cubo> cubos = this.memoryCache.Get<List<Cubo>>("FAVORITOS");
            if (cubos != null && cubos.Count > 0)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
