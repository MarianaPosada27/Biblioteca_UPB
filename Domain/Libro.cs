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
        public int Ejemplares { get; init; } = 1;

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
        public int MaximoRenovaciones { get; init; } = 2;  // Máximo de renovaciones permitidas

        public event EventHandler? StockAgotado;

        // Constructor
        public Libro(string idLibro, string titulo, Autor autor, int ejemplares, int diasPrestamo = 15, int maximoRenovaciones = 2)
        {
            if (string.IsNullOrWhiteSpace(idLibro))
                throw new ArgumentException("El ID del libro no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("El título no puede estar vacío.");
            if (autor == null)
                throw new ArgumentNullException(nameof(autor), "El autor no puede ser nulo.");
            if (ejemplares < 1)
                throw new ArgumentException("Debe haber al menos un ejemplar disponible.");
            if (diasPrestamo < 1)
                throw new ArgumentException("Los días de préstamo deben ser mayor a 0.");
            if (maximoRenovaciones < 0)
                throw new ArgumentException("El máximo de renovaciones no puede ser negativo.");

            IdLibro = idLibro.Trim().ToUpper();
            Titulo = titulo.Trim().ToUpper();
            Autor = autor;
            Ejemplares = ejemplares;
            Stock = ejemplares;
            DiasPrestamo = diasPrestamo;
            MaximoRenovaciones = maximoRenovaciones;
        }

        // Método para cambiar el título del libro   
        public void CambiarTitulo(string nuevoTitulo)
        {
            if (string.IsNullOrWhiteSpace(nuevoTitulo))
                throw new ArgumentException("El nuevo título no puede estar vacío.");

            Titulo = nuevoTitulo.Trim();
        }

        // Método para prestar un libro      
        public void Prestar(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("Debe especificar el usuario que solicita el préstamo.");

            if (Stock <= 0)
                throw new InvalidOperationException("No hay ejemplares disponibles para prestar.");

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

            // Procesar reservas si hay
            if (ColaReservas.Count > 0 && ColaReservas.Peek() == usuario)
            {
                ColaReservas.Dequeue();
                FechasReserva.Remove(usuario);
                Console.WriteLine($"Reserva de {usuario} procesada automáticamente.");
            }

            // Notificar si se agota el stock
            if (Stock == 0)
                StockAgotado?.Invoke(this, EventArgs.Empty);

            Console.WriteLine($"Libro '{Titulo}' prestado a {usuario}. Vence: {FechasVencimiento[usuario]:dd/MM/yyyy}");
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

            Console.WriteLine($"Libro '{Titulo}' devuelto por {usuario}. Stock actual: {Stock}/{Ejemplares}");

            // Notificar al siguiente en la cola de reservas
            if (ColaReservas.Count > 0)
            {
                string siguienteUsuario = ColaReservas.Peek();
                Console.WriteLine($"Libro disponible para {siguienteUsuario} (reserva pendiente).");
            }
        }

        // Método para renovar préstamo
        public void Renovar(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("Debe especificar el usuario que renueva el préstamo.");

            if (!UsuariosPrestamo.Contains(usuario))
                throw new InvalidOperationException($"El usuario {usuario} no tiene este libro prestado.");

            if (NumeroRenovaciones[usuario] >= MaximoRenovaciones)
                throw new InvalidOperationException($"Se alcanzó el límite máximo de {MaximoRenovaciones} renovaciones.");

            if (ColaReservas.Count > 0)
                throw new InvalidOperationException("No se puede renovar. Hay usuarios en cola de reserva.");

            // Renovar el préstamo
            FechasVencimiento[usuario] = DateTime.Now.AddDays(DiasPrestamo);
            NumeroRenovaciones[usuario]++;

            Console.WriteLine($"Préstamo renovado para {usuario}. Nueva fecha de vencimiento: {FechasVencimiento[usuario]:dd/MM/yyyy}");
            Console.WriteLine($"Renovaciones usadas: {NumeroRenovaciones[usuario]}/{MaximoRenovaciones}");
        }

        // Método para reservar un libro
        public void Reservar(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("Debe especificar el usuario que hace la reserva.");

            if (Disponible)
                throw new InvalidOperationException("El libro está disponible. No es necesario reservarlo.");

            if (UsuariosPrestamo.Contains(usuario))
                throw new InvalidOperationException("No puedes reservar un libro que ya tienes prestado.");

            if (ColaReservas.Contains(usuario))
                throw new InvalidOperationException("Ya tienes una reserva activa para este libro.");

            // Agregar a la cola de reservas
            ColaReservas.Enqueue(usuario);
            FechasReserva[usuario] = DateTime.Now;

            Console.WriteLine($"Libro '{Titulo}' reservado para {usuario}.");
            Console.WriteLine($"Posición en cola: {ColaReservas.Count}");
        }

        // Método para cancelar reserva
        public void CancelarReserva(string usuario)
        {
            if (!ColaReservas.Contains(usuario))
                throw new InvalidOperationException($"El usuario {usuario} no tiene reservas para este libro.");

            // Recrear la cola sin el usuario
            var nuevaCola = new Queue<string>();
            while (ColaReservas.Count > 0)
            {
                string usuarioEnCola = ColaReservas.Dequeue();
                if (usuarioEnCola != usuario)
                    nuevaCola.Enqueue(usuarioEnCola);
            }

            ColaReservas = nuevaCola;
            FechasReserva.Remove(usuario);

            Console.WriteLine($"Reserva de {usuario} cancelada.");
        }

        // Método para obtener información completa del libro
        public string MostrarInfo()
        {
            var info = $"ID: {IdLibro} | Título: {Titulo} | Autor: {Autor}\n";
            info += $"Stock: {Stock}/{Ejemplares} | Disponible: {(Disponible ? "Sí" : "No")}\n";

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

        // Método para obtener préstamos vencidos
        public List<string> ObtenerPrestamosVencidos()
        {
            var vencidos = new List<string>();
            var hoy = DateTime.Now.Date;

            foreach (var prestamo in FechasVencimiento)
            {
                if (prestamo.Value.Date < hoy)
                    vencidos.Add(prestamo.Key);
            }

            return vencidos;
        }

        // Método para verificar si un usuario específico tiene el libro vencido
        public bool EstaVencido(string usuario)
        {
            if (!UsuariosPrestamo.Contains(usuario))
                return false;

            return FechasVencimiento[usuario].Date < DateTime.Now.Date;
        }
    }
}