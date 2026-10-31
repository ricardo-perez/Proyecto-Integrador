namespace ProyectoIntegrador.DataAccess.Implementations.MsSql
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System;
    using System.Data;
    using System.Text.RegularExpressions;

    using static System.Net.Mime.MediaTypeNames;

    public class ProductoDaoMsSql(IConnectionFactory connectionFactory)
        : AdoDaoBase<Producto>(connectionFactory), IProductoDao
    {
        private const string TableName = "productos";
        private const string PrimaryKeyColumn = "id_producto";
        private const string SelectBase = $"""
            SELECT 
                {PrimaryKeyColumn}, sku, nombre, descripcion, descripcion_larga, imagen_ruta, precio, activo_para_venta, activo
            FROM 
                {TableName}
        """;
        public override int Add(Producto entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    INSERT INTO {TableName} 
                        (sku, nombre, descripcion, descripcion_larga, imagen_ruta, precio, activo_para_venta, activo) 
                    OUTPUT INSERTED.{PrimaryKeyColumn}
                    VALUES 
                        (@sku, @nombre, @descripcion, @descripcionLarga, @imagenRuta, @precio, @activoParaVenta, @activo);
                """);
            AddEntityParameters(command, entity);
            var result = command.ExecuteScalar();
            if (result == null || result == DBNull.Value)
                return 0;
            return Convert.ToInt32(result);
        }
        public override bool Delete(int id)
        {
            if (id <= 0)
                return false;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    UPDATE
                        {TableName} 
                    SET
                        activo = 0
                    WHERE 
                        {PrimaryKeyColumn} = @id
                        AND activo = 1
                """);
            AddParameter(command, "@id", id);
            return command.ExecuteNonQuery() > 0;
        }
        public override IEnumerable<Producto> GetAll()
        {
            var entityList = new List<Producto>();
            using var connection = CreateConnection();
            using var command = CreateCommand(connection, SelectBase);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                entityList.Add(Map(reader));
            return entityList;
        }
        public IEnumerable<Producto> GetPagedWithStock(int pageNumber, int pageSize)
        {
            var entityList = new List<Producto>();
            using var connection = CreateConnection();
            var query = """
            SELECT
                p.id_producto,
                p.sku,
                p.nombre,
                p.descripcion,
                p.descripcion_larga,
                p.imagen_ruta,
                p.precio,
                p.activo_para_venta,
                p.activo,
                ISNULL(SUM(ms.cantidad), 0) AS stock_total
            FROM
                productos AS p
            LEFT JOIN movimientos_stock AS ms ON ms.id_producto = p.id_producto
            WHERE
                p.activo = 1
                AND p.activo_para_venta = 1
            GROUP BY
                p.id_producto,
                p.sku,
                p.nombre,
                p.descripcion,
                p.descripcion_larga,
                p.imagen_ruta,
                p.precio,
                p.activo_para_venta,
                p.activo
            ORDER BY
                p.id_producto
            OFFSET(@pageNumber - 1) * @pageSize ROWS
            FETCH NEXT @pageSize ROWS ONLY;
            """;
            using var command = CreateCommand(connection, query);
            AddParameter(command, "@pageNumber", pageNumber);
            AddParameter(command, "@pageSize", pageSize);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                entityList.Add(MapWithStock(reader));
            return entityList;
        }
        public override Producto? GetById(int id)
        {
            if (id <= 0)
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                    WHERE 
                        {PrimaryKeyColumn} = @id
                """);
            AddParameter(command, "@id", id);
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
        }
        public override bool Update(Producto entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            if (entity.IdProducto <= 0)
                return false;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    UPDATE
                        {TableName}
                    SET 
                        sku = @sku
                        , nombre = @nombre
                        , descripcion = @descripcion
                        , descripcion_larga = @descripcionLarga
                        , imagen_ruta = @imagenRuta
                        , precio = @precio
                        , activo_para_venta = @activoParaVenta
                        , activo = @activo
                    WHERE 
                        {PrimaryKeyColumn} = @id
                """);
            AddEntityParameters(command, entity);
            AddParameter(command, "@id", entity.IdProducto);
            return command.ExecuteNonQuery() > 0;
        }
        public int GetTotalCountForCatalog()
        {
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                SELECT
                    COUNT(*) AS Cantidad_Productos
                FROM
                    productos 
                WHERE
                    activo = 1 
                    AND activo_para_venta = 1;
                """);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetInt32(reader.GetOrdinal("Cantidad_Productos"));
            }
            return 0;
        }
        private static void AddEntityParameters(IDbCommand command, Producto entity)
        {
            List<(string Name, object? Value)> parameters =
            [
                ("@sku", entity.Sku),
                ("@nombre", entity.Nombre),
                ("@descripcion", entity.Descripcion),
                ("@descripcionLarga", entity.DescripcionLarga),
                ("@imagenRuta", entity.ImagenRuta),
                ("@precio", entity.Precio),
                ("@activoParaVenta", entity.ActivoParaVenta),
                ("@Activo", entity.Activo)
            ];
            AddParameters(command, parameters);
        }
        private static Producto Map(IDataReader reader)
        {
            int descripcionLargaIndex = reader.GetOrdinal("descripcion_larga");
            int imagenRutaIndex = reader.GetOrdinal("imagen_ruta");
            return new Producto
            {
                IdProducto = reader.GetInt32(reader.GetOrdinal("id_producto")),
                Sku = reader.GetString(reader.GetOrdinal("sku")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Descripcion = reader.GetString(reader.GetOrdinal("descripcion")),
                DescripcionLarga = reader.IsDBNull(descripcionLargaIndex)
                    ? null
                    : reader.GetString(descripcionLargaIndex),
                ImagenRuta = reader.IsDBNull(imagenRutaIndex)
                    ? null
                    : reader.GetString(imagenRutaIndex),
                Precio = reader.GetDecimal(reader.GetOrdinal("precio")),
                ActivoParaVenta = reader.GetBoolean(reader.GetOrdinal("activo_para_venta")),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
            };
        }
        private static Producto MapWithStock(IDataReader reader)
        {
            var producto = Map(reader);
            int stockIndex = reader.GetOrdinal("stock_total");
            producto.Stock = reader.IsDBNull(stockIndex) 
                ? 0 
                : Convert.ToInt32(reader.GetValue(stockIndex));
            return producto;
        }
    }
}
