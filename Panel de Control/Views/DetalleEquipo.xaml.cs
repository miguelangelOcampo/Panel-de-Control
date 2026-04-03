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
        public DetalleEquipo()
        {

            InitializeComponent();
       

        var lista = new List<PeticionEquipo>
            {
                new PeticionEquipo
                {
                    Tipo = "Observación",
                    Descripcion = "Pantalla con leve parpadeo",
                    Fecha = "9 mar 2026",
                    Tecnico = "Téc. Sánchez",
                    Estado = "Pendiente"
                },
                new PeticionEquipo
                {
                    Tipo = "Mantenimiento",
                    Descripcion = "Calibración completada",
                    Fecha = "14 feb 2026",
                    Tecnico = "Téc. Sánchez",
                    Estado = "Resuelto"
                }
            };

            TablaDetalle.ItemsSource = lista;
        }
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
