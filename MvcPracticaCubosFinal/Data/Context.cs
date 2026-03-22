using Microsoft.EntityFrameworkCore;
using MvcPracticaCubosFinal.Models;

namespace MvcPracticaCubosFinal.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options) { }

        public DbSet<Cubo> Cubos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Compra> Compras { get; set; }
    }
}
