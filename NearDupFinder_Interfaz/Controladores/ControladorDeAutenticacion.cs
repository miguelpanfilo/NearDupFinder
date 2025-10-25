using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaLogin;
using NearDupFinder_LogicaDeNegocio.Servicios;
using NearDupFinder_LogicaDeNegocio.Servicios.Usuarios;

namespace NearDupFinder_Interfaz.Controladores;

[Route("auth")]
public class ControladorDeAutenticacion : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromForm] string email, [FromForm] string clave, [FromServices] GestorAutenticacionUsuario gestorAutenticacionUsuario,
        [FromServices] GestorInicializacion gestorInicializacion,[FromServices] GestorAuditoria gestorAuditoria)
    {
        gestorInicializacion.AsegurarInicializacion();
        
        DatosAutenticacion datosAutenticacion = new DatosAutenticacion(email, clave);
        bool elUsuarioExiste = gestorAutenticacionUsuario.AutenticarUsuarioBooleano(datosAutenticacion);
        if (!elUsuarioExiste)
            return Redirect("/login?error=1");
        
        DatosIdentificacion usuario = gestorAutenticacionUsuario.AutenticarUsuarioDto(datosAutenticacion);

        gestorAuditoria.AsignarUsuarioActual(usuario.Email); 

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, usuario.Nombre),
            new(ClaimTypes.Email, usuario.Email),
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString())
        };
        foreach (string rol in usuario.Roles)
            claims.Add(new Claim(ClaimTypes.Role, rol));

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