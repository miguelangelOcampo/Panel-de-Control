using MySql.Data.MySqlClient;
using Panel_de_Control.Models;
using System;
using System.Collections.Generic;

namespace Panel_de_Control.Data
{
    public class EquipoDAO
    {
        private Conexion conexion = new Conexion(); //Usar clase para crear conexion con la DB


        //METODO PARA INSERTAR UN NUEVO EQUIPO EN LA DB
        public void InsertarEquipo(Equipo equipo)//Creamos metodo para insertar un nuevo equipo en la DB
        {
            using (var conn = new Conexion().ObtenerConexion())//usamos la clase Conexion para obtener la conexion a la DB, el using se encarga de cerrar la conexion al finalizar el bloque
            {
                conn.Open();

                //consulta SQL
                string query = @"INSERT INTO equipos_biomedicos 
                                (codigoActivo, nombre, numero_serie, modelo, marca, periodicidad_mantenimiento,
                                ultimo_mantenimiento, calibracion, ultima_calibracion,
                                ubicacion, responsable, fecha_adquisicion, estado)
                                VALUES
                                (@codigoActivo, @nombre, @serie, @modelo, @marca, @periodicidad,
                                @ultimoM, @calibracion, @ultimaCal,
                                @ubicacion, @responsable, @fechaAdq, @estado)";

                var cmd = new MySqlCommand(query, conn);//Creamos el comando SQL y le asignamos la consulta y la conexion

                // Strings
                cmd.Parameters.AddWithValue("@codigoActivo", equipo.CodigoActivo);
                cmd.Parameters.AddWithValue("@nombre", equipo.Nombre);
                cmd.Parameters.AddWithValue("@serie", equipo.NumeroSerie);
                cmd.Parameters.AddWithValue("@modelo", equipo.Modelo);
                cmd.Parameters.AddWithValue("@marca", equipo.Marca);
                cmd.Parameters.AddWithValue("@periodicidad", equipo.PeriodicidadMantenimiento);
                cmd.Parameters.AddWithValue("@calibracion", equipo.Calibracion);
                cmd.Parameters.AddWithValue("@ubicacion", equipo.Ubicacion);
                cmd.Parameters.AddWithValue("@responsable", equipo.Responsable);
                cmd.Parameters.AddWithValue("@estado", equipo.Estado);

                // MANEJO DE CAMPOS VACIOS POSIBLES DateTime?
                cmd.Parameters.AddWithValue("@ultimoM",
                    equipo.UltimoMantenimiento ?? (object)DBNull.Value);

                cmd.Parameters.AddWithValue("@ultimaCal",
                    equipo.UltimaCalibracion ?? (object)DBNull.Value);

                cmd.Parameters.AddWithValue("@fechaAdq",
                    equipo.FechaAdquisicion ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();
            }
        }

        //METODO PARA OBTENER LA LISTA DE EQUIPOS DESDE LA DB
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
        //METODO PARA ELIMINAR REGISTRO DEL EQUIPO EN LA DB, RECIBE EL ID DEL EQUIPO A ELIMINAR
        public void EliminarEquipo(int id)
        {
            using (var conn = new Conexion().ObtenerConexion())
            {
                conn.Open();
                string query = "DELETE FROM equipos_biomedicos WHERE id = @id";
                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        //METODO PARA ACTUALIZAR EL ESTADO DEL EQUIPO EN LA DB, RECIBE EL ID DEL EQUIPO Y EL NUEVO ESTADO
        public void ActualizarEstado(int id, string estado)
        {
            using (var conn = new Conexion().ObtenerConexion())
            {
                conn.Open();

                string query = @"UPDATE equipos_biomedicos 
                         SET estado = @estado 
                         WHERE id = @id";

                var cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@estado", estado);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        //METODO PARA ACTUALIZAR LOS DATOS DE UN EQUIPO EN LA DB
        public void ActualizarEquipo(Equipo equipo)
        {
            using (var conn = new Conexion().ObtenerConexion())
            {
                conn.Open();

                string query = @"UPDATE equipos_biomedicos SET 
                            codigoActivo=@codigoActivo,
                            nombre=@nombre,
                            numero_serie=@serie,
                            modelo=@modelo,
                            marca=@marca,
                            periodicidad_mantenimiento=@periodicidad,
                            ultimo_mantenimiento=@ultimoM,
                            calibracion=@calibracion,
                            ultima_calibracion=@ultimaCal,
                            ubicacion=@ubicacion,
                            responsable=@responsable,
                            fecha_adquisicion=@fechaAdq,
                            estado=@estado
                         WHERE id=@id";

                var cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", equipo.Id);
                cmd.Parameters.AddWithValue("@codigoActivo", equipo.CodigoActivo);
                cmd.Parameters.AddWithValue("@nombre", equipo.Nombre);
                cmd.Parameters.AddWithValue("@serie", equipo.NumeroSerie);
                cmd.Parameters.AddWithValue("@modelo", equipo.Modelo);
                cmd.Parameters.AddWithValue("@marca", equipo.Marca);
                cmd.Parameters.AddWithValue("@periodicidad", equipo.PeriodicidadMantenimiento);
                cmd.Parameters.AddWithValue("@calibracion", equipo.Calibracion);
                cmd.Parameters.AddWithValue("@ubicacion", equipo.Ubicacion);
                cmd.Parameters.AddWithValue("@responsable", equipo.Responsable);
                cmd.Parameters.AddWithValue("@estado", equipo.Estado);

                cmd.Parameters.AddWithValue("@ultimoM", equipo.UltimoMantenimiento ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ultimaCal", equipo.UltimaCalibracion ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaAdq", equipo.FechaAdquisicion ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();
            }
        }

    }
}