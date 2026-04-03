using MySql.Data.MySqlClient;
using Panel_de_Control.Models;
using System;
using System.Collections.Generic;

namespace Panel_de_Control.Data
{
    public class EquipoDAO
    {
        private Conexion conexion = new Conexion(); //Usar clase para crear conexion con la DB

        public List<Equipo> ObtenerEquipos()
        {
            var lista = new List<Equipo>();//Lista para almacenar los equipos obtenidos de la DB para usarlos en el datagrid

            using (var conn = conexion.ObtenerConexion())
            {
                conn.Open();

                string query = "SELECT * FROM equipos_biomedicos";

                var cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())//Recorrer los resultados y agregar cada equipo a la lista
                {
                    lista.Add(new Equipo
                    {
                        Id = reader.GetInt32("id"),
                        CodigoActivo = reader["codigoActivo"].ToString(),
                        Nombre = reader["nombre"].ToString(),
                        NumeroSerie = reader["numero_serie"].ToString(),
                        Modelo = reader["modelo"].ToString(),
                        Marca = reader["marca"].ToString(),
                        PeriodicidadMantenimiento = reader["periodicidad_mantenimiento"].ToString(),
                        UltimoMantenimiento = reader.IsDBNull(reader.GetOrdinal("ultimo_mantenimiento"))? (DateTime?)null: reader.GetDateTime(reader.GetOrdinal("ultimo_mantenimiento")), //Manejo de campos vacios posibles traidos desde la DB
                        Calibracion = reader["calibracion"].ToString(),
                        UltimaCalibracion = reader.IsDBNull(reader.GetOrdinal("ultima_calibracion"))? (DateTime?)null: reader.GetDateTime(reader.GetOrdinal("ultima_calibracion")), //Manejo de campos vacios posibles traidos desde la DB
                        Ubicacion = reader["ubicacion"].ToString(),
                        Responsable = reader["responsable"].ToString(),
                        FechaAdquisicion = reader.IsDBNull(reader.GetOrdinal("fecha_adquisicion"))? (DateTime?)null: reader.GetDateTime(reader.GetOrdinal("fecha_adquisicion")), //Manejo de campos vacios posibles traidos desde la DB

                        Estado = reader["estado"].ToString()
                    });
                }
            }

            return lista;
        }
    }
}