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
    public int Id { get; set;}
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public Email Email { get; set; }
    public Fecha FechaNacimiento { get; set; }

    private HashSet<Rol> Roles { get; set; }
    
    private Clave? Clave { get; set; }
    
    private Usuario(string nombre, string apellido, Email email, Fecha fechaNacimiento)
    {
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
        FechaNacimiento = fechaNacimiento;
        Clave = new Clave();
        Id = _nextId++;
        Roles = new HashSet<Rol>();
    }
    
    public static Usuario Crear(string? nombre, string? apellido, Email email, Fecha fechaNacimiento)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ExcepcionDeUsuario("El nombre no puede estar vacío.");
        if (string.IsNullOrWhiteSpace(apellido))
            throw new ExcepcionDeUsuario("El apellido no puede estar vacío.");

        return new Usuario(nombre.Trim(), apellido.Trim(), email, fechaNacimiento);
    }

    public void AgregarRol(Rol rol)
    {
        Roles.Add(rol);
    }
    
    public void RemoverRol(Rol rol)
    {
        Roles.Remove(rol);
    }

    public bool TieneRol(Rol rol)
    {
        return Roles.Contains(rol);
    }

    public IReadOnlyCollection<Rol> ObtenerRoles()
    {
        return Roles.ToList().AsReadOnly();
    }
    
    public bool Igual(Usuario otroUsuario)
    {
        return Email.Igual(otroUsuario.Email);
    }
    
    public bool VerificarClave(string? clave)
    {
        if(string.IsNullOrEmpty(clave))
            return false;
        return Clave!.Verificar(clave);
    }
    
    public bool CambiarClave(Clave nuevaClave)
    {
        Clave = nuevaClave; 
        return true;
    }
}