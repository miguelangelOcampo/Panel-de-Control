using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel_de_Control.Models
{
    internal class PeticionEquipo
    {
        public int Id { get; set; }

        public int EquipoId { get; set; }
        public int UsuarioId { get; set; }

        public string Tipo { get; set; } = "";
        public string Estado { get; set; } = "";

        public string EstadoFormateado//Formatea el Estado para que se visualice en el datagrid de forma correcta
        {
            get
            {
                if (string.IsNullOrEmpty(Estado)) return "N/A";
                return char.ToUpper(Estado[0]) + Estado.Substring(1);
            }
        }

        public DateTime Fecha { get; set; }

        public DateTime? FechaResolucion { get; set; } //permite manejar fechas nulas

        public string Tecnico { get; set; } = "";

        public string Descripcion { get; set; } = "";

        public decimal CostosGenerados { get; set; }

    }
}
