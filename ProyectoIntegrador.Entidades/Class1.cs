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
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
    //Datos que NO se repiten en una venta:
    public class Venta
    {
        public int IdVenta { get; set; }
        public string TipoVenta { get; set; }

        public int IdCliente { get; set; }
        public int IdEmpleado { get; set; }

        public List<DetalleVenta> Detalles { get; set; }

        public Venta()
        {
            Detalles = new List<DetalleVenta>();
        }
        //Datos que se repiten en una venta:
        public class DetalleVenta
        {
            public int IdDetalle { get; set; }
            public int IdProducto { get; set; }
            public int Cantidad { get; set; }
        }
    }
}
//Comentario de prueba para ver si funciona el commit.