namespace ProyectoIntegrador.Business
{
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data.Common;
    public class MedioDePagoService(IMedioDePagoDao medioDePagoDao)
    {
        private readonly IMedioDePagoDao _medioDePagoDao = medioDePagoDao;
        public Result<List<MedioDePago>> ObtenerMediosDePago()
        {
            try
            {
                var mediosDePago = _medioDePagoDao.GetAll();
                return Result<List<MedioDePago>>.Success(mediosDePago.ToList());
            }
            catch (DbException ex)
            {
                return Result<List<MedioDePago>>.Failure($"Error de comunicación con la base de datos: {ex.Message}", ErrorType.Database);
            }
            catch (Exception ex)
            {
                return Result<List<MedioDePago>>.Failure($"Ocurrió un error inesperado durante la consulta: {ex.Message}", ErrorType.Unexpected);
            }
        }
    }
}
