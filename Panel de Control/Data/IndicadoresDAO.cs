using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Panel_de_Control.Data
{
    public class IndicadoresDAO
    {
        private Conexion conexion = new Conexion();

        // METODO PARA CALCULAR LA TASA DE OPERATIVIDAD: (EQUIPOS OPERATIVOS / TOTAL DE EQUIPOS) * 100
        public double ObtenerTasaOperatividad()
        {
            using (var conn = conexion.ObtenerConexion())
            {
                conn.Open();
                string query = @"SELECT 
                                COUNT(*) AS total,
                                SUM(CASE WHEN estado = 'Operativo' THEN 1 ELSE 0 END) AS operativos
                                FROM equipos_biomedicos";

                var cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    double total = Convert.ToDouble(reader["total"]);
                    double operativos = reader["operativos"] != DBNull.Value ? Convert.ToDouble(reader["operativos"]) : 0;

                    return total == 0 ? 0 : (operativos / total) * 100;
                }
            }
            return 0;
        }

        // METODO PARA CALCULAR EL TIEMPO PROMEDIO DE RESOLUCIÓN: AVG(TIMESTAMPDIFF(HOUR, fecha, fecha_resolucion))
        public double ObtenerTiempoPromedioResolucion()
        {
            using (var conn = conexion.ObtenerConexion())
            {
                conn.Open();
                string query = @"SELECT AVG(TIMESTAMPDIFF(HOUR, fecha, fecha_resolucion)) AS promedio
                                 FROM historial_peticiones
                                 WHERE fecha_resolucion IS NOT NULL";

                var cmd = new MySqlCommand(query, conn);
                var result = cmd.ExecuteScalar();

                return result != DBNull.Value ? Convert.ToDouble(result) : 0;
            }
        }

        // METODO PARA OBTENER LOS EQUIPOS PENDIENTES
        public List<string> ObtenerEquiposPendientes()
        {
            var lista = new List<string>();

            using (var conn = conexion.ObtenerConexion())
            {
                conn.Open();
                string query = @"SELECT DISTINCT e.CodigoActivo
                                 FROM equipos_biomedicos e
                                 JOIN historial_peticiones h ON e.id = h.equipo_id
                                 WHERE h.estado = 'pendiente'";

                var cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(reader["CodigoActivo"].ToString());
                }
            }

            return lista;
        }

        //METODO PARA OBTENER LOS COSTOS GENERADOS EN EL MES ACTUAL Y EL EQUIPO QUE MÁS COSTOS HA GENERADO
        public (double total, string equipoTop) ObtenerCostosMensuales()
        {
            using (var conn = conexion.ObtenerConexion())
            {
                conn.Open();

                double total = 0;
                string equipo = "N/A";

                // TOTAL DEL MES
                var cmd1 = new MySqlCommand(@"
                    SELECT SUM(costos_generados) 
                    FROM historial_peticiones
                    WHERE MONTH(fecha) = MONTH(CURRENT_DATE())
                    AND YEAR(fecha) = YEAR(CURRENT_DATE())", conn);

                var result1 = cmd1.ExecuteScalar();
                total = result1 != DBNull.Value ? Convert.ToDouble(result1) : 0;

                // EQUIPO MÁS COSTOSO
                var cmd2 = new MySqlCommand(@"
                    SELECT e.CodigoActivo, SUM(h.costos_generados) AS total
                    FROM historial_peticiones h
                    JOIN equipos_biomedicos e ON h.equipo_id = e.id
                    GROUP BY e.CodigoActivo
                    ORDER BY total DESC
                    LIMIT 1;", conn);

                var reader = cmd2.ExecuteReader();
                if (reader.Read())
                {
                    equipo = reader["CodigoActivo"].ToString();
                }

                return (total, equipo);
            }
        }
    }
}