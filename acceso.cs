using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace motos11
{
    internal class acceso
    {
        SqlConnection conexion;
        SqlCommand comando;
        string cadena;

        public acceso()
        {
            cadena = @"Data Source=DESKTOP-U2NBR94\SQLEXPRESS;Initial Catalog=Concesionaria;Integrated Security=True";
            conexion = new SqlConnection(cadena);
            comando = new SqlCommand();
        }
        private void conectar()
        {
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
        }
        public void desconectar()
        {
            conexion.Close();
        }
        public DataTable consultarDB(string consultaSQL)
        {
            DataTable tabla = new DataTable();
            conectar();
            comando.CommandText = consultaSQL;
            tabla.Load(comando.ExecuteReader());
            desconectar();
            return tabla;
        }
        public int actualizarDB(string consultaSQL, List<parametro> lParametro)
        {
            int filasAfectadas;
            conectar();
            comando.CommandText = consultaSQL;
            comando.Parameters.Clear();
            foreach (parametro p in lParametro)
            {
                comando.Parameters.AddWithValue(p.Nombre, p.Valor);
            }
            filasAfectadas = comando.ExecuteNonQuery();
            desconectar();
            return filasAfectadas;
        }
        public int actualizarDB(string consultaSQL)
        {
            int filasAfectadas;
            conectar();
            comando.CommandText = consultaSQL;          
            filasAfectadas = comando.ExecuteNonQuery();
            desconectar();
            return filasAfectadas;
        }
    }
}
