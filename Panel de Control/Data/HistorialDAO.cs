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


        //METODO PARA OBTENER LAS PETICIONES DE UN EQUIPO ESPECIFICO DESDE LA DB
        public List<PeticionEquipo> ObtenerPorEquipo(int equipoId)//Creamos Metodo para obtener las peticiones de un equipo especifico
        {
            var lista = new List<PeticionEquipo>();//Lista para almacenar los datos referentes a las peticiones obtenidas
            
            using (var conn = conexion.ObtenerConexion())//Obtenemos la conexion a la DB con using para asegurar que se cierre correctamente
            {
                conn.Open();//Abrimos la conexion

                //Creamos la consulta SQL para obtener los datos de las peticiones del equipo, incluyendo el nombre del tecnico que atendio la peticion
                //Usa alias como u o h para referirse a las tablas y evitar confusiones, ademas de usar parametros para evitar inyecciones SQL
                string query = @"
                    SELECT h.id, h.tipo, h.descripcion, h.fecha, h.estado, u.nombre AS tecnico, h.fecha_resolucion, h.costos_generados
                    FROM historial_peticiones h
                    INNER JOIN usuarios u ON h.usuario_id = u.id
                    WHERE h.equipo_id = @equipoId";

                var cmd = new MySqlCommand(query, conn);//Creamos el comando SQL y le asignamos la consulta y la conexion
                cmd.Parameters.AddWithValue("@equipoId", equipoId);//Agregamos el parametro de la consulta para evitar inyecciones SQL

                var reader = cmd.ExecuteReader();//Ejecutamos la consulta y obtenemos un reader para leer los resultados

                while (reader.Read())//reader lee cada fila de la base de datos y mientras existan filas se ejecuta con el While=> sera true
                {
                        var fechaRes = reader["fecha_resolucion"];//Obtenemos el valor de la fecha de resolucion, pero como puede ser null
                        var costoGen = reader["costos_generados"];//Obtenemos el valor de los costos generados, pero como puede ser null

                        lista.Add(new PeticionEquipo
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Tipo = reader["tipo"].ToString(),
                            Descripcion = reader["descripcion"]?.ToString(),
                            Fecha = Convert.ToDateTime(reader["fecha"]),
                            Estado = reader["estado"].ToString().Trim().ToLower(),
                            Tecnico = reader["tecnico"]?.ToString(),
                            FechaResolucion = fechaRes != DBNull.Value ? Convert.ToDateTime(fechaRes) : DateTime.MinValue,
                            CostosGenerados = costoGen != DBNull.Value ? Convert.ToDecimal(costoGen) : 0
                        });
                    
                }
            }

            return lista;//Devuelve la lista con las peticiones obtenidas de cada equipo especifico
        }

        //METODO PARA CONTAR EL TOTAL DE PETICIONES PENDIENTES EN LA BASE DE DATOS

        public int ContarPeticionesPendientes()//Metodo que permite contar el total de peticiones pendientes en la BD
        {
            using (var conn = new Conexion().ObtenerConexion())
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM historial_peticiones WHERE estado = 'pendiente'";

                var cmd = new MySqlCommand(query, conn);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }


        //METODO PARA INSERTAR UNA NUEVA PETICION EN LA BASE DE DATOS
        public void InsertarPeticion(PeticionEquipo peticion)
        {
            using (var conn = new Conexion().ObtenerConexion())
            {
                conn.Open();

                string query = @"
            INSERT INTO historial_peticiones
            (equipo_id, usuario_id, tipo, descripcion, estado, tecnico, fecha, fecha_resolucion, costos_generados)
            VALUES
            (@equipoId, @usuarioId, @tipo, @descripcion, @estado, @tecnico, @fecha, @fechaResolucion, @costos)";

                var cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@equipoId", peticion.EquipoId);
                cmd.Parameters.AddWithValue("@usuarioId", peticion.UsuarioId); // si no tienes usuario fijo, puedes poner null o 0
                cmd.Parameters.AddWithValue("@tipo", peticion.Tipo);
                cmd.Parameters.AddWithValue("@descripcion", peticion.Descripcion);
                cmd.Parameters.AddWithValue("@estado", peticion.Estado);
                cmd.Parameters.AddWithValue("@tecnico", peticion.Tecnico);
                cmd.Parameters.AddWithValue("@fecha", peticion.Fecha);
                cmd.Parameters.AddWithValue("@fechaResolucion", peticion.FechaResolucion ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@costos", peticion.CostosGenerados);

                cmd.ExecuteNonQuery();

                // Obtener el Id generado automáticamente
                peticion.Id = (int)cmd.LastInsertedId;
            }
        }
        //METODO PARA ACTUALIZAR EL ESTADO DE UNA PETICION EN LA BASE DE DATOS
        public void ActualizarPeticion(PeticionEquipo peticion)
        {
            using (var conn = conexion.ObtenerConexion())
            {
                conn.Open();
                //CREA LA CONSULTA SQL ADEMAS EN CASO DE QUE LA EXPRESION SEA RESUELTO O CANELADO, SE ACTUALIZA LA FECHA DE RESOLUCION, SI NO SE DEJA IGUAL
                string query = @"
                UPDATE historial_peticiones
                SET estado = @estado,
                costos_generados = @costos,
                fecha_resolucion = CASE WHEN @estado IN ('resuelto', 'cancelado') THEN NOW() ELSE fecha_resolucion END
                WHERE id = @id";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@estado", peticion.Estado);
                cmd.Parameters.AddWithValue("@tecnico", peticion.Tecnico);
                cmd.Parameters.AddWithValue("@costos", peticion.CostosGenerados);
                cmd.Parameters.AddWithValue("@id", peticion.Id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
