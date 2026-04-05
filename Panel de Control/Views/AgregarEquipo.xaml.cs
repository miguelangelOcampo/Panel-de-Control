using Panel_de_Control.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Panel_de_Control.Views
{
    /// <summary>
    /// Lógica de interacción para AgregarEquipo.xaml
    /// </summary>
    public partial class AgregarEquipo : Window
    {
        private bool esEdicion = false; //Variables que nos permitiran usar la ventana pero en un modo de edicion
        private Equipo equipoActual;//Variable para almacenar el equipo que se esta editando
        public event Action<Equipo> EquipoGuardado; //Evento para notificar a la ventana principal que se ha guardado un equipo, se usa para actualizar la tabla
        public AgregarEquipo()
        {
            InitializeComponent();
        }

        private void Button_Cancelar(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
        private void Button_Guardar(object sender, RoutedEventArgs e)
        {
            var equipo = new Equipo
            {
                Id = esEdicion ? equipoActual.Id : 0, // importante para actualizar
                CodigoActivo = txtCodigoActivo.Text,
                Nombre = txtNombre.Text,
                NumeroSerie = txtSerie.Text,
                Modelo = txtModelo.Text,
                Marca = txtMarca.Text,
                PeriodicidadMantenimiento = txtPeriodicidad.Text,
                UltimoMantenimiento = dpUltimoMantenimiento.SelectedDate,
                Calibracion = txtCalibracion.Text,
                UltimaCalibracion = dpUltimaCalibracion.SelectedDate,
                Ubicacion = txtUbicacion.Text,
                Responsable = txtResponsable.Text,
                FechaAdquisicion = dpFechaAdquisicion.SelectedDate,
                Estado = (cbEstado.SelectedItem as ComboBoxItem)?.Content.ToString()
            };

            var dao = new EquipoDAO();

            if (esEdicion)//Se usa la bandera para saber si se esta editando o agregando un nuevo equipo, si es edicion se llama al metodo actualizar y si no se llama al metodo insertar
                dao.ActualizarEquipo(equipo); // usa el método actualizar si es edición
            else
                dao.InsertarEquipo(equipo);   // usa el método insertar si es nuevo

            EquipoGuardado?.Invoke(equipo);//Dispara el evento para notificar a la ventana principal que se ha guardado un equipo
            DialogResult = true;
            this.Close();
        }
        //CONSTRUCTOR PARA MODO EDICION, SE USA EL MISMO XAML PERO CON LOS DATOS DEL EQUIPO SELECCIONADO PARA EDITARLOS
        public AgregarEquipo(Equipo equipo)
        {
            InitializeComponent();

            esEdicion = true;
            equipoActual = equipo;

            CargarDatos();
        }
        //METODO PARA CARGAR LOS DATOS DEL EQUIPO ACTUAL EN LOS TEXTBOX Y COMBOBOX DE LA VENTANA PARA PODER EDITARLOS
        private void CargarDatos()
        {
            txtCodigoActivo.Text = equipoActual.CodigoActivo;
            txtNombre.Text = equipoActual.Nombre;
            txtSerie.Text = equipoActual.NumeroSerie;
            txtModelo.Text = equipoActual.Modelo;
            txtMarca.Text = equipoActual.Marca;
            txtPeriodicidad.Text = equipoActual.PeriodicidadMantenimiento;
            txtCalibracion.Text = equipoActual.Calibracion;
            txtUbicacion.Text = equipoActual.Ubicacion;
            txtResponsable.Text = equipoActual.Responsable;

            dpUltimoMantenimiento.SelectedDate = equipoActual.UltimoMantenimiento;
            dpUltimaCalibracion.SelectedDate = equipoActual.UltimaCalibracion;
            dpFechaAdquisicion.SelectedDate = equipoActual.FechaAdquisicion;

            cbEstado.Text = equipoActual.Estado;
        }
    }
}
