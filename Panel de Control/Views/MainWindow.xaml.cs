using System;
using System.Collections.ObjectModel; 
using System.Windows;
using System.Windows.Input;

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
        // 🔥 Lista de datos
        public ObservableCollection<Equipo> ListaEquipos { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            // Inicializar lista
            ListaEquipos = new ObservableCollection<Equipo>();
            // 👇 DATOS DE PRUEBA
            ListaEquipos.Add(new Equipo
            {
                Activo = "ACT-001",
                Nombre = "Monitor de Signos Vitales",
                Marca = "Philips",
                Modelo = "MX400",
                Serie = "SN-12345",
                Periodicidad = "3 meses",
                Estado = "Operativo",
                PeriodicidadC = "Anual"

            });
            ListaEquipos.Add(new Equipo
            {
                Activo = "ACT-002",
                Nombre = "Ventilador Mecánico",
                Marca = "Dräger",
                Modelo = "V300",
                Serie = "SN-67890",
                Periodicidad = "1 mes",
                Estado = "Mantenimiento",
                PeriodicidadC = "Anual"

            });

            // 🔗 Conectar con el DataGrid
            tablaEquipos.ItemsSource = ListaEquipos;
        }
        
        
        //BOTONES
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}