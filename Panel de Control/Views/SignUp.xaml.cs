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
using System.Windows.Shapes;
using MySql.Data.MySqlClient;//Agrega la referencia a MySql.Data para poder utilizar las clases relacionadas con MySQL

namespace Panel_de_Control.Views
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        public SignUp()
        {
            InitializeComponent();
        }
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            Application.Current.MainWindow = login;
            login.Show();
            this.Close();

        }
        private void BtnCrear_Click(object sender, RoutedEventArgs e)
        {
            string correo = txtCorreo.Text;//Extrae el texto del TextBox txtCorreo y lo asigna a la variable correo
            string pass = txtPass.Password;//Extrae el texto del PasswordBox txtPass y lo asigna a la variable pass
            string nombre = txtNombre.Text;//Extrae el texto del TextBox txtNombre y lo asigna a la variable nombre

            Conexion con = new Conexion();

            using (MySqlConnection conexion = con.ObtenerConexion())
            {

                try
                {
                    conexion.Open();
                    string query = "INSERT INTO usuarios (correo, contraseña, nombre) VALUES (@correo, @contraseña, @nombre)";//Define la consulta SQL para insertar un nuevo usuario en la tabla "usuarios
                    MySqlCommand cmd = new MySqlCommand(query, conexion);//Creación del comando SQL con la consulta y la conexión

                    cmd.Parameters.AddWithValue("@correo", correo);//Agrega al valor de la consulta @correo el valor de la variable correo
                    cmd.Parameters.AddWithValue("@contraseña", pass);//Agrega al valor de la consulta @contraseña el valor de la variable pass
                    cmd.Parameters.AddWithValue("@nombre", nombre);//Agrega al valor de la consulta @nombre el valor de la variable nombre

                    var Resultado = cmd.ExecuteNonQuery();//Devuelve el numero de filas afectadas por la consulta SQL, lo que indica si la insercion fue exitosa
                    if (Resultado > 0)
                    {

                        String cantidad = Resultado.ToString();
                        MessageBox.Show("Se han agregado correctamente el usuario, Usuarios agregados: " + cantidad, "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else { 
                        MessageBox.Show("No se pudo agregar el usuario, por favor intente nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                } catch(Exception ex) { 
                    MessageBox.Show("Error al conectar a la base de datos: " + ex.Message, "Error de conexión", MessageBoxButton.OK, MessageBoxImage.Error);

                }


            }
        }
    }
}

