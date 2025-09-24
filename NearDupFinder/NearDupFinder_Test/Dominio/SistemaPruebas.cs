using NearDupFinder_Dominio;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;

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
        // 1) Crear sistema
        Sistema sistema = new Sistema();

        // 2) Crear un catálogo vacío
        Catalogo catalogo = new Catalogo("CatalogoPrueba");

        // 3) Agregar el catálogo al sistema
        sistema.AgregarCatalogo(catalogo);

        // 4) Obtener items del catálogo
        var items = sistema.ObtenerItemsDeCatalogo(catalogo);

        // 5) Validar que está vacío
        Assert.AreEqual(0, items.Count, "El sistema debería iniciar sin items en el catálogo.");
    }

    [TestMethod]
    public void AgregarItem_Valido_Ok()
    {
        // Arrange
        Sistema sistema = new Sistema();
        Item item = new Item("Titulo", "Descripcion", "Marca", "Modelo", "Categoria");
        Catalogo catalogo = new Catalogo("Titulo");
        // Act
        sistema.AgregarItemACatalogo(item, catalogo);

        // Assert
        var items = sistema.ObtenerItemsDeCatalogo(catalogo);
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

        CollectionAssert.Contains(catalogo.ObtenerItems(), item, "El item fue agregado al catálogo");
    }
    
    //---------------------------------------------------------------------------------------------------------------------------------
    //Pruebas de actualizar item
    
    [TestMethod]
    public void ActualizarItem_ActualizaPropiedadesCorrectamente()
    {
        // Arrange
        var sistema = new Sistema();
        var itemOriginal = new Item("Titulo1", "Descripcion1");
        sistema.AgregarItem(itemOriginal);

        // Crear DTO con los valores nuevos
        var itemActualizado = new ItemEditDataTransfer()
        {
            Id = itemOriginal.Id,          // Muy importante mantener el Id
            Titulo = "NuevoTitulo",
            Descripcion = "NuevaDescripcion",
            Marca = "NuevaMarca",
            Modelo = "NuevoModelo",
            Categoria = "NuevaCategoria"
        };

        // Act
        sistema.ActualizarItem(itemActualizado);

        // Assert
        var itemEnSistema = sistema.ObtenerItems().First(i => i.Id == itemOriginal.Id);
        Assert.AreEqual("NuevoTitulo", itemEnSistema.Titulo);
        Assert.AreEqual("NuevaDescripcion", itemEnSistema.Descripcion);
        Assert.AreEqual("NuevaMarca", itemEnSistema.Marca);
        Assert.AreEqual("NuevoModelo", itemEnSistema.Modelo);
        Assert.AreEqual("NuevaCategoria", itemEnSistema.Categoria);
    }

    
    
    [TestMethod]
    public void ActualizarItem_ConItemEditDto_CambiaPropiedadesCorrectamente()
    {
        
        var _sistema = new Sistema();

        // Arrange: creamos un item original
        var itemOriginal = new Item("Titulo1", "Desc1", "Marca1", "Modelo1", "Cat1");
        _sistema.AgregarItem(itemOriginal);

        // Creamos data transfer con cambios ( item temporal que va a tener todos los datos nuevos del item que se quizo editar, a excepecion del id )

        var dto = new ItemEditDataTransfer()
        {
            Id = itemOriginal.Id,
            Titulo = "TituloNuevo",
            Descripcion = "DescNueva",
            Marca = "MarcaNueva",
            Modelo = "ModeloNuevo",
            Categoria = "CatNueva"
        };

        // Act
        _sistema.ActualizarItem(dto);

        // Assert
        var itemActualizado = _sistema.ObtenerItems().Find(i => i.Id == itemOriginal.Id);
        Assert.AreEqual("TituloNuevo", itemActualizado.Titulo);
        Assert.AreEqual("DescNueva", itemActualizado.Descripcion);
        Assert.AreEqual("MarcaNueva", itemActualizado.Marca);
        Assert.AreEqual("ModeloNuevo", itemActualizado.Modelo);
        Assert.AreEqual("CatNueva", itemActualizado.Categoria);
    }
    
    [TestMethod]
    public void ActualizarItem_IdNoExistente_LanzaExcepcionConMensajeCorrecto()
    {
        var sistema = new Sistema();

        var dto = new ItemEditDataTransfer()
        {
            Id = 999,  // Id que no existe en el sistema
            Titulo = "Titulo",
            Descripcion = "Descripcion"
        };

        
        var ex = Assert.ThrowsException<InvalidOperationException>(() => sistema.ActualizarItem(dto));

        Assert.AreEqual("No se encontró el item a actualizar.", ex.Message);
    }
    
    [TestMethod]
    public void ActualizarItem_DtoNulo_LanzaArgumentNullException()
    {
        var sistema = new Sistema();

        var ex = Assert.ThrowsException<ArgumentNullException>(() => sistema.ActualizarItem(null));
        Assert.AreEqual("itemTranferido", ex.ParamName);
    }
    [TestMethod]
    public void ActualizarItem_SoloTitulo_DescripcionSeMantiene()
    {
        // Arrange
        var sistema = new Sistema();
        var itemOriginal = new Item("Titulo Original", "Descripcion Original");
        sistema.AgregarItem(itemOriginal);

        var dto = new ItemEditDataTransfer()
        {
            Id = itemOriginal.Id,
            Titulo = "Nuevo Titulo", // Solo cambio el título
            Descripcion = itemOriginal.Descripcion // Mantengo la descripción original
        };

        // Act
        sistema.ActualizarItem(dto);

        // Assert
        Assert.AreEqual("Nuevo Titulo", itemOriginal.Titulo);
        Assert.AreEqual("Descripcion Original", itemOriginal.Descripcion);
    }



    // No se hace prueba de itemTransferido.Titulo || itemTransferido.Descripcion si son vacios, porque ya se 
    // esta validando en las pruebas de Items. 
    
    //---------------------------------------------------------------------------------------------------------------------------------

    
}