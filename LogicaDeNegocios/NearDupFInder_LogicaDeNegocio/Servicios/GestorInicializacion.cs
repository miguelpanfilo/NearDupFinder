using NearDupFinder_Dominio.Clases;
using NearDupFinder_Interfaces;

namespace NearDupFinder_LogicaDeNegocio.Servicios;

public class GestorInicializacion
{
    private readonly IRepositorioUsuarios _repositorioUsuarios;
    private bool _inicializado;
    private const string EmailAdmin = "admin@gmail.com";

    public GestorInicializacion(IRepositorioUsuarios repositorioUsuarios)
    {
        _repositorioUsuarios = repositorioUsuarios;
    }

    public void AsegurarInicializacion()
    {
        if (_inicializado)
            return;

        if(_repositorioUsuarios.ObtenerUsuarioPorEmail(EmailAdmin) is null)
            CrearUsuarioAdmin();

        _inicializado = true;
    }
    
    public void CrearUsuarioAdmin()
    {
        Email email = Email.Crear(EmailAdmin);
        Fecha fecha = Fecha.Crear(1997,12,27);
        Clave clave = Clave.Crear("123QWEasdzxc@");
        Usuario adminUsuario = Usuario.Crear("admin","admin",email,fecha);
        adminUsuario.AgregarRol(Rol.Administrador);
        adminUsuario.CambiarClave(clave);
        _repositorioUsuarios.Agregar(adminUsuario);
    }
}