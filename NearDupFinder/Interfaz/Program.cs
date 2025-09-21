using Interfaz.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;


var builder = WebApplication.CreateBuilder(args);

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
app.MapPost("/auth/login", async (HttpContext http) =>
    {
        // 1) Leer campos enviados por <form>
        var form = await http.Request.ReadFormAsync();
        var usuario = form["Username"].ToString();
        var clave = form["Password"].ToString();

        // 2) Validación
        var ok = usuario == "admin" && clave == "123";
        if (!ok)
        {
            // fallo → volvemos a /login
            return Results.Redirect("/login");
        }

        // 3) Crear identidad y firmar sesión → el server emite la COOKIE
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario),
            new Claim(ClaimTypes.Role, "Administrator"),
        };
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
        return Results.Redirect("/");
    }).AllowAnonymous();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();