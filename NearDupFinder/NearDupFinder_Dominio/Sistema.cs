using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Dominio;

public class Sistema
{
    private readonly List<Usuario> _usuarios = [];
    private List<Catalogo> _catalogos;

    public Sistema()
    {
        _usuarios.Add(CrearUsuarioAdmin());
        _catalogos = new List<Catalogo>();
    }

    private Usuario CrearUsuarioAdmin()
    {
        Email email = Email.Crear("admin@gmail.com");
        Fecha fecha = Fecha.Crear(1997,12,27);
        Usuario adminUsuario = Usuario.Crear("admin","admin",email,fecha);
        adminUsuario.AgregarRol(Rol.Administrador);
        Contrasena contrasena = Contrasena.Crear("123QWEasdzxc@");
        adminUsuario.CambiarContrasena(contrasena);
        
        return adminUsuario;
    }

    public Usuario? AutenticarUsuario(string? email, string? clave)
    {
        Email emailAValidar;
        try
        {
            emailAValidar = Email.Crear(email);
        }
        catch
        {
            return null;
        }

        foreach (var usuario in _usuarios)
        {
            if (usuario.Email.Igual(emailAValidar) && usuario.VerificarContrasena(clave))
                return usuario;
        }
        return null;
    }
  

  

    public void AgregarCatalogo(Catalogo catalogo)
    {
        if (catalogo == null) throw new ArgumentNullException(nameof(catalogo));
        _catalogos.Add(catalogo);
    }

    // Función que agrega un Item a un Catalogo específico
    public void AgregarItemACatalogo(Item item, Catalogo catalogo)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (catalogo == null) throw new ArgumentNullException(nameof(catalogo));

        catalogo.AgregarItem(item);
    }
}