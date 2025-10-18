using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.Recursos;

namespace NearDupFinder_LogicaDeNegocio.Servicios;

public class GestorUsuarios(Sistema sistema, AlmacenamientoDeDatos almacenamientoDeDatos)
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

        sistema.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, $"Usuario: '{sistema.UsuarioActual}'");
        ;
        return true;
    }

    public IReadOnlyList<Usuario> ObtenerUsuarios()
    {
        return almacenamientoDeDatos.ObtenerUsuarios().AsReadOnly();
    }


    public bool BorrarUsuario(DatosUsuarioEmail datosUsuarioEmail)
    {
        Email emailUsuario;
        try
        {
            emailUsuario = Email.Crear(datosUsuarioEmail.Email);
        }
        catch (ExcepcionDeUsuario ex)
        {
            throw ex;
        }

        Usuario? usuario = BuscarUsuarioPorEmail(emailUsuario);
        if (usuario is null)
            return false;

        sistema.RegistrarLog(EntradaDeLog.AccionLog.EliminarUser, $"Usuario eliminado: '{sistema.UsuarioActual}'");

        RemoverDeLaListaDeUsuarios(usuario);

        return true;
    }

    public bool ModificarClave(DatosCambioClave datosCambioClave)
    {
        Usuario? usuarioValidado = AutenticarUsuario(datosCambioClave.Email, datosCambioClave.ClaveActual);
        if (usuarioValidado is not null)
        {
            bool claveModificada = ModificarClave(usuarioValidado, datosCambioClave.ClaveNueva);
            if (claveModificada)
                sistema.RegistrarLog(EntradaDeLog.AccionLog.EditarUsuario,
                    $"Clave modificada para el usuario: '{sistema.UsuarioActual}'");
            return claveModificada;
        }

        return false;
    }

    public bool ModificarClave(Usuario usuario, string? clave)
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

        Usuario? usuario = BuscarUsuarioPorEmail(emailAValidar);
        if (usuario is null)
            return null;

        if (usuario.VerificarClave(clave))
            return usuario;

        return null;
    }

    private bool ElEmailYaEstaRegistrado(Email email)
    {
        if (almacenamientoDeDatos.BuscarUsuarioPorEmail(email) is not null)
            return true;
        return false;
    }

    public Usuario? BuscarUsuarioPorEmail(Email email)
    {
        return almacenamientoDeDatos.BuscarUsuarioPorEmail(email);
    }

    public Usuario? BuscarUsuarioPorId(int id)
    {
        return almacenamientoDeDatos.BuscarUsuarioPorId(id);
    }

    public bool UsuarioTieneRol(Usuario usuario, Rol rol)
    {
        return usuario.TieneRol(rol);
    }

    public IReadOnlyCollection<Rol> ObtenerRolesDeUsuario(Usuario usuario)
    {
        return usuario.ObtenerRoles();
    }

    private void CambiarClaveDelUsuarioSiCorresponde(Usuario usuario, Clave? clave)
    {
        if (clave is not null)
            usuario.CambiarClave(clave);
    }

    private void AgregarRolesAlUsuario(Usuario usuario, IReadOnlyCollection<Rol>? roles)
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

    private bool ValidarNombreYApellido(string? nombre, string? apellido)
    {
        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido))
            return false;
        return true;
    }

    public bool EditarDatosDelUsuario(DatosEdicionUsuario datosEdicionUsuario)
    {
        if (!ValidarNombreYApellido(datosEdicionUsuario.NuevoNombre, datosEdicionUsuario.NuevoApellido))
            return false;

        Usuario? usuarioAModificar;
        Clave? posibleNuevaClave = null;
        Fecha fecha;
        try
        {
            Email correo = Email.Crear(datosEdicionUsuario.EmailActual);
            usuarioAModificar = BuscarUsuarioPorEmail(correo);
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

        ModificarUsuarioExistente(usuarioAModificar, datosEdicionUsuario
            , fecha);

        CambiarClaveDelUsuarioSiCorresponde(usuarioAModificar, posibleNuevaClave);

        return true;
    }

    public bool ModificarUsuario(DatosEdicionUsuario datosEdicionUsuario)
    {
        bool pasaModificarUsuario = EditarDatosDelUsuario(datosEdicionUsuario);
        if (pasaModificarUsuario)
        {
        }

        sistema.RegistrarLog(EntradaDeLog.AccionLog.EditarUsuario, $"Usuario modificado: '{sistema.UsuarioActual}'");
        return pasaModificarUsuario;
    }

    private void ModificarUsuarioExistente(Usuario usuario, DatosEdicionUsuario datosEdicionUsuario, Fecha nuevaFecha)
    {
        usuario.Nombre = datosEdicionUsuario.NuevoNombre!;
        usuario.Apellido = datosEdicionUsuario.NuevoApellido!;
        usuario.FechaNacimiento = nuevaFecha;
        var rolesDestino = datosEdicionUsuario.NuevosRoles ?? Array.Empty<Rol>();
        usuario.ObtenerRoles().Except(rolesDestino).ToList().ForEach(usuario.RemoverRol);
        rolesDestino.Except(usuario.ObtenerRoles()).ToList().ForEach(usuario.AgregarRol);
    }
}