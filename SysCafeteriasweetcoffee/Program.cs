using Microsoft.EntityFrameworkCore;
using SysCafeteriasweetcoffee.Models;
using SysCafeteriasweetcoffee.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cadena de conexión y agregar el contexto de la base de datos
builder.Services.AddDbContext<BDContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar los servicios necesarios
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<UsuarioService>();

// Agregar soporte de memoria distribuida para sesiones
builder.Services.AddDistributedMemoryCache();

// Configurar las opciones de la sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Ajusta el tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true;                 // Asegura que solo se pueda acceder a la cookie desde HTTP
    options.Cookie.IsEssential = true;              // Marca la cookie como esencial
});

 builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login"; // Página de inicio de sesión
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Tiempo de expiración de la cookie
        options.AccessDeniedPath = "/Usuario/AccessDenied"; // Página de acceso denegado
    });


var app = builder.Build();

// Configurar el middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseRouting();

// Habilitar las sesiones y autenticación
app.UseSession();
app.UseAuthentication(); // Debe ir antes de UseAuthorization
app.UseAuthorization();

// Configurar las rutas de los controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}");

app.Run();
