using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorUsuario;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaLogin;
using NearDupFinder_LogicaDeNegocio.Servicios;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFinder_LogicaDeNegocio.Servicios.Usuarios;

namespace NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;


public class GestorUsuarios(AlmacenamientoDeDatos almacenamientoDeDatos, GestorAuditoria gestorAuditoria, GestorAutenticacionUsuario gestorAutenticacionUsuario)
{
    public bool CrearUsuario(DatosRegistroUsuario datosRegistroUsuario)
    {
        Usuario nuevoUsuario;
        Clave claveDelUsuario;
        try
        {
            Email correo = Email.Crear(datosRegistroUsuario.Email);
            if (ElEmailYaEstaRegistrado(correo))
                return false;

            Fecha fecha = Fecha.Crear(datosRegistroUsuario.Anio,
                datosRegistroUsuario.Mes, datosRegistroUsuario.Dia);

            claveDelUsuario = Clave.Crear(datosRegistroUsuario.Clave);

            nuevoUsuario = Usuario.Crear(datosRegistroUsuario.Nombre,
                datosRegistroUsuario.Apellido, correo, fecha);
        }
        catch
        {
            return false;
        }

        CambiarClaveDelUsuarioSiCorresponde(nuevoUsuario, claveDelUsuario);
        AgregarRolesAlUsuario(nuevoUsuario, datosRegistroUsuario.Roles);
        AgregarALaListaDeUsuarios(nuevoUsuario);

        gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, $"Usuario: '{datosRegistroUsuario.Email}'");

        return true;
    }
    
    public bool ModificarUsuario(DatosEdicionUsuario datosEdicionUsuario)
    {
        bool seModificoElUsuario = EditarDatosDelUsuario(datosEdicionUsuario);
        if (seModificoElUsuario)
        {
            gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.EditarUsuario,
                $"Usuario modificado: '{datosEdicionUsuario.EmailActual}'");
        }
        
        return seModificoElUsuario;
    }
    
    public bool BorrarUsuario(string email)
    {
        Email emailUsuario;
        try
        {
            emailUsuario = Email.Crear(email);
        }
        catch
        {
            return false;
        }

        Usuario? usuario = ObtenerUsuarioPorEmail(emailUsuario);
        if (usuario is null)
            return false;

        gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.EliminarUser,
            $"Usuario eliminado: '{email}'");

        RemoverDeLaListaDeUsuarios(usuario);

        return true;
    }
    
    public IReadOnlyList<DatosPublicosUsuario> ObtenerUsuarios()
    {
        List<DatosPublicosUsuario> listaDtoUsuarios = new List<DatosPublicosUsuario>();
        IReadOnlyCollection<Usuario> listaUsuarios = almacenamientoDeDatos.ObtenerUsuarios().AsReadOnly();
        foreach (Usuario usuario in listaUsuarios)
            listaDtoUsuarios.Add(DatosPublicosUsuario.FromEntity(usuario));
        
        return listaDtoUsuarios;
    }
    
    public DatosPublicosUsuario ObtenerUsuarioPorId(int id)
    {
        Usuario? usuario = almacenamientoDeDatos.BuscarUsuarioPorId(id);
        
        if (usuario is null)
            throw new ExcepcionDeUsuario("El usuario no existe");
        
        return DatosPublicosUsuario.FromEntity(usuario);
    }

    public int ObtenerIdDeUsuario(string email)
    {
        Email? emailUsuario;
        try
        {
            emailUsuario = Email.Crear(email);
        }
        catch 
        {
            throw new ExcepcionDeUsuario("El email no es valido");
        }
        
        Usuario? usuario = ObtenerUsuarioPorEmail(emailUsuario);

        if (usuario is null)
            throw new ExcepcionDeUsuario("El usuario no existe");
        
        return usuario.Id;
    }

    public bool UsuarioTieneRol(string email, string rol)
    {
        Email emailUsuario;
        try
        {
            emailUsuario = Email.Crear(email);
        }
        catch
        {
            throw new ExcepcionDeUsuario("El email no es valido");
        }
        Usuario? usuario = ObtenerUsuarioPorEmail(emailUsuario);

        if (usuario is null)
            throw new ExcepcionDeUsuario("El usuario no existe");
        
        Rol rolABuscar;
        if(string.Equals(rol, "Administrador"))
            rolABuscar = Rol.Administrador;
        else
            rolABuscar = Rol.Revisor;
            
        return usuario.TieneRol(rolABuscar);
    }

    public bool ModificarClave(DatosCambioClave datosCambioClave)
    {
        DatosAutenticacion datosAutenticacion = new DatosAutenticacion(datosCambioClave.Email, datosCambioClave.ClaveActual);
        Usuario? usuarioValidado = gestorAutenticacionUsuario.AutenticarUsuario(datosAutenticacion);
        if (usuarioValidado is not null)
        {
            bool claveModificada = ModificarClaveInterno(usuarioValidado, datosCambioClave.ClaveNueva);
            if (claveModificada)
                gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.EditarUsuario,
                    $"Clave modificada para el usuario: '{datosCambioClave.Email}'");
            return claveModificada;
        }

        return false;
    }

    private bool ModificarClaveInterno(Usuario usuario, string? clave)
    {
        Clave nuevaClave;
        try
        {
            nuevaClave = Clave.Crear(clave);
        }
        catch
        {
            return false;
        }

        CambiarClaveDelUsuarioSiCorresponde(usuario, nuevaClave);

        return true;
    }
    
    private void CambiarClaveDelUsuarioSiCorresponde(Usuario usuario, Clave? clave)
    {
        if (clave is not null)
            usuario.CambiarClave(clave);
    }

    private bool ElEmailYaEstaRegistrado(Email email)
    {
        if (almacenamientoDeDatos.BuscarUsuarioPorEmail(email) is not null)
            return true;
        return false;
    }

    private Usuario? ObtenerUsuarioPorEmail(Email email)
    {
        return almacenamientoDeDatos.BuscarUsuarioPorEmail(email);
    }

    private void AgregarRolesAlUsuario(Usuario usuario, IReadOnlyCollection<Rol> roles)
    {
        foreach (var rol in roles)
            usuario.AgregarRol(rol);
    }

    private void AgregarALaListaDeUsuarios(Usuario usuario)
    {
        almacenamientoDeDatos.AgregarUsuario(usuario);
    }

    private void RemoverDeLaListaDeUsuarios(Usuario usuario)
    {
        almacenamientoDeDatos.RemoverUsuario(usuario);
    }
    
    private bool EditarDatosDelUsuario(DatosEdicionUsuario datosEdicionUsuario)
    {
        if (!ValidarNombreYApellido(datosEdicionUsuario.NuevoNombre, datosEdicionUsuario.NuevoApellido))
            return false;

        Usuario? usuarioAModificar;
        Clave? posibleNuevaClave = null;
        Fecha fecha;
        try
        {
            Email correo = Email.Crear(datosEdicionUsuario.EmailActual);
            usuarioAModificar = ObtenerUsuarioPorEmail(correo);
            if (usuarioAModificar is null)
                return false;

            fecha = Fecha.Crear(datosEdicionUsuario.Anio, datosEdicionUsuario.Mes, datosEdicionUsuario.Dia);

            if (!string.IsNullOrWhiteSpace(datosEdicionUsuario.NuevaClave))
                posibleNuevaClave = Clave.Crear(datosEdicionUsuario.NuevaClave);
        }
        catch
        {
            return false;
        }

        ModificarUsuarioExistente(usuarioAModificar, datosEdicionUsuario, fecha);

        CambiarClaveDelUsuarioSiCorresponde(usuarioAModificar, posibleNuevaClave);

        return true;
    }
    
    private bool ValidarNombreYApellido(string? nombre, string? apellido)
    {
        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido))
            return false;
        return true;
    }

    private void ModificarUsuarioExistente(Usuario usuario, DatosEdicionUsuario datosEdicionUsuario, Fecha nuevaFecha)
    {
        usuario.Nombre = datosEdicionUsuario.NuevoNombre!;
        usuario.Apellido = datosEdicionUsuario.NuevoApellido!;
        usuario.FechaNacimiento = nuevaFecha;
        IReadOnlyCollection<Rol> rolesDestino = datosEdicionUsuario.NuevosRoles;
        usuario.ObtenerRoles().Except(rolesDestino).ToList().ForEach(usuario.RemoverRol);
        rolesDestino.Except(usuario.ObtenerRoles()).ToList().ForEach(usuario.AgregarRol);
    }
}