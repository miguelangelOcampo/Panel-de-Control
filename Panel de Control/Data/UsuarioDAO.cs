using MySql.Data.MySqlClient;
using Panel_de_Control.Models;
using System;
using System.Collections.Generic;

namespace Panel_de_Control.Data
{
    internal class UsuarioDAO
    {
        private Conexion conexion = new Conexion(); // Usamos tu clase de conexión

        // Método para obtener todos los usuarios
        public List<Usuario> ObtenerTodos()
        {
            var lista = new List<Usuario>();

            using (var conn = conexion.ObtenerConexion())
            {
                conn.Open();

                string query = "SELECT id, nombre FROM usuarios ORDER BY nombre"; // Solo seleccionamos los campos que necesitamos

                var cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Usuario
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Nombre = reader["nombre"].ToString()
                        });
                    }
                }
            }

            return lista;
        }
    }
}