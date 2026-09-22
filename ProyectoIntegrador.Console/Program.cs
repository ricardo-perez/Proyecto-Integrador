using Microsoft.Extensions.Configuration;
using ProyectoIntegrador.Business;
using ProyectoIntegrador.Console.Menus;
using ProyectoIntegrador.DataAccess.Factories;
using ProyectoIntegrador.DataAccess.Implementations.MsSql;
using ProyectoIntegrador.DataAccess.Interfaces;
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
string connectionString = configuration.GetConnectionString("SqlServerConnection")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'SqlServerConnection' en appsettings.json.");
IConnectionFactory connectionFactory = new MsSqlConnectionFactory(connectionString);
IClienteDao clienteDao = new ClienteDaoMsSql(connectionFactory);
var clienteBusiness = new ClienteService(clienteDao);
var clienteMenu = new ClienteMenu(clienteBusiness);
var mainMenu = new MainMenu(clienteMenu);
mainMenu.Show();