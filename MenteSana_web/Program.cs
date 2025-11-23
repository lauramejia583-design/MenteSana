using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ===========================
// 🔧 CONFIGURACIÓN DE SERVICIOS
// ===========================

// Razor Pages (por si usas alguna)
builder.Services.AddRazorPages();

// Controladores con vistas (MVC)
builder.Services.AddControllersWithViews();

// Servicios para manejar sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de sesión
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ===========================
// 🚀 CONFIGURACIÓN DEL PIPELINE
// ===========================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilitar sesión antes de autorización
app.UseSession();
app.UseAuthorization();

// ===========================
// 🌐 CONFIGURACIÓN DE RUTAS
// ===========================

// 🔸 Redirección raíz → /Acceso/Login
app.MapGet("/", context =>
{
    context.Response.Redirect("/Acceso/Login");
    return Task.CompletedTask;
});

// 🔸 Ruta por defecto del patrón MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Login}/{id?}");

// 🔸 Si usas Razor Pages también
app.MapRazorPages();

// ===========================
app.Run();