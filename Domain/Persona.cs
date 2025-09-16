

namespace Biblioteca_UPB.Domain
{
    public abstract class Persona
    {
        public string Documento { get; set; }
        public string Nombre { get; private set; }

        // Constructor protegido para que solo las clases derivadas puedan instanciar
        protected Persona(string documento, string nombre)
        {
            if (string.IsNullOrWhiteSpace(documento))
                throw new ArgumentException("El Identifidcador no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.");
            Documento = documento;
            Nombre = nombre.Trim();
        }
        //Metodo sobrescrito para mostrar la informacion de la persona
        public override string ToString()=> $"{Nombre} (Documento: {Documento})";
    }
}  
