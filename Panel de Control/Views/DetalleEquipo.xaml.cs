using Panel_de_Control.Data;
using Panel_de_Control.Models;
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
    /// Lógica de interacción para DetalleEquipo.xaml
    /// </summary>
    public partial class DetalleEquipo : Window

    {

        private Equipo _equipo;//Variable privada para almacenar el equipo seleccionado y mostrar sus detalles en la ventana


        //EVENTO DE CONTROL PARA QUITAR SELECCION AL HACER CLICK
        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject source = e.OriginalSource as DependencyObject;

            // Recorre el árbol visual
            while (source != null)
            {
                // Si el click fue dentro de una FILA del DataGrid → NO deseleccionar
                if (source is DataGridRow)
                    return;

                source = VisualTreeHelper.GetParent(source);
            }

            // Si el click fue fuera del DataGrid → deseleccionar
            TablaDetalle.UnselectAll();
        }

        //CONSTRUCTOR VACIO
        public DetalleEquipo()
        {
            InitializeComponent();
        }


        //CONSTRUCTORES Y METODOS CON INFORMACION DEL EQUIPO SELECCIONADO
        public DetalleEquipo(Equipo equipo)//Creamos un constructor para pasar los datos de los equipos a la ventana
        {
            InitializeComponent();
            _equipo = equipo;

            CargarDatosEquipo();
            CargarPeticiones();
        }
        private void CargarDatosEquipo()//Método para cargar los datos del equipo en los textblocks de la ventana
        {
            txtNombre.Text = _equipo.Nombre ?? "N/A";
            txtEstado.Text = _equipo.Estado ?? "N/A";
            txtActivo.Text = _equipo.CodigoActivo ?? "N/A";
            txtSerie.Text = _equipo.NumeroSerie ?? "N/A";
            txtPeriodicidad.Text = _equipo.PeriodicidadMantenimiento ?? "N/A";
            txtMarcaModelo.Text = $"{_equipo.Marca ?? ""} - {_equipo.Modelo ?? ""}";
            txtUbicacion.Text = _equipo.Ubicacion ?? "N/A";
            txtFechaAdquisicion.Text = _equipo.FechaAdquisicion?.ToString("dd/MM/yyyy") ?? "N/A";
            txtResponsable.Text = _equipo.Responsable ?? "N/A";
        }
        private void CargarPeticiones()//Método para cargar las peticiones relacionadas con el equipo en la tabla de detalles
        {
            var dao = new HistorialDAO();
            var lista = dao.ObtenerPorEquipo(_equipo.Id);

            TablaDetalle.ItemsSource = lista;
        }


        //BOTONES
        private void Button_CerrarSesion(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            Application.Current.MainWindow = login;
            login.Show();
            this.Close();
        }

        private void Button_PanelControl(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            Application.Current.MainWindow = main;
            main.Show();
            this.Close();
        }
        private void Button_Indicadores(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            Application.Current.MainWindow = main;
            main.Show();
            this.Close();
        }
    }
    
}
