using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Biblioteca_UPB.Domain
{
    public class Estudiante : Persona
    {
        public string Carrera { get; private set; }
        public string email { get; private set; }

        //Constructor 

        public Estudiante(string documento, string nombre, string carrera, string email) : base(documento, nombre)
        {
            if (string.IsNullOrWhiteSpace(carrera))
                throw new ArgumentException("La carrera no puede estar vacía.");
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("El email no es válido.");
            Carrera = carrera.Trim();
            this.email = email.Trim().ToLower();
        }

        public Estudiante(string documento, string nombre) : base(documento, nombre)
        {
        }

    }
}
