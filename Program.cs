using Biblioteca_UPB.Domain;
using Biblioteca_UPB.Services;


namespace Biblioteca_UPB
{
    internal class Program
    {
        private static void Main()
        {

            Console.WriteLine("=== Biblioteca U.P.B. — Sistema de Préstamos ===\n");
            var autor1 = new Autor("A001", "Pedro Perez Márquez", "Ficción");

            var listaPrestamos = new List<Prestamo>();

            var listaLibros = new List<Libro>
                {
                    new Libro("L001", "Fundamentos de Programación en C#", autor1, 3),
                    new Libro("L002", "Estructuras de Datos y Algoritmos", autor1, 2),
                    new Libro("L003", "Cálculo Diferencial e Integral", autor1, 5),
                    new Libro("L004", "Circuitos Eléctricos", autor1, 4),
                    new Libro("L005", "Mecánica Vectorial para Ingenieros", autor1, 2),
                    new Libro("L006", "Ingeniería de Software: Un Enfoque Práctico", autor1, 1)
                };

            var listaEstudiantes = new List<Estudiante>
                    {
                        new Estudiante("1000", "Brandom Perez", "Ingenieria de Sistemas", "branchi@gmail.es"),
                        new Estudiante("2000", "Miriam Lopez", "Administración de Empresas", "lopezmi@hotmailñ.com"),
                        new Estudiante("3000", "Francisco Mejia", "Ingenieria en Producción", "franme@gmail.com.co"),
                        new Estudiante("4000", "Maria Camila Alvarez", "Artes y Humanidades", "maposal@hotmail.com"),
                        new Estudiante("5000", "Juan Fernando Rueda","Ingenieria Industrial", "juanfer@live.es"),
                        new Estudiante("6000", "Jhonatan moreno", "Ingenieria Biomedica", "jhonainge@gmail.com")
                    };

            // 1) Datos iniciales

            var estudiante = new Estudiante("1001234567", "Mariana Álvarez");
            var servicio = new ServicioPrestamos();


            // 2) Menú interactivo
            while (true)
            {
                Console.WriteLine("\n=== MENÚ PRINCIPAL ===");

                Console.WriteLine("1. Prestar libro");
                Console.WriteLine("2. Devolver libro");
                Console.WriteLine("3. Listar Historial de Préstamos");
                Console.WriteLine("4. Listar libros disponibles");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opción: ");
                var opcion = Console.ReadLine();

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            Console.WriteLine("\n== PRESTAR LIBRO ==");

                            Console.Write("Ingrese documento del estudiante: ");
                            var docEstudiante = Console.ReadLine()??"";
                            estudiante = listaEstudiantes.Find(e => e.Documento == docEstudiante);
                            if (estudiante == null)
                            {
                                Console.WriteLine("Estudiante no encontrado.");
                                break;
                            }   

                            Console.Write("Ingrese ID del libro: ");
                            var idLibro = Console.ReadLine().ToUpper()?? "";
                            bool sw = false;
                            for (int i = 0; i < listaLibros.Count; i++)
                            {
                                if (listaLibros[i].IdLibro == idLibro)
                                {
                                    Prestamo p = servicio.RealizarPrestamo(listaLibros[i], estudiante);
                                  //  listaLibros[i].Stock-;
                                    listaPrestamos.Add(p);
                                    Console.WriteLine(p.MostrarInfo());
                                    sw = true;
                                    break;
                                }
                            }
                            if (!sw)
                                Console.WriteLine("El libro no esta disponible o no existe.");
                            break;

                        case "2":
                            Console.WriteLine("\n== DEVOLVER LIBRO ==");
                            Console.Write("Ingrese ID del préstamo (ej: P001): ");
                            var idPrestamoDev = Console.ReadLine().ToUpper() ?? "";
                            try
                            {
                                
                                // Buscar el préstamo activo con ese ID
                                var prestamoDev = servicio.ObtenerPrestamosActivos()
                                 .FirstOrDefault(p => p.IdPrestamo == idPrestamoDev);

                                if (prestamoDev == null)
                                {
                                    Console.WriteLine("Préstamo no encontrado");
                                    break;
                                }

                                // Determinar el libro correspondiente
                                Libro? libroDev = null;
                                
                                if (prestamoDev.Libro.IdLibro == listaLibros.FirstOrDefault(l => l.IdLibro == prestamoDev.Libro.IdLibro).IdLibro)
                                    
                                libroDev = listaLibros.FirstOrDefault(l => l.IdLibro == prestamoDev.Libro.IdLibro);
                                //libroDev.Stock++;
                                
                                // Procesar devolución
                                servicio.DevolverPrestamo(idPrestamoDev, libroDev);
                                Console.WriteLine(" Libro devuelto con éxito.");

                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[ERROR] {ex.Message}");
                            }
                            break;

                        case "3":
                            Console.WriteLine("\n== HISTORIAL DE PRÉSTAMOS ==");
                            foreach (var p in servicio.ObtenerPrestamosActivos())
                                Console.WriteLine($" - {p.MostrarInfo()}");
                            break;

                        case "4":
                            Console.WriteLine("\n==LIBROS DISPONIBLES ==");
                            foreach (Libro l in listaLibros)
                            {
                                Console.WriteLine($"{l.IdLibro} - {l.Titulo} ({l.Stock} disponibles) - Autor: {l.Autor.Nombre}");
                            }
                            break;

                        case "0":
                            Console.WriteLine(" Saliendo del sistema..." + "Gracias por visitar la Biblioteca U.P.B. ¡Hasta pronto!");
                            return;

                        default:
                            Console.WriteLine(" Opción no válida, intente de nuevo.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] {ex.Message}");

                }
            }
        }
    }

}

