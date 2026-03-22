using MvcPracticaCubosFinal.Models;

namespace MvcPracticaCubosFinal.Dtos
{
    public class CuboCarritoDto
    {
        public Cubo Cubo { get; set; }
        public int Cantidad { get; set; }

        public int Subtotal
        {
            get
            {
                if (Cubo != null)
                {
                    return Cubo.Precio * Cantidad;
                }

                return 0;
            }
        }
    }
}
