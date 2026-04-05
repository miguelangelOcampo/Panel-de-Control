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
                               // Variable para la fila de petición seleccionada
       
        private PeticionEquipo _peticionSeleccionada;//Variable para almacenar la peticion seleccionada ( Almacena su Id)


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
            cmbEstado.Text = _equipo.Estado;
        }

        private void CargarDatosEquipo()//Método para cargar los datos del equipo en los textblocks de la ventana
        {
            txtNombre.Text = _equipo.Nombre ?? "N/A";
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

        //EVENTO PARA ACTUALIZAR EL ESTADO DEL EQUIPO EN DETALLE (COMBOBOX)
        private void cmbEstado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbEstado.SelectedItem == null || _equipo == null)
                return;

            string nuevoEstado = (cmbEstado.SelectedItem as ComboBoxItem).Content.ToString();

            // Evitar actualización innecesaria
            if (_equipo.Estado == nuevoEstado)
                return;

            _equipo.Estado = nuevoEstado;

            var dao = new EquipoDAO();
            dao.ActualizarEstado(_equipo.Id, nuevoEstado);
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
            Indicadores indicadores = new Indicadores();
            Application.Current.MainWindow = indicadores;
            indicadores.Show();
            this.Close();
        }

        private void Button_Refrescar(object sender, RoutedEventArgs e)
        {
            CargarDatosEquipo();   // datos del equipo
            CargarPeticiones();   //recarga la tabla de peticiones del equipo
        }

        private void Button_EditarEquipo(object sender, RoutedEventArgs e)
        {
            var ventana = new AgregarEquipo(_equipo);//Reutilizamos la ventana de agregar equipo pero con el constructor que recibe un equipo para editarlo
            ventana.EquipoGuardado += (equipoActualizado) =>//Suscribimos al evento para recibir el equipo actualizado después de guardar los cambios
            {
                _equipo = equipoActualizado; // actualizar el objeto local
                CargarDatosEquipo();          // refrescar UI
                CargarPeticiones();           // si quieres refrescar tabla de peticiones también
            };

            ventana.ShowDialog();
        }

        //BOTON PARA REPOTAR UNA NUEVA PETICION PARA EL EQUIPO
        private void Button_ReportarPeticion(object sender, RoutedEventArgs e)
        {
            // Crear instancia de la ventana y pasarle el Id del equipo si quieres asociar la petición
            var ventana = new AgregarPeticion(_equipo.Id);

            // Mostrar la ventana como diálogo modal (no permite interactuar con la ventana principal hasta cerrarla)
            ventana.ShowDialog();

            // Opcional: después de cerrar la ventana, puedes recargar el DataGrid si se agregó una nueva petición
            CargarPeticiones();
        }


        //EVENTO PARA ENVIAR LOS DATOS DE LA PETICION SELECCIONADA
        private void Button_EnviarPeticion(object sender, RoutedEventArgs e)
        {
            if (_peticionSeleccionada == null)
            {
                MessageBox.Show("Por favor, selecciona una petición para actualizar.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Actualizar los datos de la petición seleccionada
            _peticionSeleccionada.Estado = (cmbEstadoPeticion.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Pendiente";
            _peticionSeleccionada.CostosGenerados = decimal.TryParse(txtCostoGenerado.Text, out var costo) ? costo : 0;

            // Guardar cambios en la base de datos
            var dao = new HistorialDAO();
            dao.ActualizarPeticion(_peticionSeleccionada); // Este método lo creamos en HistorialDAO

            // Refrescar el DataGrid
            CargarPeticiones();

            // Limpiar selección y campos
            _peticionSeleccionada = null;
            txtIdPeticion.Text = "Petición seleccionada: N/A";
            cmbEstadoPeticion.SelectedIndex = 0;
            txtCostoGenerado.Clear();
        }

        //EVENTO SIRVE PARA MOSTRAR EL ID DE LA PETICION SELECCIONADA DE LA FILA DE LA TABLA DE PETICIONES DEL EQUIPO
        private void TablaDetalle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TablaDetalle.SelectedItem is PeticionEquipo peticion)
            {
                _peticionSeleccionada = peticion;

                // Mostrar el ID de la petición
                txtIdPeticion.Text = $"Petición seleccionada: {peticion.Id}";//Actualiza la caja de texto para que aparezca el ID

                // Llenar los campos para editar
                cmbEstadoPeticion.Text = peticion.EstadoFormateado;
                txtCostoGenerado.Text = peticion.CostosGenerados.ToString();
            }
            else
            {
                txtIdPeticion.Text = "Petición seleccionada: N/A";
                _peticionSeleccionada = null;
            }
        }
    }
}
