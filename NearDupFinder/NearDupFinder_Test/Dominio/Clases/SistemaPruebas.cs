using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Test.Dominio;

[TestClass]
public class SistemaPruebas
{
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
        public void ActualizarItemEnCatalogo_ModificaTituloYDescripcion()
        {
            var sistema = new Sistema();
            var catalogo = new Catalogo("Catálogo Test");
            var item = new Item("Original", "Descripcion original")
            {
                Categoria = "Cat 1",
                Marca = "Marca 1",
                Modelo = "Modelo 1"
            };
            catalogo.AgregarItem(item);

            var dto = new ItemEditDataTransfer
            {
                Id = item.Id,
                Titulo = "Nuevo Título",
                Descripcion = "Nueva Descripción",
                Categoria = "Cat 1",
                Marca = "Marca 1",
                Modelo = "Modelo 1"
            };

            sistema.ActualizarItemEnCatalogo(catalogo, dto);

            Assert.AreEqual("Nuevo Título", item.Titulo);
            Assert.AreEqual("Nueva Descripción", item.Descripcion);
        }
        
        [TestMethod]
        public void ActualizarItemEnCatalogo_ModificaCategoriaMarcaModelo()
        {
            
            var sistema = new Sistema();
            var catalogo = new Catalogo("Catálogo Test");
            var item = new Item("Original", "Descripcion original")
            {
                Categoria = "Cat 1",
                Marca = "Marca 1",
                Modelo = "Modelo 1"
            };
            catalogo.AgregarItem(item);

            var dto = new ItemEditDataTransfer
            {
                Id = item.Id,
                Titulo = "Original",
                Descripcion = "Descripcion original",
                Categoria = "Cat 2",
                Marca = "Marca 2",
                Modelo = "Modelo 2"
            };

            sistema.ActualizarItemEnCatalogo(catalogo, dto);

            
            Assert.AreEqual("Cat 2", item.Categoria);
            Assert.AreEqual("Marca 2", item.Marca);
            Assert.AreEqual("Modelo 2", item.Modelo);
        }
        [TestMethod]
        public void ActualizarItemEnCatalogo_ItemNoExiste_Excepcion()
        {
            
            var sistema = new Sistema();
            var catalogo = new Catalogo("Catálogo Test");

            var dto = new ItemEditDataTransfer
            {
                Id = 999, // Id inexistente
                Titulo = "Título",
                Descripcion = "Descripcion",
                Categoria = "Cat",
                Marca = "Marca",
                Modelo = "Modelo"
            };

            var ex = Assert.ThrowsException<ItemException>(
                () => sistema.ActualizarItemEnCatalogo(catalogo, dto)
            );

            Assert.AreEqual("No se encontró el item a actualizar.", ex.Message);
        }
        [TestMethod]
        public void AltaItem_AgregaItemAlCatalogo()
        {
            var sistema = new Sistema();
            var catalogo = new Catalogo("Catálogo Test");
            sistema.AgregarCatalogo(catalogo);

            var nuevoItem = new Item("Item 1", "Descripción 1");

            sistema.AltaItem("Catálogo Test", nuevoItem);
            var items = catalogo.Items;

            Assert.AreEqual(1, items.Count);
            Assert.AreEqual("Item 1", items.First().Titulo);
            Assert.AreEqual("Descripción 1", items.First().Descripcion);
        }
        [TestMethod]
        [ExpectedException(typeof(ItemException))]
        public void AltaItem_SinCatalogo_Excepcion()
        {
            
            var sistema = new Sistema();
            var nuevoItem = new Item("Item 1", "Desc");

            sistema.AltaItem("Inexistente", nuevoItem);

        }

        [TestMethod]
        [ExpectedException(typeof(ItemException))]
        public void AltaItem_LanzaExcepcionSiTituloOVacio()
        {
            var sistema = new Sistema();
            var catalogo = new Catalogo("Catálogo Test");
            sistema.AgregarCatalogo(catalogo);

            var nuevoItem = new Item("", "Descripción 1"); 
            sistema.AltaItem("Catálogo Test", nuevoItem);
        }


        [TestMethod]
        [ExpectedException(typeof(ItemException))]
        public void AltaItem_DescripcionVacia_Excepcion()
        {
            var sistema = new Sistema();
            var catalogo = new Catalogo("Catálogo Test");
            sistema.AgregarCatalogo(catalogo);

            var nuevoItem = new Item("Titulo", ""); 
            sistema.AltaItem("Catálogo Test", nuevoItem);
        }
        
        
        
        
        

    }

    
