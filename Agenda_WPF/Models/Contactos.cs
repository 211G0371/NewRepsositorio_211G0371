using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda_WPF.Models
{
    public class Contactos
    {
        // el ? es que acepta null
        public string? Nombre { get; set; } // = " "; otra forma para poner el tipo de dato null
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public DateTime FechaNacimiento { get; set; } = DateTime.Now.Date;
    }
}
