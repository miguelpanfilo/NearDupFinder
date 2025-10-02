using Interfaz.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using NearDupFinder_Dominio.Clases;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Sistema>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Si alguien intenta entrar a una página [Authorize] y no está logueado, lo manda acá.
        options.LoginPath = "/login"; 
        // Si usás una acción de “logout” por ruta, esta es la URL convencional.
        options.LogoutPath = "/logout";
    });

// Habilita [Authorize], roles, políticas, y <AuthorizeView> en la UI.
builder.Services.AddAuthorization(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// lee/escribe la cookie y establece HttpContext.User
app.UseAuthentication(); 
// aplica las reglas [Authorize]
app.UseAuthorization();

// POST /login: procesa formulario HTML, emite cookie y redirige
app.MapPost("/auth/login", async (HttpContext http, Sistema sistemaTemp) =>
    {
        // 1) Leer campos enviados por <form>
        var form = await http.Request.ReadFormAsync();
        var email = form["Email"].ToString();
        var clave = form["Password"].ToString();

        // 2) Validación
        var usuario = sistemaTemp.AutenticarUsuario(email, clave);
        if (usuario is null)
            // credenciales inválidas → marcar error y volver al login
            return Results.Redirect("/login?error=1");


        // 3) Crear identidad y firmar sesión → el server emite la COOKIE
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario!.Nombre),
            new Claim(ClaimTypes.Email, usuario.Email.ToString()),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
        };
        // Un claim por cada rol
        foreach (var rol in usuario.ObtenerRoles())
        {
            claims.Add(new Claim(ClaimTypes.Role, rol.ToString())); 
        }
        var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await http.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        // 4) éxito → redirigimos al home ya autenticados
        return Results.Redirect("/");
    }).AllowAnonymous();

// POST /logout: borra la cookie de autenticación y redirige
app.MapPost("/logout", async (HttpContext http) =>
    {
        // elimina la cookie de auth
        await http.SignOutAsync();
        // te lleva a la home
        return Results.Redirect("/login");
    }).AllowAnonymous();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();