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