using NearDupFinder_Dominio;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio;

[TestClass]
public class SistemaPruebas
{
    [TestInitialize]
    public void Init()
    {
        Item.ResetIdCounter();
    }
    
    [TestMethod]
    public void AutenticarUsuario_Correto_RetornaUsuario()
    {
        Sistema sistema = new Sistema();
        string emailAdmin = "admin@gmail.com";
        string claveAdmin = "123QWEasdzxc@";
        
        Usuario? admin = sistema.AutenticarUsuario(emailAdmin, claveAdmin);
        
        Assert.IsNotNull(admin);
    }
    
    [TestMethod]
    public void AutenticoUsuario_Incorrecto_RetornaNulo()
    {
        Sistema sistema = new Sistema();
        string emailIncorrecto = "incorrecto@gmail.com";
        string claveIncorrecta= "mal";
        
        Usuario? admin = sistema.AutenticarUsuario(emailIncorrecto, claveIncorrecta);
        
        Assert.IsNull(admin);
    }
    
    [TestMethod]
    public void AutenticoUsuario_EmailIncorrecto_RetornaNulo()
    {
        Sistema sistema = new Sistema();
        string emailIncorrecto = "incorrecto@gmail.com";
        string claveAdmin = "123QWEasdzxc@";
        
        Usuario? admin = sistema.AutenticarUsuario(emailIncorrecto, claveAdmin);
        
        Assert.IsNull(admin);
    }
    
    [TestMethod]
    public void AutenticoUsuario_ClaveIncorrecta_RetornaNulo()
    {
        Sistema sistema = new Sistema();
        string emailAdmin = "admin@gmail.com";
        string claveIncorrecta= "mal";
        
        Usuario? admin = sistema.AutenticarUsuario(emailAdmin, claveIncorrecta);
        
        Assert.IsNull(admin);
    }
    
    [TestMethod]
    public void AutenticoUsuario_EmailNulo_RetornaNulo()
    {
        Sistema sistema = new Sistema();
        string? emailNulo = null;
        string claveAdmin = "123QWEasdzxc@";
        
        Usuario? admin = sistema.AutenticarUsuario(emailNulo, claveAdmin);
        
        Assert.IsNull(admin);
    }
    
    [TestMethod]
    public void AutenticoUsuario_ClaveNula_RetornaNulo()
    {
        Sistema sistema = new Sistema();
        string emailAdmin = "admin@gmail.com";
        string? claveNula= null;
        
        Usuario? admin = sistema.AutenticarUsuario(emailAdmin, claveNula);
        
        Assert.IsNull(admin);
    }
    
    
    
    [TestMethod]
    public void Sistema_Inicio_Vacio()
    {
        Sistema sistema = new Sistema();

        
        var items = sistema.ObtenerItems();

        
        Assert.AreEqual(0, items.Count, "El sistema debería iniciar sin items.");
    }
    [TestMethod]
    public void AgregarItem_Valido_Ok()
    {
        
        Sistema sistema = new Sistema();
        Item item = new Item("Titulo", "Descripcion", "Marca", "Modelo", "Categoria");

        
        sistema.AgregarItem(item);

        
        var items = sistema.ObtenerItems();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual("Titulo", items[0].Titulo);
    }


    [TestMethod]
    public void EditarTitulo_CambiaSoloTitulo()
    {
        
        Item item = new Item("Titulo original", "Descripcion", "Marca", "Modelo", "Categoria");

        
        item.EditarTitulo("Nuevo titulo");

        Assert.AreEqual("Nuevo titulo", item.Titulo);
    }
    [TestMethod]
    public void EditarDescripcion_CambiaSoloDescripcion()
    {
        Item item = new Item("Titulo", "Descripcion original", "Marca", "Modelo", "Categoria");

        item.EditarDescripcion("Nueva descripcion");

        Assert.AreEqual("Nueva descripcion", item.Descripcion);
    }

    [TestMethod]
    public void EditarMarca_CambiaSoloMarca()
    {
        Item item = new Item("Titulo", "Descripcion", "Marca original", "Modelo", "Categoria");

        item.EditarMarca("Nueva marca");

        Assert.AreEqual("Nueva marca", item.Marca);
    }

    [TestMethod]
    public void EditarModelo_CambiaSoloModelo()
    {
        Item item = new Item("Titulo", "Descripcion", "Marca", "Modelo original", "Categoria");

        item.EditarModelo("Nuevo modelo");

        Assert.AreEqual("Nuevo modelo", item.Modelo);
    }

    [TestMethod]
    public void EditarCategoria_CambiaSoloCategoria()
    {
        Item item = new Item("Titulo", "Descripcion", "Marca", "Modelo", "Categoria original");

        item.EditarCategoria("Nueva categoria");

        Assert.AreEqual("Nueva categoria", item.Categoria);
    }

    [TestMethod]
    public void AgregarItemACatalogo_ItemValido_ItemAgregado()
    {
        
        var sistema = new Sistema();
            
        
        var catalogo = new Catalogo("Titulo");
        sistema.AgregarCatalogo(catalogo);

        var item = new Item
        {
            Titulo = "Item Test",
            Descripcion = "Descripción de prueba",
            Categoria = "Categoria1"
        };

        sistema.AgregarItemACatalogo(item, catalogo);

        CollectionAssert.Contains(catalogo.ObtenerItems(), item, "El item no fue agregado al catálogo");
    }
    
    
    
}