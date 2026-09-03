using MySql.Data.MySqlClient;
using ProyectoIntegrador.Entidades;
using ProyectoIntegrador.Negocio;
using System;
using System.Collections.Generic;
using System.Data;
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

public class ProductoNegocio
{
    private readonly Conexion conexion;

    public ProductoNegocio()
    {
        conexion = new Conexion();
    }

    // Alta (Create)
    public void AgregarProducto(Producto producto)
    {
        using (MySqlConnection cn = conexion.ObtenerConexion())
        {
            cn.Open();

            string consulta = @"INSERT INTO productos
                                    (codigo, descripcion, descripcion_larga,
                                     stock_online, stock_fisico,
                                     activo_para_venta, precio_actual)
                                    VALUES
                                    (@codigo, @descripcion, @descripcion_larga,
                                     @stock_online, @stock_fisico,
                                     @activo_para_venta, @precio_actual)";

            using (MySqlCommand comando = new MySqlCommand(consulta, cn))
            {
                comando.Parameters.AddWithValue("@codigo", producto.Codigo);
                comando.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                comando.Parameters.AddWithValue("@descripcion_larga", producto.DescripcionLarga);
                comando.Parameters.AddWithValue("@stock_online", producto.StockOnline);
                comando.Parameters.AddWithValue("@stock_fisico", producto.StockFisico);
                comando.Parameters.AddWithValue("@activo_para_venta", producto.ActivoParaVenta);
                comando.Parameters.AddWithValue("@precio_actual", producto.PrecioActual);

                comando.ExecuteNonQuery();
            }
        }
    }

    // Baja (Delete)
    public void EliminarProducto(int idProducto)
    {
        using (MySqlConnection cn = conexion.ObtenerConexion())
        {
            cn.Open();

            string consulta = @"DELETE FROM productos
                                    WHERE id_producto = @id_producto";

            using (MySqlCommand comando = new MySqlCommand(consulta, cn))
            {
                comando.Parameters.AddWithValue("@id_producto", idProducto);

                comando.ExecuteNonQuery();
            }
        }
    }

    // Modificación (Update)
    public void EditarProducto(Producto producto)
    {
        using (MySqlConnection cn = conexion.ObtenerConexion())
        {
            cn.Open();

            string consulta = @"UPDATE productos
                                    SET codigo = @codigo,
                                        descripcion = @descripcion,
                                        descripcion_larga = @descripcion_larga,
                                        stock_online = @stock_online,
                                        stock_fisico = @stock_fisico,
                                        activo_para_venta = @activo_para_venta,
                                        precio_actual = @precio_actual
                                    WHERE id_producto = @id_producto";

            using (MySqlCommand comando = new MySqlCommand(consulta, cn))
            {
                comando.Parameters.AddWithValue("@id_producto", producto.IdProducto);
                comando.Parameters.AddWithValue("@codigo", producto.Codigo);
                comando.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                comando.Parameters.AddWithValue("@descripcion_larga", producto.DescripcionLarga);
                comando.Parameters.AddWithValue("@stock_online", producto.StockOnline);
                comando.Parameters.AddWithValue("@stock_fisico", producto.StockFisico);
                comando.Parameters.AddWithValue("@activo_para_venta", producto.ActivoParaVenta);
                comando.Parameters.AddWithValue("@precio_actual", producto.PrecioActual);

                comando.ExecuteNonQuery();
            }
        }
    }

    // Consulta (Read)
    public DataTable ObtenerProductos()
    {
        using (MySqlConnection cn = conexion.ObtenerConexion())
        {
            cn.Open();

            string consulta = @"SELECT
                                        id_producto,
                                        codigo,
                                        descripcion,
                                        descripcion_larga,
                                        stock_online,
                                        stock_fisico,
                                        activo_para_venta,
                                        precio_actual
                                    FROM productos";

            using (MySqlCommand comando = new MySqlCommand(consulta, cn))
            {
                using (MySqlDataAdapter adaptador = new MySqlDataAdapter(comando))
                {
                    DataTable tabla = new DataTable();

                    adaptador.Fill(tabla);

                    return tabla;
                }
            }
        }
    }

    // Nueva: búsqueda centralizada para la capa de presentación.
    // Si texto es null/empty devuelve todos los productos.
    // Si texto es numérico busca por id exacto y también por LIKE en campos relevantes.
    // Si no es numérico busca por descripcion/codigo/descripcion_larga con LIKE.
    public DataTable BuscarProductos(string texto)
    {
        using (MySqlConnection cn = conexion.ObtenerConexion())
        {
            cn.Open();

            string consulta;
            MySqlCommand comando = cn.CreateCommand();

            if (string.IsNullOrWhiteSpace(texto))
            {
                consulta = @"SELECT
                                id_producto,
                                codigo,
                                descripcion,
                                descripcion_larga,
                                stock_online,
                                stock_fisico,
                                activo_para_venta,
                                precio_actual
                              FROM productos";
            }
            else
            {
                // Si es número, permitir buscar por id exacto + LIKE en otras columnas
                if (int.TryParse(texto.Trim(), out int idBusqueda))
                {
                    consulta = @"SELECT
                                    id_producto,
                                    codigo,
                                    descripcion,
                                    descripcion_larga,
                                    stock_online,
                                    stock_fisico,
                                    activo_para_venta,
                                    precio_actual
                                 FROM productos
                                 WHERE id_producto = @idBusqueda
                                    OR descripcion LIKE @bus
                                    OR codigo LIKE @bus
                                    OR descripcion_larga LIKE @bus";
                    comando.Parameters.AddWithValue("@idBusqueda", idBusqueda);
                    comando.Parameters.AddWithValue("@bus", "%" + texto.Trim() + "%");
                }
                else
                {
                    consulta = @"SELECT
                                    id_producto,
                                    codigo,
                                    descripcion,
                                    descripcion_larga,
                                    stock_online,
                                    stock_fisico,
                                    activo_para_venta,
                                    precio_actual
                                 FROM productos
                                 WHERE descripcion LIKE @bus
                                    OR codigo LIKE @bus
                                    OR descripcion_larga LIKE @bus";
                    comando.Parameters.AddWithValue("@bus", "%" + texto.Trim() + "%");
                }
            }

            comando.CommandText = consulta;

            using (MySqlDataAdapter adaptador = new MySqlDataAdapter(comando))
            {
                DataTable tabla = new DataTable();
                adaptador.Fill(tabla);
                return tabla;
            }
        }
    }
}
