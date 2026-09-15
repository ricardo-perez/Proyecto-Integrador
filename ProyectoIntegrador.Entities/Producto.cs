using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProyectoIntegrador.Entities
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string? DescripcionLarga { get; set; }
        public string? ImagenRuta { get; set; }
        public decimal Precio { get; set; } = 0.00m;
        public bool ActivoParaVenta { get; set; } = false;
    }
}
