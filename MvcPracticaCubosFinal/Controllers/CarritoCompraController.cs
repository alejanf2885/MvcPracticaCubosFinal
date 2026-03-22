using Microsoft.AspNetCore.Mvc;
using MvcPracticaCubosFinal.Dtos;
using MvcPracticaCubosFinal.Extensions;
using MvcPracticaCubosFinal.Filters;
using MvcPracticaCubosFinal.Models;
using MvcPracticaCubosFinal.Repositories;

namespace MvcPracticaCubosFinal.Controllers
{
    public class CarritoCompraController : Controller
    {
        private IHttpContextAccessor httpContextAccessor;
        private CuboRepository _cuboRepository;
        public CarritoCompraController(IHttpContextAccessor httpContextAccessor, CuboRepository cuboRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            _cuboRepository = cuboRepository;
        }

        public async Task<IActionResult> Index()
        {

            List<CuboCarrito> carrito = this.httpContextAccessor.HttpContext.Session.GetObject<List<CuboCarrito>>("CARRITO");

            List<CuboCarritoDto> carritoDto = new List<CuboCarritoDto>();
            if (carrito == null || carrito.Count == 0)
            {
                return View(carritoDto);
            }
            else
            {
                foreach (CuboCarrito cubo in carrito)
                {
                    Cubo cuboInfo = await _cuboRepository.GetCuboAsync(cubo.IdCubo);
                    if (cuboInfo != null)
                    {
                        CuboCarritoDto cuboCarritoDto = new CuboCarritoDto
                        {
                            Cubo = cuboInfo,
                            Cantidad = cubo.Cantidad
                        };
                        carritoDto.Add(cuboCarritoDto);
                    }
                }
                return View(carritoDto);
            }

        }

        [AuthorizeUsuarios]

        public async Task<IActionResult> AddToCart(int id)
        {

            //Conseguir el carrito de la sesión
            List<CuboCarrito> carrito = this.httpContextAccessor.HttpContext.Session.GetObject<List<CuboCarrito>>("CARRITO");

            if (carrito == null || carrito.Count == 0)
            {
                carrito = new List<CuboCarrito>();
            }

            //Comprobar si el cubo ya está en el carrito
            CuboCarrito cuboCarrito = carrito.FirstOrDefault(c => c.IdCubo == id);
            if (cuboCarrito != null)
            {
                cuboCarrito.Cantidad++;
            }
            else
            {
                carrito.Add(new CuboCarrito { IdCubo = id, Cantidad = 1 });
            }

            this.httpContextAccessor.HttpContext.Session.SetObject("CARRITO", carrito);

            return RedirectToAction("Index");
        }

        [AuthorizeUsuarios]

        public async Task<IActionResult> RemoveFromCart(int id)
        {
            List<CuboCarrito> carrito = this.httpContextAccessor.HttpContext.Session.GetObject<List<CuboCarrito>>("CARRITO");
            if (carrito != null && carrito.Count > 0)
            {
                CuboCarrito cuboCarrito = carrito.FirstOrDefault(c => c.IdCubo == id);
                if (cuboCarrito != null)
                {
                    carrito.Remove(cuboCarrito);
                    this.httpContextAccessor.HttpContext.Session.SetObject("CARRITO", carrito);
                }

            }
            return RedirectToAction("Index");
        }

        [AuthorizeUsuarios]

        public async Task<IActionResult> RemoveOne(int id)
        {
            List<CuboCarrito> carrito = this.httpContextAccessor.HttpContext.Session.GetObject<List<CuboCarrito>>("CARRITO");

            if (carrito == null || carrito.Count == 0)
            {
                carrito = new List<CuboCarrito>();
            }

            CuboCarrito cuboCarrito = carrito.FirstOrDefault(c => c.IdCubo == id);
            if (cuboCarrito != null)
            {
                if (cuboCarrito.Cantidad > 1)
                {
                    cuboCarrito.Cantidad--;
                }
                else
                {
                    carrito.Remove(cuboCarrito);
                }
            }


            this.httpContextAccessor.HttpContext.Session.SetObject("CARRITO", carrito);

            return RedirectToAction("Index");
        }
    }
}
