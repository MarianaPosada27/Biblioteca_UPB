using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_UPB.Common
{
    public static class ReglasPrestamo
    {
        // Constantes
            public const int DiasPrestamoPorDefecto = 15;
            public const int MaximoRenovaciones = 2;
            public const int MaximoLibrosPorEstudiante = 3;
            public const decimal MultaPorDiaRetraso = 500m;

        // Método para calcular multa
        public static decimal CalcularMulta(int diasAtraso)
        {
            if (diasAtraso < 0)
                throw new ArgumentException("Los días de atraso no pueden ser negativos.");

            return diasAtraso * MultaPorDiaRetraso;
        }

        // Método para calcular nueva fecha de vencimiento
        public static DateTime CalcularNuevaFecha(DateTime fechaActual)
        {
            return fechaActual.AddDays(DiasPrestamoPorDefecto);
        }
    }
}
