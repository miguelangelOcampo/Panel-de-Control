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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Panel_de_Control.Views
{
    public partial class Indicadores : Window
    {
        public Indicadores()
        {
            InitializeComponent();
            CargarIndicadores();
        }

        //METODO PARA CARGAR LOS INDICADORES EN LA INTERFAZ USA METODOS DE IndicadoresDAO PARA OBTENER LOS DATOS DE LA BASE DE DATOS
        private void CargarIndicadores()
        {
            var dao = new IndicadoresDAO();

            // 1. Operatividad
            txtOperatividad.Text = dao.ObtenerTasaOperatividad().ToString("F2") + "%";

            // 2. Tiempo
            txtTiempo.Text = dao.ObtenerTiempoPromedioResolucion().ToString("F2") + " horas";

            // 3. Equipos pendientes
            var pendientes = dao.ObtenerEquiposPendientes();
            txtAlertas.Text = pendientes.Count.ToString();

            panelEquiposPendientes.Children.Clear();

            foreach (var equipo in pendientes)
            {
                panelEquiposPendientes.Children.Add(new TextBlock
                {
                    Text = equipo,
                    Margin = new Thickness(0, 5, 0, 0)
                });
            }

            // 4. Costos
            var costos = dao.ObtenerCostosMensuales();
            txtCostos.Text = "$" + costos.total.ToString("N0");
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