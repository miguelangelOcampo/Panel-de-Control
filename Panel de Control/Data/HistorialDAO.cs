using Panel_de_Control.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;//Libreria para trabajar con MySQL


namespace Panel_de_Control.Data
{
    internal class HistorialDAO//Clase para trabajar con la base de datos (Especificamente con las peticiones de los equipos)
    {
        private Conexion conexion = new Conexion();//Usar clase para crear conexion con la DB

        public List<PeticionEquipo> ObtenerPorEquipo(int equipoId)//Creamos Metodo para obtener las peticiones de un equipo especifico
        {
            var lista = new List<PeticionEquipo>();//Lista para almacenar los datos referentes a las peticiones obtenidas
            
            using (var conn = conexion.ObtenerConexion())//Obtenemos la conexion a la DB con using para asegurar que se cierre correctamente
            {
                conn.Open();//Abrimos la conexion

                //Creamos la consulta SQL para obtener los datos de las peticiones del equipo, incluyendo el nombre del tecnico que atendio la peticion
                //Usa alias como u o h para referirse a las tablas y evitar confusiones, ademas de usar parametros para evitar inyecciones SQL
                string query = @"
                    SELECT h.tipo, h.fecha, h.estado, u.nombre,  h.descripcion  AS tecnico
                    FROM historial_peticiones h
                    INNER JOIN usuarios u ON h.usuario_id = u.id
                    WHERE h.equipo_id = @equipoId";

                var cmd = new MySqlCommand(query, conn);//Creamos el comando SQL y le asignamos la consulta y la conexion
                cmd.Parameters.AddWithValue("@equipoId", equipoId);//Agregamos el parametro de la consulta para evitar inyecciones SQL

                var reader = cmd.ExecuteReader();//Ejecutamos la consulta y obtenemos un reader para leer los resultados

                while (reader.Read())//reader lee cada fila de la base de datos y mientras existan filas se ejecuta con el While=> sera true
                {
                    lista.Add(new PeticionEquipo //Va agregando a la lista los datos de cada peticion obtenida de la base de datos
                    {
                        Tipo = reader["tipo"].ToString(),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        Estado = reader["estado"].ToString(),
                        Tecnico = reader["tecnico"].ToString(),
                        Descripcion = reader["descripcion"]?.ToString()
                    });
                }
            }

            return lista;//Devuelve la lista con las peticiones obtenidas de cada equipo especifico
        }
    }
}
