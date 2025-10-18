using NearDupFinder_Interfaz.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using NearDupFinder_Almacenamiento;
using NearDupFinder_LogicaDeNegocio;
using NearDupFinder_LogicaDeNegocio.Servicios;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<AlmacenamientoDeDatos>();
builder.Services.AddScoped<Sistema>();
builder.Services.AddScoped<Login>();
builder.Services.AddScoped<GestorUsuarios>();
builder.Services.AddControllers();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login"; 
        options.LogoutPath = "/logout";
    });

builder.Services.AddAuthorization(); 

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication(); 
app.UseAuthorization();

app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();