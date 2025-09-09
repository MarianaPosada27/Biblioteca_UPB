using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_UPB.Domain
{
    public class Bibliotecario : Persona
    {
        public string CodigoEmpleado { get; private set; }

        //Constructor 
        public Bibliotecario(string documento, string nombre, string codigoEmpleado) : base(documento, nombre)
        {
            //Inicializa el codigo del empleado, si es nulo o vacio se asigna "General"
            CodigoEmpleado = string.IsNullOrWhiteSpace(codigoEmpleado) ? " General " : codigoEmpleado.Trim();
        }
    }
}