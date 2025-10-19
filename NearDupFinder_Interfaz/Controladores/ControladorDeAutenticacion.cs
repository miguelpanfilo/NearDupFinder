using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NearDupFinder_LogicaDeNegocio.DTOs;
using NearDupFinder_LogicaDeNegocio.Servicios;

namespace NearDupFinder_Interfaz.Controladores;

[Route("auth")]
public class ControladorDeAutenticacion : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromForm] string email, [FromForm] string clave, [FromServices] Login login,
        [FromServices] GestorInicializacion gestorInicializacion,[FromServices] GestorAuditoria gestorAuditoria)
    {
        gestorInicializacion.AsegurarInicializacion();
        
        var usuario = login.AutenticarUsuario(new DatosAutenticacion(email, clave));
        if (usuario is null)
            return Redirect("/login?error=1");

        gestorAuditoria.AsignarUsuarioActual(usuario.Email.ToString()); 

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
    public async Task<IActionResult> Logout([FromServices] GestorAuditoria gestorAuditoria)
    {
        gestorAuditoria.DesasignarUsuario();

        await HttpContext.SignOutAsync();
        return Redirect("/login");
    }
}