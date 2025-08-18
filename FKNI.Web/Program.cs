using FKNI.Application.Config;
using FKNI.Application.Profiles;
using FKNI.Application.Services.Implementations;
using FKNI.Application.Services.Interfaces;
using FKNI.Infraestructure.Data;
using FKNI.Infraestructure.Repository.Implementations;
using FKNI.Infraestructure.Repository.Interfaces;
using FKNI.Web.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Mapeo de la clase AppConfig para leer appsettings.json
builder.Services.Configure<AppConfig>(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();
//***********************
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Ruta donde redirige si no está autenticado
        options.AccessDeniedPath = "/Login/Forbidden"; // Ruta si no tiene permisos
    });

builder.Services.AddAuthorization();

//Registra repositorios, Servicios y profiles

// Configurar D.I.
//Repository
builder.Services.AddTransient<IRepositoryUsuarios, RepositoryUsuarios>();
builder.Services.AddTransient<IRepositoryProductos, RepositoryProductos>();
builder.Services.AddTransient<IRepositoryCategorias, RepositoryCategorias>();
builder.Services.AddTransient<IRepositoryImagenes, RepositoryImagenes>();
builder.Services.AddTransient<IRepositoryEtiquetas, RepositoryEtiquetas>();
builder.Services.AddTransient<IRepositoryResenas, RepositoryResenas>();
builder.Services.AddTransient<IRepositoryPromociones, RepositoryPromociones>();
builder.Services.AddTransient<IRepositoryPedidos, RepositoryPedidos>();
//Services
builder.Services.AddTransient<IServiceUsuarios, ServiceUsuarios>();
builder.Services.AddTransient<IServiceProductos, ServiceProductos>();
builder.Services.AddTransient<IServiceCategorias, ServiceCategorias>();
builder.Services.AddTransient<IServiceImagenes, ServiceImagenes>();
builder.Services.AddTransient<IServiceEtiquetas, ServiceEtiquetas>();
builder.Services.AddTransient<IServiceResenas, ServiceResenas>();
builder.Services.AddTransient<IServicePromociones, ServicePromociones>();
builder.Services.AddTransient<IServicePedidos, ServicePedidos>();
//Configurar Automapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<UsuariosProfile>();
    config.AddProfile<ProductosProfile>();
    config.AddProfile<CategoriasProfile>();
    config.AddProfile<ImagenesProfile>();
    config.AddProfile<EtiquetasProfile>();
    config.AddProfile<ResenasProfile>();
    config.AddProfile<PromocionesProfile>();
    config.AddProfile<PedidosProfile>();
});
// Configuar Conexión a la Base de Datos SQL
builder.Services.AddDbContext<FKNIContext>(options =>
{
    // it read appsettings.json file
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase"));
    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

//Configuración Serilog
// Logger. P.E. Verbose = muestra SQl Statement
var logger = new LoggerConfiguration()
// Limitar la información de depuración
.MinimumLevel.Override("Microsoft", LogEventLevel.Error)
.Enrich.FromLogContext()
// Log LogEventLevel.Verbose muestra mucha información, pero no es necesaria
//solo para el proceso de depuración
 .WriteTo.Console(LogEventLevel.Information)
.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level ==
LogEventLevel.Information).WriteTo.File(@"Logs\Info-.log", shared: true, encoding:
Encoding.ASCII, rollingInterval: RollingInterval.Day))
 .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level ==
LogEventLevel.Debug).WriteTo.File(@"Logs\Debug-.log", shared: true, encoding:
System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
 .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level ==
LogEventLevel.Warning).WriteTo.File(@"Logs\Warning-.log", shared: true, encoding:
System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
 .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level ==
LogEventLevel.Error).WriteTo.File(@"Logs\Error-.log", shared: true, encoding: Encoding.ASCII,
rollingInterval: RollingInterval.Day))
 .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level ==
LogEventLevel.Fatal).WriteTo.File(@"Logs\Fatal-.log", shared: true, encoding: Encoding.ASCII,
rollingInterval: RollingInterval.Day))
 .CreateLogger();
builder.Host.UseSerilog(logger);
//***************************

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // Error control Middleware
    app.UseMiddleware<ErrorHandlingMiddleware>();
}

//Activar soporte a la solicitud de registro con SERILOG
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

// Activar Antiforgery
app.UseAntiforgery();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
