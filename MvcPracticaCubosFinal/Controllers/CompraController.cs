using Microsoft.AspNetCore.Mvc;
using MvcPracticaCubosFinal.Extensions;
using MvcPracticaCubosFinal.Filters;
using MvcPracticaCubosFinal.Models;
using MvcPracticaCubosFinal.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcPracticaCubosFinal.Controllers
{
    public class CompraController : Controller
    {
        private CompraRepository _compraRepository;
        private IHttpContextAccessor httpContextAccessor;
        private CuboRepository _cuboRepository;

        public CompraController(CompraRepository compraRepository, IHttpContextAccessor httpContextAccessor, CuboRepository cuboRepository)
        {
            _compraRepository = compraRepository;
            this.httpContextAccessor = httpContextAccessor;
            _cuboRepository = cuboRepository;
        }

        [AuthorizeUsuarios(Policy = "HasCompras")]
        public async Task<IActionResult> Index()
        {

            string idUsuario = User.FindFirstValue("IdUsuario");
            int id = int.Parse(idUsuario);


            List<Compra> compras = await _compraRepository.GetComprasAsync(id);
            return View(compras);
        }


        public async Task<IActionResult> Compra()
        {
            string idUsuario = User.FindFirstValue("IdUsuario");
            int id = int.Parse(idUsuario);
            List<CuboCarrito> carrito = this.httpContextAccessor.HttpContext.Session.GetObject<List<CuboCarrito>>("CARRITO");

            int idCompra = await this._compraRepository.GetIdCompraMaxAsync() + 1;
            if (carrito != null && carrito.Count > 0)
            {
                DateTime fecha = DateTime.Now;
                List<Compra> compras = new List<Compra>();

                foreach (var item in carrito)
                {
                    Cubo cubo = await this._cuboRepository.GetCuboAsync(item.IdCubo);
                    Compra compra = new Compra
                    {
                        IdCompra = idCompra,
                        IdCubo = item.IdCubo,
                        Cantidad = item.Cantidad,
                        Precio = cubo.Precio * item.Cantidad,
                        FechaPedido = fecha,
                        IdUsuario = id
                    };
                    await this._compraRepository.CreateCompraAsync(compra);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
