using Microsoft.AspNetCore.Mvc;
using MvcPracticaCubosFinal.Dtos;
using MvcPracticaCubosFinal.Models;
using MvcPracticaCubosFinal.Repositories;
using System.Threading.Tasks;

namespace MvcPracticaCubosFinal.Controllers
{
    public class CubosController : Controller
    {
        private CuboRepository _cuboRepository;
        private IWebHostEnvironment _hostEnvironment;
        public CubosController(CuboRepository cuboRepository, IWebHostEnvironment hostEnvironment)
        {
            _cuboRepository = cuboRepository;
            _hostEnvironment = hostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            List<Cubo> cubos = await this._cuboRepository.GetCubosAsync();
            return View(cubos);
        }
        public async Task<IActionResult> Details(int id)
        {
            Cubo cubo = await this._cuboRepository.GetCuboAsync(id);
            return View(cubo);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CuboDto cuboDto)
        {
            Cubo cubo = new Cubo
            {
                IdCubo = cuboDto.IdCubo,
                Nombre = cuboDto.Nombre,
                Modelo = cuboDto.Modelo,
                Marca = cuboDto.Marca,
                Precio = cuboDto.Precio
            };

            if (cuboDto.ImagenFile != null && cuboDto.ImagenFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(this._hostEnvironment.WebRootPath, "images");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                Guid guid = Guid.NewGuid();
                string extension = Path.GetExtension(cuboDto.ImagenFile.FileName);
                string fileName = guid.ToString() + extension;
                string path = Path.Combine(uploadsFolder, fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await cuboDto.ImagenFile.CopyToAsync(stream);
                }

                cubo.Imagen = fileName;
            }

            await this._cuboRepository.InsertCuboAsync(cubo);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            Cubo cubo = await this._cuboRepository.GetCuboAsync(id);
            CuboDto cuboDto = new CuboDto
            {
                IdCubo = cubo.IdCubo,
                Nombre = cubo.Nombre,
                Modelo = cubo.Modelo,
                Marca = cubo.Marca,
                Precio = cubo.Precio,
                Imagen = cubo.Imagen

            };
            return View(cuboDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CuboDto cuboDto)
        {
            Cubo cubo = await this._cuboRepository.GetCuboAsync(cuboDto.IdCubo);

            cubo.Nombre = cuboDto.Nombre;
            cubo.Modelo = cuboDto.Modelo;
            cubo.Marca = cuboDto.Marca;
            cubo.Precio = cuboDto.Precio;

            if (cuboDto.ImagenFile != null && cuboDto.ImagenFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(this._hostEnvironment.WebRootPath, "images");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                Guid guid = Guid.NewGuid();
                string extension = Path.GetExtension(cuboDto.ImagenFile.FileName);
                string fileName = guid.ToString() + extension;
                string path = Path.Combine(uploadsFolder, fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await cuboDto.ImagenFile.CopyToAsync(stream);
                }

                cubo.Imagen = fileName;
            }

            await this._cuboRepository.UpdateCuboAsync(cubo);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            
            Cubo cubo = await this._cuboRepository.GetCuboAsync(id);
            if(cubo == null)
            {
                return NotFound();
            }
            else
            {
                await this._cuboRepository.DeleteCuboAsync(id);
            }
                return RedirectToAction("Index");
        }

    }
}
