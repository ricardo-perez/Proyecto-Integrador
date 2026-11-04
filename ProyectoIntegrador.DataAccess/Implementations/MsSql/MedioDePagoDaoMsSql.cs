namespace ProyectoIntegrador.DataAccess.Implementations.MsSql
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data;
    internal class MedioDePagoDaoMsSql(IConnectionFactory connectionFactory)
        : AdoDaoBase<MedioDePago>(connectionFactory), IMedioDePagoDao
    {
        private const string TableName = "medios_de_pago";
        private const string PrimaryKeyColumn = "id_medio_de_pago";
        private const string SelectBase = $"""
            SELECT 
                {PrimaryKeyColumn}, nombre, descripcion
            FROM 
                {TableName}
        """;
        public override IEnumerable<MedioDePago> GetAll()
        {
            var entityList = new List<MedioDePago>();
            using var connection = CreateConnection();
            using var command = CreateCommand(connection, SelectBase);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                entityList.Add(Map(reader));
            return entityList;
        }
        public override MedioDePago? GetById(int id)
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
        private static MedioDePago Map(IDataReader reader)
        {
            int descripcionIndex = reader.GetOrdinal("descripcion");
            return new MedioDePago
            {
                IdMedioDePago = reader.GetInt32(reader.GetOrdinal(PrimaryKeyColumn)),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Descripcion = reader.IsDBNull(descripcionIndex)
                    ? null
                    : reader.GetString(descripcionIndex)
            };
        }
        public override int Add(MedioDePago entity)
        {
            throw new NotImplementedException();
        }
        public override bool Update(MedioDePago entity)
        {
            throw new NotImplementedException();
        }
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
