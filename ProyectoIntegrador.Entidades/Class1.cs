using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIntegrador.Entidades
{
    public class Class1
    {
    }

    public class Producto
    {
        public int IdProducto { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionLarga { get; set; }
        public int StockOnline { get; set; }
        public int StockFisico { get; set; }
        public bool ActivoParaVenta { get; set; }
        public decimal PrecioActual { get; set; }
    }

    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public string MedioDePago { get; set; }
        public decimal MontoTotal { get; set; }
    }

    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class Factura
    {
        public int IdFactura { get; set; }
        public int IdVenta { get; set; }
        public string TipoFactura { get; set; }
        public string PuntoVenta { get; set; }
        public string NumeroFactura { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal Total { get; set; }
    }
}