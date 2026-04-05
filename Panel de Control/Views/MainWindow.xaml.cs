using Panel_de_Control.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MySql.Data.MySqlClient;//Agrega la referencia a MySql.Data para poder utilizar las clases relacionadas con MySQL
using Panel_de_Control.Data;//Agrega la referencia Data para poder utilizar las clases relacionadas con la conexion a la BD y el acceso a los datos de los equipos

namespace Panel_de_Control.Views
{
    public partial class MainWindow : Window
    {
        private List<Equipo> listaEquipos = new List<Equipo>();//lista para almacenar los datos de los equipos obtenidos de la base de datos



        //EVENTO DE CONTROL PARA QUITAR SELECCION AL HACER CLICK
        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Si el click NO fue dentro del DataGrid
            if (!tablaEquipos.IsMouseOver)
            {
                tablaEquipos.UnselectAll();
            }
        }
        //Lista de datos
        public ObservableCollection<Equipo> ListaEquipos { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            CargarEquipos();
            CargarIndicadores();
        }

        //BOTONES
        private void Button_CerrarSesion(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            Application.Current.MainWindow = login;
            login.Show();
            this.Close();
        }

        private void Button_AgregarEquipo(object sender, RoutedEventArgs e)
        {
            var ventana = new AgregarEquipo();

            if (ventana.ShowDialog() == true)
            {
                CargarEquipos();
            }
        }

        private void Button_Indicadores(object sender, RoutedEventArgs e)//Viaja a la ventana de indicadores y cierra la ventana principal
        {
            Indicadores indicadores = new Indicadores();
            Application.Current.MainWindow = indicadores;
            indicadores.Show();
            this.Close();

        }

        private void Button_Refrescar(object sender, RoutedEventArgs e)
        {
            CargarEquipos();      //recarga la tabla principal
            CargarIndicadores(); //recarga los indicadores de la ventana principal

        }
        //BOTON PARA VER EL DETALLE DE UN EQUIPO EN LA BASE DE DATOS (Viaja al a ventana DetalleEquipo)
        private void VerDetalle_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            var equipo = (button.DataContext as Equipo); //IMPORTANTE

            if (equipo != null)
            {
                var ventana = new DetalleEquipo(equipo);
                ventana.Show();
                this.Close();
            }
        }
        //BOTON PARA ELIMINAR UN EQUIPO DE LA BASE DE DATOS
        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var equipo = button.DataContext as Equipo; // tu modelo

            if (equipo == null) return;

            var resultado = MessageBox.Show(
                $"¿Seguro que deseas eliminar el equipo {equipo.Nombre}?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes)
            {
                var dao = new EquipoDAO();
                dao.EliminarEquipo(equipo.Id); // debes tener este método

                CargarEquipos(); // recargar tabla
            }
        }
        //BARRA DE BUSQUEDA PARA FILTRAR LOS EQUIPOS EN LA TABLA
        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (listaEquipos == null) return;

            string filtro = txtBuscar.Text.ToLower().Trim();

            if (string.IsNullOrEmpty(filtro))
            {
                tablaEquipos.ItemsSource = listaEquipos;
                return;
            }

            var filtrados = listaEquipos.Where(equipo =>

                equipo.Id.ToString().Contains(filtro) ||

                (equipo.CodigoActivo?.ToLower().Contains(filtro) ?? false) ||
                (equipo.NumeroSerie?.ToLower().Contains(filtro) ?? false) ||
                (equipo.Nombre?.ToLower().Contains(filtro) ?? false) ||
                (equipo.Marca?.ToLower().Contains(filtro) ?? false) ||
                (equipo.Modelo?.ToLower().Contains(filtro) ?? false) ||
                (equipo.Ubicacion?.ToLower().Contains(filtro) ?? false) ||
                (equipo.Responsable?.ToLower().Contains(filtro) ?? false) ||
                (equipo.Estado?.ToLower().Contains(filtro) ?? false) ||
                (equipo.PeriodicidadMantenimiento?.ToLower().Contains(filtro) ?? false) ||
                (equipo.Calibracion?.ToLower().Contains(filtro) ?? false) 

            ).ToList();

            tablaEquipos.ItemsSource = filtrados;
        }

        //CONTEO Y CARGA DE INDICADORES DE LA VENTANA PRINCIPAL
        private void CargarIndicadores()//Metodo para cargar los indicadores en los paneles
        {
            txtTotalEquipos.Text = listaEquipos.Count.ToString();

            txtOperativos.Text = listaEquipos//hace que la propiedad txtOperativos muestre el conteo de los equipos que tienen el estado "operativo" en la base de datos, usando el metodo Count para contar los elementos que cumplen la condicion y luego convirtiendo el resultado a string para mostrarlo en el TextBlock
                .Count(e => e.Estado.ToLower().Trim() == "operativo")
                .ToString();

            txtMantenimiento.Text = listaEquipos//hace que la propiedad txtMantenimiento muestre el conteo de los equipos que tienen el estado "mantenimiento" en la base de datos, usando el metodo Count para contar los elementos que cumplen la condicion y luego convirtiendo el resultado a string para mostrarlo en el TextBlock
                .Count(e => e.Estado.ToLower().Trim() == "mantenimiento")//cuenta los elementos de la lista de equipos que cumplen la condicion de tener el estado "mantenimiento" se compara siempre en minuscula
                .ToString();

            var dao = new HistorialDAO();

            txtPendientes.Text = dao.ContarPeticionesPendientes().ToString();
        }

        //EVENTO PARA CARGAR LOS EQUIPOS EN EL DATA GRID CUANDO SE CARGUE LA VENTANA
        private void CargarEquipos()
        {
            var dao = new EquipoDAO();
            listaEquipos = dao.ObtenerEquipos();//Carga los equipos obtenidos de la base de datos a la propiedad ItemsSource del DataGrid, lo que hace que se muestren en la tabla
            tablaEquipos.ItemsSource = listaEquipos;
        }

        //EVENTO PARA BOTON DE GESTIONAR COLUMNAS

        //1. Se crea una lista de tipo ColumnaConfig para rellenarla con el nombre y el estado de cada columna del DataGrid
        private void Button_GestionarColumnas(object sender, RoutedEventArgs e)
        {
            var columnas = new List<ColumnaConfig>();//crea una lista de tipo clase ColumnaConfig que se creo para usar metodos set y get de los atributos del nombre y activacion (CheckBox) de cada columna

            //Usa un foreach para recorrer TODAS las columnas de la tabla (DataGrid)y agregarla a la lista columnas
            foreach (var col in tablaEquipos.Columns)
            {
                columnas.Add(new ColumnaConfig //Agrega a la lista columnas un objeto nuevo de la clase ColumnaConfig usando el Nombre y el estado de Activacion
                {
                    Nombre = col.Header.ToString(),//el nombre de la columna se obtiene del Header del DataGrid
                    Activa = col.Visibility == Visibility.Visible//el estado de activacion se determina por la propiedad Visibility (si es visible esta activa, si no, no lo esta)
                });
            }

        //2. Se crea una nueva Ventana del tipo GestionarColumnas y se le pasa la lista de columnas para mostrarla en esa ventana

            var ventana = new GestionarColumnas(columnas);//crea una nueva ventana del tipo GestionarColumnas (Fue creada para mostrar la lista de columnas activas)

            if (ventana.ShowDialog() == true)//muestra la ventana de gestionar columnas
            {
                // aplicar cambios automáticamente
                foreach (var config in columnas)//recorre cada elemento de la lista recien creada columnas (lista que contiene El nombre y estado de activacion de cada columna del DataGrid)
                {
                    var columna = tablaEquipos.Columns.FirstOrDefault(c => c.Header.ToString() == config.Nombre);
                    //Crea una variable columna que sirve para buscar uno por uno cada columna del DataGrid comparando el Header con el nombre configurado en ella
                    //Es decir que guarda en la varaiable columna el componente del DataGrid con los mismo datos para poder manipular su visibilidad

                    if (columna != null)//si encuentra la columna, aplica la configuración de visibilidad
                    {
                        columna.Visibility = config.Activa//Si la columna esta activa se vuelve Visible de lo contrario se colapsa (se oculta)
                            ? Visibility.Visible
                            : Visibility.Collapsed;
                    }
                }
            }


        }
    }
}