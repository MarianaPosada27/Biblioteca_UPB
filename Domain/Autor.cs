

namespace Biblioteca_UPB.Domain
{
    public class Autor : Persona
    {
        public string generoLiterario { get; set; } 

        // Constructor
        public Autor(string documento, string nombre, string generoLiterario) : base(documento, nombre)
        {
            if (string.IsNullOrWhiteSpace(generoLiterario))
                throw new ArgumentException("El género literario no puede estar vacío.");

            this.generoLiterario = generoLiterario.Trim();
        }
    }
}