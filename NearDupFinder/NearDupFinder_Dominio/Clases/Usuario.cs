using System.Collections;

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
    
    private Contrasena _contrasena;
    
    private Usuario(string nombre, string apellido, Email email, Fecha fechaNacimiento)
    {
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
        FechaNacimiento = fechaNacimiento;
        _contrasena = new Contrasena();
        Id = _nextId++;
    }
    
    public static Usuario Crear(string? nombre, string? apellido, Email email, Fecha fechaNacimiento)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));
        if (string.IsNullOrWhiteSpace(apellido))
            throw new ArgumentException("El apellido no puede estar vacío.", nameof(apellido));
        if (email is null)
            throw new ArgumentNullException(nameof(email));
        if (fechaNacimiento is null)
            throw new ArgumentNullException(nameof(fechaNacimiento));

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
    
    public bool Igual(Usuario? otroUsuario)
    {
        if (otroUsuario is null) 
            return false;

        return this.Email.Igual(otroUsuario.Email);
    }
    
    public bool CambiarContrasena(Contrasena? nuevaContra)
    {
        if (nuevaContra is null)
            return false;
        
        this._contrasena = nuevaContra; 
        return true;
    }

    public bool VerificarContrasena(string? contra)
    {
        return _contrasena.Verificar(contra);
    }
}