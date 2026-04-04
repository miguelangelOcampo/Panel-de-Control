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

        private void Button_Indicadores(object sender, RoutedEventArgs e)
        {
            Indicadores indicadores = new Indicadores();
            Application.Current.MainWindow = indicadores;
            indicadores.Show();
            this.Close();

        }

        //EVENTO PARA CARGAR LOS EQUIPOS EN EL DATA GRID CUANDO SE CARGUE LA VENTANA
        private void CargarEquipos()
        {
            var dao = new EquipoDAO();
            tablaEquipos.ItemsSource = dao.ObtenerEquipos();//Carga los equipos obtenidos de la base de datos a la propiedad ItemsSource del DataGrid, lo que hace que se muestren en la tabla
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