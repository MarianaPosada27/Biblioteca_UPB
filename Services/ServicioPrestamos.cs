using Biblioteca_UPB.Common;
using Biblioteca_UPB.Domain;


namespace Biblioteca_UPB.Services
{
    public class ServicioPrestamos
    {
        private readonly List<Prestamo> _prestamos = new();

        public int DiasPrestamoPorDefecto { get; set; } = ReglasPrestamo.DiasPrestamoPorDefecto;

        public event EventHandler<PrestamoEventArgs>? PrestamoRealizado;
        public event EventHandler<PrestamoEventArgs>? PrestamoDevuelto;

        // Prestar un libro
        public Prestamo RealizarPrestamo(Libro libro, Estudiante estudiante)
        {
            ValidarPrestamo(libro, estudiante);

            var prestamo = new Prestamo(libro, estudiante, DiasPrestamoPorDefecto);
            _prestamos.Add(prestamo);

            libro.Stock--;
            libro.Disponible = libro.Stock > 0;

            PrestamoRealizado?.Invoke(this, new PrestamoEventArgs(prestamo, libro, estudiante));

            return prestamo;
        }

        // Devolver préstamo
        public void DevolverPrestamo(string idPrestamo, Libro libro)
        {
            var prestamo = _prestamos.FirstOrDefault(p => p.IdPrestamo == idPrestamo && p.EstaActivo)
                ?? throw new InvalidOperationException("Préstamo no encontrado o ya devuelto.");

            Console.WriteLine($"Días de préstamo: {ReglasPrestamo.CalcularDiasPrestamo(prestamo.FechaPrestamo)}");

            prestamo.Devolver();
            libro.Stock++;
            libro.Disponible = true;

            PrestamoDevuelto?.Invoke(this, new PrestamoEventArgs(prestamo, libro, null));
        }
        

        public List<Prestamo> ObtenerPrestamosActivos()
            => _prestamos.Where(p => p.EstaActivo).OrderBy(p => p.FechaVencimiento).ToList();

        // Validaciones privadas
        private void ValidarPrestamo(Libro libro, Estudiante estudiante)
        {
            if (libro.Stock <= 0)
                throw new InvalidOperationException("No hay ejemplares disponibles.");

            if (_prestamos.Any(p => p.Libro.IdLibro == libro.IdLibro && p.Estudiante.Documento == estudiante.Documento && p.EstaActivo))
                throw new InvalidOperationException("El estudiante ya tiene este libro prestado.");

            if (_prestamos.Any(p => p.Estudiante.Documento == estudiante.Documento && p.EstaVencido))
                throw new InvalidOperationException("El estudiante tiene préstamos vencidos.");
        }
       
    }
    public class PrestamoEventArgs : EventArgs
    {
        public Prestamo Prestamo { get; }
        public Libro? Libro { get; }
        public Estudiante? Estudiante { get; }

        public PrestamoEventArgs(Prestamo prestamo, Libro? libro, Estudiante? estudiante)
        {
            Prestamo = prestamo;
            Libro = libro;
            Estudiante = estudiante;
        }
    }
}
