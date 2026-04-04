using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace Panel_de_Control
    {
    public class Equipo//Clase para usar metodos get y set en los atributos del datagrid (Tabla de equipos)
    {
        public int Id { get; set; }
        public string CodigoActivo { get; set; }
        public string Nombre { get; set; }
        public string NumeroSerie { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public string PeriodicidadMantenimiento { get; set; }
        public DateTime? UltimoMantenimiento { get; set; }
        public string Calibracion { get; set; }
        public DateTime? UltimaCalibracion{ get; set; }
        public string Ubicacion { get; set; }
        public string Responsable { get; set; }
        public DateTime? FechaAdquisicion { get; set; }
        public string Estado { get; set; }
    }
}
