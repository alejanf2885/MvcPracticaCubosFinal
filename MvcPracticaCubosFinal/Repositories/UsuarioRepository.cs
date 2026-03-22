using Microsoft.EntityFrameworkCore;
using MvcPracticaCubosFinal.Data;
using MvcPracticaCubosFinal.Models;

namespace MvcPracticaCubosFinal.Repositories
{
    public class UsuarioRepository
    {
        private Context _context;

        public UsuarioRepository(Context context)
        {
            _context = context;
        }

        public async Task<Usuario> GetUsuarioAsync(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario> GetUsuarioByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task CreateUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario> LogInUsuarioAsync(string email, string password)
        {

            Usuario empleado = await this._context.Usuarios
                .FirstOrDefaultAsync(x => x.Email == email && x.Password == password);

            return empleado;
        }
    }

}
