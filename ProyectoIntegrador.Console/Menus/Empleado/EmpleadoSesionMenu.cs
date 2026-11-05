namespace ProyectoIntegrador.Console.Menus.Empleado
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Console.Helpers;
    using ProyectoIntegrador.Entities;
    using System;
    internal class EmpleadoSesionMenu(Empleado empleado, ProductoService productoService, MovimientoStockService movimientoStockService)
    {
        private readonly Empleado _empleado = empleado;
        private readonly ProductoService _productoService = productoService;
        private readonly MovimientoStockService _movimientoStockService = movimientoStockService;
        internal void Show()
        {
            while (true)
            {
                Console.Clear();
                ConsoleHelper.Messages.OK($"Bienvenid@, {_empleado.Nombre} {_empleado.Apellido} [ID: {_empleado.IdEmpleado}]");
                Console.Write("""
                    BACKOFFICE PRODUCTOS
                    ====================
                    [1] Agregar
                    [2] Gestionar
                    [0] Cerrar sesión
                    Ingrese una opción: 
                    """);

                var input = Console.ReadLine()?.Trim();
                if (!int.TryParse(input, out var option))
                {
                    ConsoleHelper.Messages.OpcionNoValida();
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                switch (option)
                {
                    case 1:
                        AddProduct();
                        break;
                    case 2:
                        EditProducts();
                        break;
                    case 0:
                        if (SignOut())
                            return;
                        break;
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        ConsoleHelper.Messages.Continuar();
                        break;
                }
            }
        }
        private static bool SignOut()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("""
                    ¿Está seguro de que desea cerrar la sesión?
                    [1] Sí
                    [2] No
                    Ingrese una opción: 
                    """);

                var input = Console.ReadLine()?.Trim();
                if (!int.TryParse(input, out var option))
                {
                    ConsoleHelper.Messages.OpcionNoValida();
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                switch (option)
                {
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Cerrando sesión...");
                        Console.ResetColor();
                        ConsoleHelper.Messages.Continuar();
                        return true;
                    case 2:
                        return false;
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        ConsoleHelper.Messages.Continuar();
                        break;
                }
            }
        }
        private void AddProduct()
        {
            var cancelKeyword = "0";
            Console.Clear();

            Console.WriteLine("""
            AGREGAR PRODUCTO  (Ingrese 0 en cualquier campo para cancelar)
            ==============================================================
            """);

            string? sku;
            while (true)
            {
                sku = ConsoleHelper.Inputs.GetValidText("SKU: ", cancelKeyword: cancelKeyword);
                if (sku == null)
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
                var result = _productoService.ExisteSku(sku);
                if (!result.IsSuccess)
                {
                    ConsoleHelper.Messages.Error(result.Message);
                    continue;
                }
                break;
            }

            string? nombre;
            while (true)
            {
                nombre = ConsoleHelper.Inputs.GetValidText("Nombre: ", cancelKeyword: cancelKeyword);
                if (nombre == null)
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
                break;

            }

            string? descripcion;
            while (true)
            {
                descripcion = ConsoleHelper.Inputs.GetValidText("Descripción corta: ", cancelKeyword: cancelKeyword);
                if (descripcion == null)
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
                break;
            }

            string? descripcionLarga;
            while (true)
            {
                descripcionLarga = ConsoleHelper.Inputs.GetValidText("Descripción larga: ", cancelKeyword: cancelKeyword);
                if (descripcionLarga == null)
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
                break;
            }

            decimal precio;
            while (true)
            {
                Console.Write("Precio: ");
                if (!decimal.TryParse(Console.ReadLine(), out precio))
                {
                    ConsoleHelper.Messages.Error("Precio no válido");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
                if (precio == 0)
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
                break;
            }

            var producto = new Producto
            {
                Sku = sku,
                Nombre = nombre,
                Descripcion = descripcion,
                DescripcionLarga = descripcionLarga,
                Precio = precio,
                ActivoParaVenta = false,
                Activo = false
            };
            while (true)
            {
                Console.Clear();
                Console.Write($"""
                DATOS CARGADOS
                ==============
                {producto}

                ¿Desea cargar el producto?
                [1] Sí
                [2] No
                Ingrese una opción: 
                """);

                var input = Console.ReadLine()?.Trim();
                if (!int.TryParse(input, out var option))
                {
                    ConsoleHelper.Messages.OpcionNoValida();
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                switch (option)
                {
                    case 1:
                        ConsoleHelper.Messages.Info("Guardando datos...");
                        var result = _productoService.CrearProducto(producto);
                        if (result.IsSuccess)
                            ConsoleHelper.Messages.OK($"{result.Message} (ID : {result.Value.IdProducto})");
                        else
                            ConsoleHelper.Messages.Error(result.Message);
                        ConsoleHelper.Messages.Continuar();
                        return;
                    case 2:
                        ConsoleHelper.Messages.Volviendo();
                        ConsoleHelper.Messages.Continuar();
                        return;
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        ConsoleHelper.Messages.Continuar();
                        break;
                }
            }
        }
        private void EditProducts()
        {
            while (true)
            {
                Console.Clear();
                var result = _productoService.ObtenerTodosLosProductos();
                if (!result.IsSuccess || result.Value == null || result.Value.Count == 0)
                {
                    Console.WriteLine("No hay productos en el sistema");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }

                var productos = result.Value;

                Console.WriteLine("""
                GESTIÓN DE PRODUCTOS
                ====================
                """);

                int index = 0;
                foreach (var item in productos)
                {
                    int stock = _productoService.ObtenerStock(item.IdProducto);
                    Console.WriteLine($"[{++index}] {item.Nombre} (SKU: {item.Sku}) - Stock: {stock} - Activo: {(item.Activo ? "Sí" : "No")} - Activo para Venta: {(item.ActivoParaVenta ? "Sí" : "No")}");
                }

                Console.Write("""
                [0] Volver
                Ingrese una opción: 
                """);

                var input = Console.ReadLine()?.Trim();
                if (!int.TryParse(input, out var opcion) || opcion < 0 || opcion > productos.Count)
                {
                    ConsoleHelper.Messages.OpcionNoValida();
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                if (opcion == 0)
                    return;

                var producto = productos[opcion - 1];
                EditProduct(producto);
            }
        }
        private void EditProduct(Producto producto)
        {
            while (true)
            {
                Console.Clear();
                int stock = _productoService.ObtenerStock(producto.IdProducto);
                Console.Write($"""
                    {producto.Nombre} [SKU: {producto.Sku}]
                    Stock actual: {stock} | Activo: {(producto.Activo ? "Sí" : "No")} | Venta: {(producto.ActivoParaVenta ? "Sí" : "No")}
                    ========================================
                    [1] Modificar
                    [2] Agregar stock
                    [3] Quitar stock
                    [0] Volver
                    Ingrese una opción: 
                    """);

                var input = Console.ReadLine()?.Trim();
                if (!int.TryParse(input, out var option))
                {
                    ConsoleHelper.Messages.OpcionNoValida();
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                switch (option)
                {
                    case 1:
                        var productoUpdated = UpdateProduct(producto);

                        if (productoUpdated == producto)
                            break;

                        ConsoleHelper.Messages.Info("Guardando cambios...");
                        var result = _productoService.ActualizarProducto(productoUpdated);
                        if (result.IsSuccess)
                        {
                            ConsoleHelper.Messages.OK("Datos actualizados correctamente");
                            producto.Sku = productoUpdated.Sku;
                            producto.Nombre = productoUpdated.Nombre;
                            producto.Descripcion = productoUpdated.Descripcion;
                            producto.DescripcionLarga = productoUpdated.DescripcionLarga;
                            producto.Precio = productoUpdated.Precio;
                            producto.ActivoParaVenta = productoUpdated.ActivoParaVenta;
                            producto.Activo = productoUpdated.Activo;
                        }
                        else
                        {
                            ConsoleHelper.Messages.Error(result.Message);
                        }
                        ConsoleHelper.Messages.Continuar();
                        break;
                    case 2:
                        UpdateStock(producto, true);
                        break;
                    case 3:
                        UpdateStock(producto, false);
                        break;
                    case 0:
                        return;
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        ConsoleHelper.Messages.Continuar();
                        break;
                }
            }
        }
        private Producto UpdateProduct(Producto producto)
        {
            var productoUpdated = new Producto
            {
                IdProducto = producto.IdProducto,
                Sku = producto.Sku,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                DescripcionLarga = producto.DescripcionLarga,
                ImagenRuta = producto.ImagenRuta,
                Precio = producto.Precio,
                ActivoParaVenta = producto.ActivoParaVenta,
                Activo = producto.Activo,
                Stock = producto.Stock
            };

            while (true)
            {
                Console.Clear();
                Console.Write($"""
                    MODIFICAR PRODUCTO
                    ==================
                    [1] SKU:                [{productoUpdated.Sku}]
                    [2] Nombre:             [{productoUpdated.Nombre}]
                    [3] Descripción:        [{productoUpdated.Descripcion}]
                    [4] Descrip. Larga:     [{productoUpdated.DescripcionLarga ?? "N/A"}]
                    [5] Precio:             [$ {productoUpdated.Precio:N2}]
                    [6] Activo para Venta:  [{(productoUpdated.ActivoParaVenta ? "Sí" : "No")}]
                    [7] Activo:             [{(productoUpdated.Activo ? "Sí" : "No")}]
                    [8] Guardar
                    [0] Volver
                    Ingrese una opción: 
                    """);

                var input = Console.ReadLine()?.Trim();
                if (!int.TryParse(input, out var option))
                {
                    ConsoleHelper.Messages.OpcionNoValida();
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                if (option == 0)
                {
                    if (Confirm("Si vuelve se perderán los cambios. Está seguro?"))
                    {
                        return producto;
                    }
                    continue;
                }

                if (option == 8)
                {
                    if (Confirm("Se guardarán los cambios. Está seguro?"))
                    {
                        return productoUpdated;
                    }
                    continue;
                }

                Console.WriteLine("""
                    
                    Ingrese un nuevo valor o presione ENTER para mantener el valor actual
                    """);

                switch (option)
                {
                    case 1:
                        {
                            Console.Write($"SKU [{productoUpdated.Sku}]: ");
                            var value = Console.ReadLine()?.Trim();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                if (value != productoUpdated.Sku)
                                {
                                    var result = _productoService.ExisteSku(value);
                                    if (!result.IsSuccess)
                                    {
                                        ConsoleHelper.Messages.Error(result.Message);
                                        ConsoleHelper.Messages.Continuar();
                                        break;
                                    }
                                }
                                productoUpdated.Sku = value;
                            }
                            break;
                        }
                    case 2:
                        {
                            Console.Write($"Nombre [{productoUpdated.Nombre}]: ");
                            var value = Console.ReadLine()?.Trim();
                            if (!string.IsNullOrWhiteSpace(value))
                                productoUpdated.Nombre = value;
                            break;
                        }
                    case 3:
                        {
                            Console.Write($"Descripción [{productoUpdated.Descripcion}]: ");
                            var value = Console.ReadLine()?.Trim();
                            if (!string.IsNullOrWhiteSpace(value))
                                productoUpdated.Descripcion = value;
                            break;
                        }
                    case 4:
                        {
                            Console.Write($"Descripción Larga [{productoUpdated.DescripcionLarga}]: ");
                            var value = Console.ReadLine()?.Trim();
                            if (!string.IsNullOrEmpty(value))
                                productoUpdated.DescripcionLarga = value;
                            break;
                        }
                    case 5:
                        {
                            Console.Write($"Precio [{productoUpdated.Precio}]: ");
                            var value = Console.ReadLine()?.Trim();
                            if (decimal.TryParse(value, out var precioVal) && precioVal >= 0)
                            {
                                productoUpdated.Precio = precioVal;
                            }
                            else if (!string.IsNullOrWhiteSpace(value))
                            {
                                ConsoleHelper.Messages.Error("Precio inválido");
                                ConsoleHelper.Messages.Continuar();
                            }
                            break;
                        }
                    case 6:
                        {
                            Console.Write($"Activo para Venta (1: Sí / 0: No) [{(productoUpdated.ActivoParaVenta ? 1 : 0)}]: ");
                            var val = Console.ReadLine()?.Trim();
                            if (int.TryParse(val, out var actVenta) && (actVenta == 0 || actVenta == 1))
                                productoUpdated.ActivoParaVenta = actVenta == 1;
                            break;
                        }
                    case 7:
                        {
                            Console.Write($"Activo general (1: Sí / 0: No) [{(productoUpdated.Activo ? 1 : 0)}]: ");
                            var val = Console.ReadLine()?.Trim();
                            if (int.TryParse(val, out var actGen) && (actGen == 0 || actGen == 1))
                                productoUpdated.Activo = actGen == 1;
                            break;
                        }
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        ConsoleHelper.Messages.Continuar();
                        break;
                }
            }
        }
        private void UpdateStock(Producto producto, bool esIngreso)
        {
            Console.Clear();
            string type = esIngreso
                ? "AGREGAR STOCK"
                : "QUITAR STOCK";
            Console.WriteLine($"""
                {type} - {producto.Nombre}
                =================================
                """);

            Console.Write("Cantidad: ");
            if (!int.TryParse(Console.ReadLine(), out var cantidad) || cantidad <= 0)
            {
                ConsoleHelper.Messages.Error("Cantidad no válida");
                ConsoleHelper.Messages.Continuar();
                return;
            }

            if (!esIngreso)
                cantidad = -cantidad;

            Console.Write("Motivo del movimiento: ");
            var motivo = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(motivo))
            {
                ConsoleHelper.Messages.Error("El motivo es obligatorio");
                ConsoleHelper.Messages.Continuar();
                return;
            }

            var movimientoStock = new MovimientoStock()
            {
                IdProducto = producto.IdProducto,
                IdTipoMovimiento = 1,
                IdEmpleado = _empleado.IdEmpleado,
                TipoStock = "DEPÓSITO",
                Cantidad = cantidad,
                Fecha = DateTime.Now,
                Motivo = motivo

            };
            var result = _movimientoStockService.RegistrarMovimientoStock(movimientoStock);
            if (result.IsSuccess)
                ConsoleHelper.Messages.OK(result.Message);
            else
                ConsoleHelper.Messages.Error(result.Message);
            ConsoleHelper.Messages.Continuar();
        }
        private static bool Confirm(string mensaje)
        {
            while (true)
            {
                Console.Clear();
                Console.Write($"""
                    {mensaje}
                    [1] Sí
                    [2] No
                    Ingrese una opción: 
                    """);

                var input = Console.ReadLine()?.Trim();
                if (!int.TryParse(input, out var option))
                {
                    ConsoleHelper.Messages.OpcionNoValida();
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                switch (option)
                {
                    case 1:
                        return true;
                    case 2:
                        return false;
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        ConsoleHelper.Messages.Continuar();
                        continue;
                }
            }
        }
    }
}
