using System.Security.Principal;
using Gastroworld.Data;
using GastroWorld.Models;
using GastroWorld.Models.IModel;
using GastroWorld.Models.Repositories;
using GastroWorld.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

//ConexionBd
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAuthentication(); 
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();


// 🔹 Agregar conexión a la base de datos MySQL antes de `builder.Build()`
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 25)) // Ajusta la versión de MySQL según corresponda
    ));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
//builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
//builder.Services.AddScoped<ITipoEspecieRepository, TipoEspecieRepository>();
//builder.Services.AddScoped<IEstadoCitaRepository, EstadoCitaRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "api",
    pattern: "api/{controller=Registro}/{action=Register}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pgprincipal}/{action=cargarView}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Ajustes}/{action=Ajustes}/{id?}");
app.Run();
