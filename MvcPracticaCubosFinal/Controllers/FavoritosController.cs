using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MvcPracticaCubosFinal.Filters;
using MvcPracticaCubosFinal.Models;
using MvcPracticaCubosFinal.Repositories;
using System.Threading.Tasks;

namespace MvcPracticaCubosFinal.Controllers
{
    public class FavoritosController : Controller
    {

        private IMemoryCache memoryCache;

        private CuboRepository cuboRepository;

        public FavoritosController(IMemoryCache memoryCache, CuboRepository cuboRepository)
        {
            this.memoryCache = memoryCache;
            this.cuboRepository = cuboRepository;
        }

        [AuthorizeUsuarios(Policy = "HasFavoritos")]
        public IActionResult Index()
        {
            List<Cubo> cubos = this.memoryCache.Get<List<Cubo>>("FAVORITOS");
            return View(cubos);
        }
        public async Task<IActionResult> AddFavorito(int id)
        {
            Cubo cubo = await this.cuboRepository.GetCuboAsync(id);
            List<Cubo> cubos = this.memoryCache.Get<List<Cubo>>("FAVORITOS");

            if (cubos == null)
            {
                cubos = new List<Cubo>();
            }

            if (!(cubos.Any(z => z.IdCubo == id)))
            {
                cubos.Add(cubo);
                this.memoryCache.Set("FAVORITOS", cubos);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            List<Cubo> cubos = this.memoryCache.Get<List<Cubo>>("FAVORITOS");

            if (cubos != null)
            {
                Cubo cuboEliminar = cubos.FirstOrDefault(z => z.IdCubo == id);

                if (cuboEliminar != null)
                {
                    cubos.Remove(cuboEliminar);
                    this.memoryCache.Set("FAVORITOS", cubos);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
