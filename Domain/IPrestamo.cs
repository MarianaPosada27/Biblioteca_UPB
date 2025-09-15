using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_UPB.Domain
{
    public interface IPrestamo
    {
        string IdPrestamo { get; }
        Libro Libro { get; }
        Estudiante Estudiante { get; }
        DateTime FechaPrestamo { get; }
        DateTime FechaVencimiento { get; set; }
        DateTime? FechaDevolucion { get; set; }

        bool EstaActivo { get; }
        bool EstaVencido { get; }

        void Devolver();
        string MostrarInfo();
    }
}
