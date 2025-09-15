using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_UPB.Domain
{
    public class Prestamo : IPrestamo
    {
        public Libro Libro { get; init; }
        public Estudiante Estudiante { get; init; }
        public DateTime FechaPrestamo { get; init; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime? FechaDevolucion { get; set; }
     
        public bool EstaActivo => FechaDevolucion == null;
        public bool EstaVencido => EstaActivo && DateTime.Now > FechaVencimiento;

        private static int contador = 0; // contador compartido por todos los préstamos
        public string IdPrestamo { get; init; }

        public Prestamo(Libro idLibro, Estudiante idEstudiante, int diasPrestamo = 15)
        {
            contador++;
            IdPrestamo = $"P{contador:D3}"; // P001, P002, P003...
            Libro = idLibro ?? throw new ArgumentNullException(nameof(idLibro));
            Estudiante = idEstudiante ?? throw new ArgumentNullException(nameof(idEstudiante));
            FechaPrestamo = DateTime.Now.AddDays(-1);
            FechaVencimiento = DateTime.Now.AddDays(diasPrestamo);
            ;
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
            var info = $"ID: {IdPrestamo} | Libro: {Libro.IdLibro} - {Libro.Titulo} | Estudiante: {Estudiante.Documento} - {Estudiante.Nombre} - {Estudiante.Carrera} - {Estudiante.email}\n";
            info += $"Estado: {estado} | Prestado: {FechaPrestamo:dd/MM/yyyy} | Vence: {FechaVencimiento:dd/MM/yyyy}\n";
            

            if (FechaDevolucion.HasValue)
                info += $" | Devuelto: {FechaDevolucion:dd/MM/yyyy}";

            return info;
        }
    }
}

