using Microsoft.EntityFrameworkCore;
using SysCafeteriasweetcoffee.Models;
using SysCafeteriasweetcoffee.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BDContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));






// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<UsuarioService>();

builder.Services.AddDbContext<BDContext>();

// **Agrega el soporte de memoria distribuida para sesiones**
builder.Services.AddDistributedMemoryCache();

// **Configura las opciones de la sesión**
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Ajusta el tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true;                 // Asegura que solo se pueda acceder a la cookie desde HTTP
    options.Cookie.IsEssential = true;              // Hace que la cookie sea esencial
});

var app = builder.Build();
//Configurar el middleware
if (!app.Environment.IsDevelopment())
{
    //app.UseDatabaseErrorPage();
    app.UseDeveloperExceptionPage();
}

else
{
    app.UseExceptionHandler("/Home/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
