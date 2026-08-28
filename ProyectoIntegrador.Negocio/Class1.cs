using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIntegrador.Negocio
{
    public class Conexion
    {
        private readonly string connectionString;

        public Conexion()
        {
            string ruta = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "database.txt"
            );

            if (!File.Exists(ruta))
            {
                throw new FileNotFoundException(
                    "No se encontró el archivo database.txt."
                );
            }

            string[] lineas = File.ReadAllLines(ruta);

            string server = "";
            string port = "";
            string database = "";
            string user = "";
            string password = "";

            foreach (string linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea))
                    continue;

                string[] partes = linea.Split(new char[] { '=' }, 2);

                if (partes.Length != 2)
                    continue;

                string clave = partes[0].Trim();
                string valor = partes[1].Trim();

                switch (clave)
                {
                    case "Server":
                        server = valor;
                        break;

                    case "Port":
                        port = valor;
                        break;

                    case "Database":
                        database = valor;
                        break;

                    case "User":
                        user = valor;
                        break;

                    case "Password":
                        password = valor;
                        break;
                }
            }

            if (string.IsNullOrEmpty(server) ||
                string.IsNullOrEmpty(port) ||
                string.IsNullOrEmpty(database) ||
                string.IsNullOrEmpty(user))
            {
                throw new Exception(
                    "El archivo database.txt no contiene todos los datos necesarios."
                );
            }

            connectionString =
                $"Server={server};" +
                $"Port={port};" +
                $"Database={database};" +
                $"Uid={user};" +
                $"Pwd={password};";
        }

        public MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(connectionString);
        }
    }
}