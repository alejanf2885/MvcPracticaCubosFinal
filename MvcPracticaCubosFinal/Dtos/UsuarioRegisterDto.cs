namespace MvcPracticaCubosFinal.Dtos
{
    public class UsuarioRegisterDto
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IFormFile Foto { get; set; }
    }
}
