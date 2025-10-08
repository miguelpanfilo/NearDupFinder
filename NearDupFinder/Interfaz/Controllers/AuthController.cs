using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NearDupFinder_Dominio.Clases;

namespace Interfaz.Controllers;

[Route("auth")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromForm] string email, [FromForm] string clave, [FromServices] Sistema sistema)
    {
        var usuario = sistema.ValidarUsuario(email, clave);
        if (usuario is null) 
            return Redirect("/login?error=1");

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, usuario.Nombre),
            new(ClaimTypes.Email, usuario.Email.ToString()),
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString())
        };
        foreach (var rol in usuario.ObtenerRoles())
            claims.Add(new Claim(ClaimTypes.Role, rol.ToString()));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        return Redirect("/");
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/login");
    }
}