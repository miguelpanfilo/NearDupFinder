using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Dominio.Clases;

public enum Rol
{
    Administrador,
    Revisor
}

public class Usuario
{
    private static int _nextId = 1;
    public int Id { get; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public Email Email { get; }
    public Fecha FechaNacimiento { get; set; }

    private readonly HashSet<Rol> _roles = new();
    
    private Clave? _clave;
    
    private Usuario(string nombre, string apellido, Email email, Fecha fechaNacimiento)
    {
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
        FechaNacimiento = fechaNacimiento;
        _clave = new Clave();
        Id = _nextId++;
    }
    
    public static Usuario Crear(string? nombre, string? apellido, Email email, Fecha fechaNacimiento)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new UsuarioException("El nombre no puede estar vacío.");
        if (string.IsNullOrWhiteSpace(apellido))
            throw new UsuarioException("El apellido no puede estar vacío.");

        return new Usuario(nombre.Trim(), apellido.Trim(), email, fechaNacimiento);
    }

    public void AgregarRol(Rol rol)
    {
        _roles.Add(rol);
    }
    
    public void RemoverRol(Rol rol)
    {
        _roles.Remove(rol);
    }

    public bool TieneRol(Rol rol)
    {
        return _roles.Contains(rol);
    }

    public IReadOnlyCollection<Rol> ObtenerRoles()
    {
        return _roles.ToList().AsReadOnly();
    }
    
    public bool Igual(Usuario otroUsuario)
    {
        return Email.Igual(otroUsuario.Email);
    }
    public bool VerificarClave(string? clave)
    {
        if(string.IsNullOrEmpty(clave))
            return false;
        return _clave!.Verificar(clave);
    }
    
    public bool CambiarClave(Clave nuevaClave)
    {
        _clave = nuevaClave; 
        return true;
    }
}