using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Panel_de_Control.Data
{
    public class Conexion
    {
        private string cadena = "server=localhost;database=Sibio;user=root;password=miguelanjel131;";//cadena para establecer los datos para la conexion con la BD

        public MySqlConnection ObtenerConexion()//Se crea una clase para la Obtener la conexion con la BD que se usara dentro de la logica de la app
        {
            return new MySqlConnection(cadena);//Retorna una nueva instancia (objeto) de MySqlConnection utilizando la cadena de conexión definida en la variable cadena.
        }
    }
}