namespace ProyectoIntegrador.DataAccess.Implementations.MsSql
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data;
    internal class ProductoDaoMsSql(IConnectionFactory connectionFactory)
        : AdoDaoBase<Producto>(connectionFactory), IProductoDao
    {
        private const string TableName = "productos";
        private const string PrimaryKeyColumn = "id_producto";
        private const string SelectBase = $"""
            SELECT 
                {PrimaryKeyColumn}, sku, nombre, descripcion, descripcion_larga, imagen_ruta, precio, activo_para_venta
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
                        (sku, nombre, descripcion, descripcion_larga, imagen_ruta, precio, activo_para_venta) 
                    OUTPUT INSERTED.{PrimaryKeyColumn}
                    VALUES 
                        (@Sku, @Nombre, @Descripcion, @DescripcionLarga, @ImagenRuta, @Precio, @ActivoParaVenta);
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
                    DELETE FROM
                        {TableName} 
                    WHERE 
                        {PrimaryKeyColumn} = @Id
                """);
            AddParameter(command, "@Id", id);
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
        public override Producto? GetById(int id)
        {
            if (id <= 0)
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                    WHERE 
                        {PrimaryKeyColumn} = @Id
                """);
            AddParameter(command, "@Id", id);
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
                        sku = @Sku
                        , nombre = @Nombre
                        , descripcion = @Descripcion
                        , descripcion_larg = @DescripcionLarga
                        , imagen_ruta = @ImagenRuta
                        , precio = @Precio
                        , activo_para_venta = @ActivoParaVenta
                    WHERE 
                        {PrimaryKeyColumn} = @Id
                """);
            AddEntityParameters(command, entity);
            AddParameter(command, "@Id", entity.IdProducto);
            return command.ExecuteNonQuery() > 0;
        }
        private static void AddEntityParameters(IDbCommand command, Producto entity)
        {
            List<(string Name, object? Value)> parameters =
            [
                ("@Sku", entity.Sku),
                ("@Nombre", entity.Nombre),
                ("@Descripcion", entity.Descripcion),
                ("@DescripcionLarga", entity.DescripcionLarga),
                ("@ImagenRuta", entity.ImagenRuta),
                ("@Precio", entity.Precio),
                ("@ActivoParaVenta", entity.ActivoParaVenta)
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
                ActivoParaVenta = reader.GetBoolean(reader.GetOrdinal("activo_para_venta"))
            };
        }
    }
}
