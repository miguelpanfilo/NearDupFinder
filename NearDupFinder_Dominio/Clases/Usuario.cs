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
    public int Id { get;  set; }
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public Email Email { get; set; } = null!;
    public Fecha FechaNacimiento { get; set; } = null!;
    public Clave Clave { get; set; } = null!;
  
    public ICollection<Rol> Roles { get; private set; } = new List<Rol>();
    
    public ICollection<RolPersistido> RolesPersistidos { get; private set; } = new List<RolPersistido>();

    private Usuario() {} 
    
    public static Usuario Crear(string? nombre, string? apellido, Email email, Fecha fechaNacimiento)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ExcepcionDeUsuario("El nombre no puede estar vacío.");
        if (string.IsNullOrWhiteSpace(apellido))
            throw new ExcepcionDeUsuario("El apellido no puede estar vacío.");
        
        Usuario nuevoUsuario = new Usuario();
        nuevoUsuario.Id = _nextId++;
        nuevoUsuario.Nombre = nombre.Trim();
        nuevoUsuario.Apellido = apellido.Trim();
        nuevoUsuario.Email = email;
        nuevoUsuario.FechaNacimiento = fechaNacimiento;
        nuevoUsuario.Clave = new Clave();

        return nuevoUsuario;
    }

    public void AgregarRol(Rol rol)
    {
        if (!Roles.Contains(rol)) 
            Roles.Add(rol);
        
        String rolTexto = rol.ToString();
        if (!RolesPersistidos.Any(rolPersistido => string.Equals(rolPersistido.Valor, rolTexto, StringComparison.OrdinalIgnoreCase)))
            RolesPersistidos.Add(new RolPersistido(rolTexto));
    }

    public void RemoverRol(Rol rol)
    {
        if (Roles.Contains(rol)) 
            Roles.Remove(rol);
        
        String rolTexto = rol.ToString();
        RolPersistido? rolPersistido = RolesPersistidos.FirstOrDefault(rPersistido => string.Equals(rPersistido.Valor, rolTexto, StringComparison.OrdinalIgnoreCase));
        if (rolPersistido != null) 
            RolesPersistidos.Remove(rolPersistido);
    }
    public bool TieneRol(Rol rol) => Roles.Contains(rol);

    public IReadOnlyCollection<Rol> ObtenerRoles()
    {
        return Roles.ToList().AsReadOnly();
    }
    
    public void SincronizarRolesDesdePersistencia()
    {
        Roles.Clear();

        if (this.RolesPersistidos.Count == 0)
            return;

        foreach (RolPersistido rolPersistido in this.RolesPersistidos)
        {
            string valorPersistido = rolPersistido.Valor;
            if (string.IsNullOrWhiteSpace(valorPersistido))
                continue;

            Rol rolConvertido;
            bool sePudoConvertir = Enum.TryParse<Rol>(valorPersistido, true, out rolConvertido);
            if (!sePudoConvertir)
                continue;
            
            bool esUnValorValidoDelEnum = Enum.IsDefined(typeof(Rol), rolConvertido);
            if (!esUnValorValidoDelEnum)
                continue;

            bool yaExisteEnLaColeccion = Roles.Contains(rolConvertido);
            if (!yaExisteEnLaColeccion)
                this.Roles.Add(rolConvertido);
        }
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