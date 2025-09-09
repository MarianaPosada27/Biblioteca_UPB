using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_UPB.Domain
{
    public class Prestamo
    {
        public string IdLibro { get; init; }
        public string IdEstudiante { get; init; }
        public DateTime FechaPrestamo { get; init; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public int NumeroRenovaciones { get; set; }
        public bool EstaActivo => FechaDevolucion == null;
        public bool EstaVencido => EstaActivo && DateTime.Now > FechaVencimiento;

        private static int contador = 0; // contador compartido por todos los préstamos
        public string IdPrestamo { get; init; }

        public Prestamo(string idLibro, string idEstudiante, int diasPrestamo = 15)
        {
            contador++;
            IdPrestamo = $"P{contador:D3}"; // P001, P002, P003...
            IdLibro = idLibro ?? throw new ArgumentNullException(nameof(idLibro));
            IdEstudiante = idEstudiante ?? throw new ArgumentNullException(nameof(idEstudiante));
            FechaPrestamo = DateTime.Now;
            FechaVencimiento = DateTime.Now.AddDays(diasPrestamo);
            NumeroRenovaciones = 0;
        }


        public void Renovar(int diasAdicionales = 15, int maxRenovaciones = 2)
        {
            if (NumeroRenovaciones >= maxRenovaciones)
                throw new InvalidOperationException($"Se alcanzó el límite de {maxRenovaciones} renovaciones.");

            if (!EstaActivo)
                throw new InvalidOperationException("No se puede renovar un préstamo ya devuelto.");

            FechaVencimiento = DateTime.Now.AddDays(diasAdicionales);
            NumeroRenovaciones++;
        }

        public void Devolver()
        {
            if (!EstaActivo)
                throw new InvalidOperationException("Este préstamo ya fue devuelto.");

            FechaDevolucion = DateTime.Now;
        }

        public string MostrarInfo()
        {
            var estado = EstaActivo ? (EstaVencido ? "VENCIDO" : "ACTIVO") : "DEVUELTO";
            var info = $"ID: {IdPrestamo} | Libro: {IdLibro} | Estudiante: {IdEstudiante}\n";
            info += $"Estado: {estado} | Prestado: {FechaPrestamo:dd/MM/yyyy} | Vence: {FechaVencimiento:dd/MM/yyyy}\n";
            info += $"Renovaciones: {NumeroRenovaciones}";

            if (FechaDevolucion.HasValue)
                info += $" | Devuelto: {FechaDevolucion:dd/MM/yyyy}";

            return info;
        }
    }
}

