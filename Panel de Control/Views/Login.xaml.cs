using Panel_de_Control.View;
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
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtCorreo.Text == "admin@hospital.com" && txtPass.Password == "1234")
            {
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Correo o contraseña incorrectos");
            }
        }

        private void BtnRegistro_Click(object sender, RoutedEventArgs e)
        {
            SignUp registro = new SignUp();
            registro.ShowDialog();
        }
    }
}
