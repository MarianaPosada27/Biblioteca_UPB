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



        // Método para calcular cantidad de dias que el estudiante tiene un libro hasta su devolución
        public static int CalcularDiasPrestamo(DateTime fechaprestamo)
        {
            return (DateTime.Now - fechaprestamo).Days;  
        }
    }
}
