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

        public DateTime Fecha { get; set; }

        // ESTE ES PARA MOSTRAR EN EL DATAGRID (JOIN)
        public string Tecnico { get; set; } = "";

        // OPCIONAL (si agregas en BD)
        public string Descripcion { get; set; } = "";

    }
}
