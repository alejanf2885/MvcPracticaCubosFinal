namespace MvcPracticaCubosFinal.Dtos
{
    public class CuboDto
    {
        public int IdCubo { get; set; }
        public string Nombre { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }

        public IFormFile ImagenFile { get; set; }

        public string Imagen { get; set; }

        public int Precio { get; set; }
    }
}
