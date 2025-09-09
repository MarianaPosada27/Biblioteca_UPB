using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_UPB.Common
{
        public class Result
        {
            // Propiedades
            public bool Ok { get; } //Solo lectura, indica si la operación fue exitosa    
        public string Message { get; } //Solo lectura, mensaje asociado al resultado    

        // Constructor privado pqara forzar el uso de los métodos Success y Fail
        private Result(bool ok, string message)
            {
                Ok = ok;
                Message = message;
            }

            // Método Success
            public static Result Success(string msg = "OK") => new(true, msg);

            // Método Fail
            public static Result Fail(string msg) => new(false, msg);
        }
}
