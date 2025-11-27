using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MenteSana_web.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// ===========================
// 🔧 CONFIGURACIÓN DE SERVICIOS
// ===========================

// Razor Pages (si las usas)
builder.Services.AddRazorPages();

// Controladores con vistas (MVC + API)
builder.Services.AddControllersWithViews();

// 👉 REGISTRO DEL DbContext (EF Core)
builder.Services.AddDbContext<MenteSanaDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MenteSanaConnection")
    )
);

// Sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ===========================
// 🚀 PIPELINE
// ===========================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Sesiones antes de autorización
app.UseSession();
app.UseAuthorization();

// ===========================
// 🌐 RUTAS
// ===========================

// ⚠️ Importante: habilitar rutas de controladores API + MVC
app.MapControllers();

// Redirección raíz → /Acceso/Login
app.MapGet("/", context =>
{
    context.Response.Redirect("/Acceso/Login");
    return Task.CompletedTask;
});

// Ruta por defecto MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Login}/{id?}"
);

// Razor Pages si las usas
app.MapRazorPages();

app.Run();
