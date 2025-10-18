using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NearDupFinder_LogicaDeNegocio;
using NearDupFinder_LogicaDeNegocio.Recursos;
using NearDupFinder_LogicaDeNegocio.Servicios;

namespace NearDupFinder_Interfaz.Controladores;

[Route("auth")]
public class ControladorDeAutenticacion : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromForm] string email, [FromForm] string clave, [FromServices] Login login)
    {
        DatosAutenticacion datosAutenticacion = new(email, clave);
        var usuario = login.AutenticarUsuario(datosAutenticacion);
        if (usuario is null)
            return Redirect("/login?error=1");

        //login.AsignarUsuarioActual(usuario.Email.ToString());  por ahora no funciona hasta desacoplarlo de sistema

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
    public async Task<IActionResult> Logout([FromServices] Sistema sistema)
    {
        sistema.DesasignarUsuario();

        await HttpContext.SignOutAsync();
        return Redirect("/login");
    }
}