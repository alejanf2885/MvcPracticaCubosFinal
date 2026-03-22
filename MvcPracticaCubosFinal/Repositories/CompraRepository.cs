using Microsoft.EntityFrameworkCore;
using MvcPracticaCubosFinal.Data;
using MvcPracticaCubosFinal.Models;

namespace MvcPracticaCubosFinal.Repositories
{
    public class CompraRepository
    {
        private Context _context;
        private CuboRepository _cuboRepository;

        public CompraRepository(Context context, CuboRepository cuboRepository)
        {
            _context = context;
            _cuboRepository = cuboRepository;
        }


        public async Task<List<Compra>> GetComprasAsync(int idUsuario)
        {
            var consulta = await this._context.Compras
                                .Where(z => z.IdUsuario == idUsuario)
                                .ToListAsync();

            return consulta;
        }

        public async Task<int> GetIdCompraMaxAsync()
        {
            int maxIdCompra = await this._context.Compras.MaxAsync(z => z.IdCompra);
            return maxIdCompra;
        }




        public async Task CreateCompraAsync(Compra compra)
        {

            this._context.Compras.Add(compra);

            await this._context.SaveChangesAsync();

           
            this._context.Entry(compra).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

        }
    }
}
