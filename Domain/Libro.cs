using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_UPB.Domain
{
    public class Libro 
    {
        // Propiedades básicas del libro
        public string IdLibro { get; set; }
        public string Titulo { get; private set; }
        public Autor Autor { get; set; }
        public bool Disponible { get; set; } = true;
        public int Stock { get;  set; }
       

        // Propiedades para control de préstamos
        public List<string> UsuariosPrestamo { get; private set; } = new List<string>();
        public Dictionary<string, DateTime> FechasPrestamo { get; private set; } = new Dictionary<string, DateTime>();
        public Dictionary<string, DateTime> FechasVencimiento { get; private set; } = new Dictionary<string, DateTime>();
        public Dictionary<string, int> NumeroRenovaciones { get; private set; } = new Dictionary<string, int>();

        // Propiedades para reservas
        public Queue<string> ColaReservas { get; private set; } = new Queue<string>();
        public Dictionary<string, DateTime> FechasReserva { get; private set; } = new Dictionary<string, DateTime>();

        // Configuración del sistema
        public int DiasPrestamo { get; init; } = 15;  // Días por defecto del préstamo

        public event EventHandler? StockAgotado;

        // Constructor
        public Libro(string idLibro, string titulo, Autor autor, int diasPrestamo = 15, int maximoRenovaciones = 2)
        {
            if (string.IsNullOrWhiteSpace(idLibro))
                throw new ArgumentException("El ID del libro no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("El título no puede estar vacío.");
            if (autor == null)
                throw new ArgumentNullException(nameof(autor), "El autor no puede ser nulo.");
            if (diasPrestamo < 1)
                throw new ArgumentException("Los días de préstamo deben ser mayor a 0.");

            IdLibro = idLibro.Trim().ToUpper();
            Titulo = titulo.Trim().ToUpper();
            Autor = autor;
            Stock = new Random().Next(1,5);
            DiasPrestamo = diasPrestamo;
        }

        // Método para prestar un libro      
        public void Prestar(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("Debe especificar el usuario que solicita el préstamo.");

            if (Stock <= 0)
                throw new InvalidOperationException("No hay Libros disponibles para prestar.");

            if (UsuariosPrestamo.Contains(usuario))
                throw new InvalidOperationException($"El usuario {usuario} ya tiene este libro prestado.");

            // Registrar el préstamo
            Stock--;
            UsuariosPrestamo.Add(usuario);
            FechasPrestamo[usuario] = DateTime.Now;
            FechasVencimiento[usuario] = DateTime.Now.AddDays(DiasPrestamo);
            NumeroRenovaciones[usuario] = 0;

            // Actualizar disponibilidad
            Disponible = Stock > 0;

        }

        // Método para devolver un libro
        public void Devolver(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("Debe especificar el usuario que devuelve el libro.");

            if (!UsuariosPrestamo.Contains(usuario))
                throw new InvalidOperationException($"El usuario {usuario} no tiene este libro prestado.");

            // Remover el préstamo
            Stock++;
            UsuariosPrestamo.Remove(usuario);
            FechasPrestamo.Remove(usuario);
            FechasVencimiento.Remove(usuario);
            NumeroRenovaciones.Remove(usuario);

            // Actualizar disponibilidad
            Disponible = true;

            Console.WriteLine($"Libro '{Titulo}' devuelto por {usuario}. Stock actual: {Stock}");
        }

        // Método para obtener información completa del libro
        public string MostrarInfo()
        {
            var info = $"ID: {IdLibro} | Título: {Titulo} | Autor: {Autor}\n";
            info += $"Stock: {Stock} | Disponible: {(Disponible ? "Sí" : "No")}\n";

            if (UsuariosPrestamo.Count > 0)
            {
                info += "Préstamos activos:\n";
                foreach (var usuario in UsuariosPrestamo)
                {
                    info += $"  - {usuario}: Vence {FechasVencimiento[usuario]:dd/MM/yyyy} (Renovaciones: {NumeroRenovaciones[usuario]})\n";
                }
            }

            if (ColaReservas.Count > 0)
            {
                info += $"Reservas en cola: {string.Join(", ", ColaReservas)}\n";
            }

            return info.TrimEnd();
        }

    }
}