using Panel_de_Control.Models;
using System.Collections.Generic;
using System.Windows;

namespace Panel_de_Control.Views
{
    public partial class GestionarColumnas : Window
    {
        public List<ColumnaConfig> Columnas { get; set; }//Lista de tipo ColumnaConfig para almacenar el nombre y estado de cada columna del DataGrid

        //Constructor correcto (recibe columnas)
        public GestionarColumnas(List<ColumnaConfig> columnas)//Usa un constructor para poder mostrar la lista de columnas en la ventana (Viene del MainWindow)
        {
            InitializeComponent();//Inicializa la ventana
            Columnas = columnas;//Asigna la lista de columnas recibida al atributo Columnas de esta clase para poder usarla en esta ventana
            ListaColumnas.ItemsSource = Columnas;//Asigna la lista de columnas al ItemsSource del ListBox para mostrarla en la ventana
        }
        // Solo mostrar/ocultar

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}