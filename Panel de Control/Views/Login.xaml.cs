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
using MySql.Data.MySqlClient;
using Panel_de_Control.Data;

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
            string correo = txtCorreo.Text;//Extrae el texto del TextBox txtCorreo y lo asigna a la variable correo
            string pass = txtPass.Password;//Extrae el texto del PasswordBox txtPass y lo asigna a la variable pass

            Conexion con = new Conexion();//Crea una instancia de la clase Conexion para establecer la conexión con la base de datos

            using (MySqlConnection conexion = con.ObtenerConexion())//Utiliza la clase MySqlConnection (Clase creada por nosotros) para obtener la conexión a la BD y se envuelve en un bloque using para asegurar que la conexión se cierre después de su uso
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT rol FROM usuarios WHERE correo = @correo AND contraseña = @contraseña";//Creacion como un String de la consulta SQL para verificar las credenciales del usuario

                    MySqlCommand cmd = new MySqlCommand(query, conexion);//Creación del comando SQL con la consulta y la conexión

                    cmd.Parameters.AddWithValue("@correo", correo);//Le dice que donde se vea @correo en la consulta use el valor de la variable correo
                    cmd.Parameters.AddWithValue("@contraseña", pass);//Le dice que donde se vea @contraseña en la consulta use el valor de la variable pass
                    var Resultado = cmd.ExecuteScalar();//Devuelve el valor de la primera fila y de la columna que se ponga en el SELECT, en este caso el rol.

                    if (Resultado != null)//Si el resultado no es nulo, significa que se encontró un usuario con las credenciales proporcionadas
                    {
                       string rol = Resultado.ToString();

                        MessageBox.Show("Inicio de sesión exitoso. Rol: " + rol, "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                        //Abrir MainWindow
                        MainWindow main = new MainWindow();
                        main.Show();
                        this.Close();
                    }
                    else//Si el resultado es nulo, significa que no se encontró un usuario con las credenciales proporcionadas
                    {
                        MessageBox.Show("Correo o contraseña incorrectos.", "Error de inicio de sesión", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)//Manejo de excepciones para errores de conexión o consulta
                {
                    MessageBox.Show("Error al conectar a la base de datos: " + ex.Message, "Error de conexión", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnRegistro_Click(object sender, RoutedEventArgs e)
        {
            SignUp registro = new SignUp();
            registro.ShowDialog();
        }
    }
}
