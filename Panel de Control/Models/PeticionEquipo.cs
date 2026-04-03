using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel_de_Control.Models
{
    internal class PeticionEquipo
    {
        public string Tipo { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public string Fecha { get; set; } = "";
        public string Tecnico { get; set; } = "";
        public string Estado { get; set; } = "";
    }
}
