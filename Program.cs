using System;
using System.Collections.Generic;
using Biblioteca_UPB.Common;
using Biblioteca_UPB.Domain;
using Biblioteca_UPB.Services;
using System.Globalization;

namespace Biblioteca_UPB
{
        internal class Program
        {
            private static void Main()
            {
              
                Console.WriteLine("=== Biblioteca U.P.B. — Sistema de Préstamos ===\n");


                // 1) Datos iniciales
                var estudiante = new Estudiante("1001234567", "Mariana Álvarez");
                var autor1 = new Autor("A001", "Pedro Perez Márquez", "Ficción");
                var libro1 = new Libro("L001", "Cien Años de Soledad", autor1, 2);
                var libro2 = new Libro("L002", "El Principito", autor1, 1);
                var servicio = new ServicioPrestamos();

                // Suscribir a eventos
                servicio.PrestamoRealizado += (s, e) =>
                    Console.WriteLine($"[EVENTO] Préstamo registrado para {e.Estudiante?.Nombre} - {e.Libro?.Titulo}");
                servicio.PrestamoDevuelto += (s, e) =>
                    Console.WriteLine($"[EVENTO] Libro devuelto: {e.Libro?.Titulo}");
                servicio.PrestamoRenovado += (s, e) =>
                    Console.WriteLine($"[EVENTO] Préstamo renovado: {e.Prestamo.IdPrestamo}");

                // 2) Menú interactivo
                while (true)
                {
                    Console.WriteLine("\n=== MENÚ PRINCIPAL ===");
                    Console.WriteLine("1. Prestar libro");
                    Console.WriteLine("2. Devolver libro");
                    Console.WriteLine("3. Renovar préstamo");
                    Console.WriteLine("4. Listar préstamos activos");
                    Console.WriteLine("5. Listar préstamos vencidos");
                    Console.WriteLine("6. Listar historial de préstamos");
                    Console.WriteLine("0. Salir");
                    Console.Write("Seleccione una opción: ");
                    var opcion = Console.ReadLine();

                    try
                    {
                        switch (opcion)
                        {
                            case "1":
                                Console.WriteLine("\n== PRESTAR LIBRO ==");
                                Console.Write("Ingrese ID del libro (L001/L002): ");
                                var idLibro = Console.ReadLine() ?? "";
                                var libro = idLibro == "L001" ? libro1 : libro2;
                                var prestamo = servicio.RealizarPrestamo(libro, estudiante);
                                Console.WriteLine(prestamo.MostrarInfo());
                                break;
                            case "2":
                                Console.WriteLine("\n== DEVOLVER LIBRO ==");
                                Console.Write("Ingrese ID del préstamo (ej: P001): ");
                                var idPrestamoDev = Console.ReadLine() ?? "";
                                try
                                {
                                // Buscar el préstamo activo con ese ID
                                var prestamoDev = servicio.ObtenerPrestamosActivos()
                                                          .FirstOrDefault(p => p.IdPrestamo == idPrestamoDev);

                                if (prestamoDev == null)
                                {
                                    Console.WriteLine("❌ Préstamo no encontrado o ya fue devuelto.");
                                    break;
                                }

                                // Determinar el libro correspondiente
                                    Libro? libroDev = null;
                                if (prestamoDev.IdLibro == libro1.IdLibro)
                                    libroDev = libro1;
                                else if (prestamoDev.IdLibro == libro2.IdLibro)
                                    libroDev = libro2;

                                if (libroDev == null)
                                {
                                    Console.WriteLine("❌ Libro no encontrado en el catálogo.");
                                    break;
                                }

                                // Procesar devolución
                                servicio.DevolverPrestamo(idPrestamoDev, libroDev);
                                Console.WriteLine("✅ Libro devuelto con éxito.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[ERROR] {ex.Message}");
                            }
                            break;

                        case "3":
                                Console.WriteLine("\n== RENOVAR PRÉSTAMO ==");
                                Console.Write("Ingrese ID del préstamo: ");
                                var idPrestamoRen = Console.ReadLine() ?? "";
                                servicio.RenovarPrestamo(idPrestamoRen);
                                break;

                            case "4":
                                Console.WriteLine("\n== PRÉSTAMOS ACTIVOS ==");
                                foreach (var p in servicio.ObtenerPrestamosActivos())
                                    Console.WriteLine($" - {p.MostrarInfo()}");
                                break;

                            case "5":
                                Console.WriteLine("\n== PRÉSTAMOS VENCIDOS ==");
                                foreach (var p in servicio.ObtenerPrestamosVencidos())
                                    Console.WriteLine($" - {p.MostrarInfo()}");
                                break;

                            case "6":
                                Console.WriteLine("\n== HISTORIAL COMPLETO ==");
                                foreach (var p in servicio.ObtenerHistorialPrestamos())
                                    Console.WriteLine($" - {p.MostrarInfo()}");
                                break;

                            case "0":
                                Console.WriteLine("👋 Saliendo del sistema...");
                                return;

                            default:
                                Console.WriteLine("⚠ Opción no válida, intente de nuevo.");
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

