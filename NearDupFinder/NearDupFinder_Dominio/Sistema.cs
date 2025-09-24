using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Dominio;

public class Sistema
{
    private readonly List<Usuario> _usuarios = [];
    private List<Catalogo> _catalogos;
    
    private List<Item> _items = new List<Item>();

  

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
   
    
    public List<Item> ObtenerItemsDeCatalogo(Catalogo catalogo)
    {
        if (catalogo == null) throw new ArgumentNullException(nameof(catalogo));
        return catalogo.ObtenerItems(); // usa la función del catálogo
    }
    
    public List<Item> ObtenerTodosLosItems()
    {
        return _catalogos.SelectMany(c => c.ObtenerItems()).ToList();
    }
    public List<Catalogo> ObtenerCatalogos()
    {
        return _catalogos;
    }
    public void AgregarItem(Item item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        _items.Add(item);
    }

    public List<Item> ObtenerItems()
    {
        return _items;
    }
    public void ActualizarItem(Item itemActualizado, int id)
    {
        if (itemActualizado == null)
            throw new ArgumentNullException(nameof(itemActualizado));

        // Buscar el item original por Id en la lista de sistema
        var original = _items.FirstOrDefault(i => i.Id == id);
        if (original == null)
            throw new InvalidOperationException("No se encontró el item a actualizar.");

        // Actualizar propiedades usando los métodos de edición existentes
        original.EditarTitulo(itemActualizado.Titulo);
        original.EditarDescripcion(itemActualizado.Descripcion);
        original.EditarMarca(itemActualizado.Marca);
        original.EditarModelo(itemActualizado.Modelo);
        original.EditarCategoria(itemActualizado.Categoria);
    }


}