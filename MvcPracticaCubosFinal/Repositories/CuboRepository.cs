using Microsoft.EntityFrameworkCore;
using MvcPracticaCubosFinal.Data;
using MvcPracticaCubosFinal.Models;
using System.Threading.Tasks;

namespace MvcPracticaCubosFinal.Repositories
{
    public class CuboRepository
    {
        private Context _context;

        public CuboRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<Cubo>> GetCubosAsync()
        {
            var consulta = from datos in _context.Cubos
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<Cubo> GetCuboAsync(int id)
        {
            var consulta = from datos in _context.Cubos
                           where datos.IdCubo == id
                           select datos;
            return await consulta.FirstOrDefaultAsync();

        }

        public async Task InsertCuboAsync(Cubo cubo)
        {
            _context.Cubos.Add(cubo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCuboAsync(Cubo cubo)
        {
            _context.Cubos.Update(cubo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCuboAsync(int id)
        {
            Cubo cubo = await GetCuboAsync(id);
            _context.Cubos.Remove(cubo);
            await _context.SaveChangesAsync();
        }

    }
}
