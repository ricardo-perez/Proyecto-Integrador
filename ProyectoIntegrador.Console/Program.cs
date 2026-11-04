using Microsoft.Extensions.Configuration;
using ProyectoIntegrador.Business;
using ProyectoIntegrador.Console.Menus;
using ProyectoIntegrador.Console.Menus.Cliente;
using ProyectoIntegrador.Console.Menus.Empleado;
using ProyectoIntegrador.DataAccess.Factories;
using ProyectoIntegrador.DataAccess.Implementations.MsSql;
using ProyectoIntegrador.DataAccess.Interfaces;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
string connectionString = configuration.GetConnectionString("SqlServerConnection")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'SqlServerConnection' en appsettings.json");

IConnectionFactory connectionFactory = new MsSqlConnectionFactory(connectionString);
IClienteDao clienteDao = new ClienteDaoMsSql(connectionFactory);
IProductoDao productoDao = new ProductoDaoMsSql(connectionFactory);
IMedioDePagoDao medioDePagoDao = new MedioDePagoDaoMsSql(connectionFactory);
IVentaDao ventaDao = new VentaDaoMsSql(connectionFactory);
IDetalleVentaDao detalleVentaDao = new DetalleVentaDaoMsSql(connectionFactory);
IMovimientoStockDao movimientoStockDao = new MovimientoStockDaoMsSql(connectionFactory);

var clienteSerive = new ClienteService(clienteDao);
var productoService = new ProductoService(productoDao);
var medioDePagoService = new MedioDePagoService(medioDePagoDao);
var ventaService = new VentaService(ventaDao, detalleVentaDao, movimientoStockDao);

var clienteMenu = new ClienteMainMenu(clienteSerive, productoService, medioDePagoService, ventaService);
var empleadoMenu = new EmpleadoMainMenu();
var mainMenu = new MainMenu(clienteMenu, empleadoMenu);

mainMenu.Show();