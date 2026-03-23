// ─── IMPORTS ───────────────────────────────────────────────────
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MvcPracticaCubosFinal.Data;
using MvcPracticaCubosFinal.Policies;
using MvcPracticaCubosFinal.Repositories;
using System;

var builder = WebApplication.CreateBuilder(args);

// ─── SERVICIOS BASE ────────────────────────────────────────────
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<CuboRepository>();
builder.Services.AddTransient<UsuarioRepository>();
builder.Services.AddTransient<CompraRepository>();

builder.Services.AddScoped<IAuthorizationHandler, HasComprasHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, HasFavoritosHandler>();
builder.Services.AddAuthorization(options =>
{

    options.AddPolicy("HasCompras", policy =>
        policy.AddRequirements(new HasComprasRequirement()));
    options.AddPolicy("HasFavoritos", policy =>
       policy.AddRequirements(new HasFavoritosRequirement()));
});

// ─── BASE DE DATOS ─────────────────────────────────────────────
string connectionString = builder.Configuration.GetConnectionString("SqlCubos");
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));

// ─── CACHÉ ─────────────────────────────────────────────────────
// Datos globales compartidos (catálogos, listas que no cambian mucho)
builder.Services.AddMemoryCache();

// ─── SESIÓN ────────────────────────────────────────────────────
// Datos temporales por usuario (carrito, nombre, etc.)
builder.Services.AddDistributedMemoryCache(); // almacén base requerido por Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ─── SEGURIDAD (añadido) ───────────────────────────────────────
builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme =
        CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme =
        CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
        CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Managed/Login";
    options.AccessDeniedPath = "/Managed/Denied";
});


var app = builder.Build();

// ─── PIPELINE ──────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();          // 1. Session — entre UseRouting y UseAuthentication
app.UseAuthentication();  // 2. Authentication (añadido)

app.UseAuthorization();   // 3. Authorization — siempre al final

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
