using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_UPB.Domain
{
    public class Autor : Persona
    {
        public string? generoLiterario { get; set; } // Nota el signo de interrogación

        // Constructor
        public Autor(string documento, string nombre, string generoLiterario) : base(documento, nombre)
        {
            if (string.IsNullOrWhiteSpace(generoLiterario))
                throw new ArgumentException("El género literario no puede estar vacío.");

            this.generoLiterario = generoLiterario.Trim();
        }
    }
}