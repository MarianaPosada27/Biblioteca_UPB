using Biblioteca_UPB.Common;
using Biblioteca_UPB.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_UPB.Services
{
    public class ServicioPrestamos
    {
        private readonly List<Prestamo> _prestamos = new();

        public int DiasPrestamoPorDefecto { get; set; } = ReglasPrestamo.DiasPrestamoPorDefecto;
        public int MaximoRenovaciones { get; set; } = ReglasPrestamo.MaximoRenovaciones;
        public int MaximoLibrosPorEstudiante { get; set; } = ReglasPrestamo.MaximoLibrosPorEstudiante;

        public event EventHandler<PrestamoEventArgs>? PrestamoRealizado;
        public event EventHandler<PrestamoEventArgs>? PrestamoDevuelto;
        public event EventHandler<PrestamoEventArgs>? PrestamoRenovado;

        // Prestar un libro
        public Prestamo RealizarPrestamo(Libro libro, Estudiante estudiante)
        {
            ValidarPrestamo(libro, estudiante);

            var prestamo = new Prestamo(libro.IdLibro, estudiante.Documento, DiasPrestamoPorDefecto);
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

            prestamo.Devolver();
            libro.Stock++;
            libro.Disponible = true;

            PrestamoDevuelto?.Invoke(this, new PrestamoEventArgs(prestamo, libro, null));
        }

        // Renovar préstamo
        public void RenovarPrestamo(string idPrestamo)
        {
            var prestamo = _prestamos.FirstOrDefault(p => p.IdPrestamo == idPrestamo && p.EstaActivo)
                ?? throw new InvalidOperationException("Préstamo no encontrado o ya devuelto.");

            prestamo.Renovar(DiasPrestamoPorDefecto, MaximoRenovaciones);
            PrestamoRenovado?.Invoke(this, new PrestamoEventArgs(prestamo, null, null));
        }

        // Consultas
        public List<Prestamo> ObtenerPrestamosEstudiante(string idEstudiante, bool soloActivos = true)
        {
            var query = _prestamos.Where(p => p.IdEstudiante == idEstudiante);
            if (soloActivos) query = query.Where(p => p.EstaActivo);
            return query.OrderBy(p => p.FechaPrestamo).ToList();
        }

        public List<Prestamo> ObtenerPrestamosLibro(string idLibro, bool soloActivos = true)
        {
            var query = _prestamos.Where(p => p.IdLibro == idLibro);
            if (soloActivos) query = query.Where(p => p.EstaActivo);
            return query.OrderBy(p => p.FechaPrestamo).ToList();
        }

        public List<Prestamo> ObtenerPrestamosVencidos()
            => _prestamos.Where(p => p.EstaVencido).OrderBy(p => p.FechaVencimiento).ToList();

        public List<Prestamo> ObtenerPrestamosActivos()
            => _prestamos.Where(p => p.EstaActivo).OrderBy(p => p.FechaVencimiento).ToList();

        // Validaciones privadas
        private void ValidarPrestamo(Libro libro, Estudiante estudiante)
        {
            if (libro.Stock <= 0)
                throw new InvalidOperationException("No hay ejemplares disponibles.");

            if (ObtenerPrestamosEstudiante(estudiante.Documento).Count >= MaximoLibrosPorEstudiante)
                throw new InvalidOperationException($"El estudiante ya tiene {MaximoLibrosPorEstudiante} libros prestados.");

            if (_prestamos.Any(p => p.IdLibro == libro.IdLibro && p.IdEstudiante == estudiante.Documento && p.EstaActivo))
                throw new InvalidOperationException("El estudiante ya tiene este libro prestado.");

            if (_prestamos.Any(p => p.IdEstudiante == estudiante.Documento && p.EstaVencido))
                throw new InvalidOperationException("El estudiante tiene préstamos vencidos.");
        }
        // Obtener historial completo de préstamos (activos y devueltos)
        public List<Prestamo> ObtenerHistorialPrestamos()
        {
            return _prestamos
                .OrderBy(p => p.FechaPrestamo)
                .ToList();
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
