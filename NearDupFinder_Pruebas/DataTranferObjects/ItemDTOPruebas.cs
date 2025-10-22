using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs;
/*
namespace NearDupFinder_Pruebas.DataTranferObjects;
[TestClass]
public class ListaItemsTests
{
    private Sistema? _sistema;
    private Catalogo? _catalogo;

    [TestInitialize]
    public void Setup()
    {
        _sistema = new Sistema();
        _catalogo = new Catalogo("Catalogo Test");
        _sistema.AgregarCatalogo(_catalogo);
    }
    [TestMethod]
    public void CrearItemEditDataTransfer_ConPropiedades()
    { 
        var itemEdit = new ItemDto();
        
        itemEdit.Titulo = "Mi Titulo";
        itemEdit.Descripcion = "Mi descripcion";

        Assert.AreEqual("Mi Titulo", itemEdit.Titulo);
        Assert.AreEqual("Mi descripcion", itemEdit.Descripcion);
    }

    [TestMethod]
    public void ItemEditDataTransfer_PuedeAlmacenarTodasLasPropiedades()
    {
        var itemEdit = new ItemDto();

        itemEdit.Id = 10;
        itemEdit.Titulo = "Titulo";
        itemEdit.Descripcion = "Descripcion";
        itemEdit.Categoria = "Categoria1";
        itemEdit.Marca = "MarcaX";
        itemEdit.Modelo = "ModeloY";
        
        Assert.AreEqual(10, itemEdit.Id);
        Assert.AreEqual("Titulo", itemEdit.Titulo);
        Assert.AreEqual("Descripcion", itemEdit.Descripcion);
        Assert.AreEqual("Categoria1", itemEdit.Categoria);
        Assert.AreEqual("MarcaX", itemEdit.Marca);
        Assert.AreEqual("ModeloY", itemEdit.Modelo);
    }
    
    [TestMethod]
    public void ItemEditDataTransfer_ValoresPorDefectoNoSonNull()
    {
        var itemEdit = new ItemDto();

        Assert.IsNotNull(itemEdit.Titulo);
        Assert.IsNotNull(itemEdit.Descripcion);
        Assert.IsNotNull(itemEdit.Categoria);
        Assert.IsNotNull(itemEdit.Marca);
        Assert.IsNotNull(itemEdit.Modelo);
    }
    
    [TestMethod]
    public void ItemEditDataTransfer_ValidarIgualdades()
    {
        var item = new Item
        { 
            Titulo = "Laptop",
            Descripcion = "15 pulgadas",
            Categoria = "Computadoras",
            Marca = "HP",
            Modelo = "Pavilion"
        };

        var dto = new ItemDto
        {
            Id = item.Id,
            Titulo = item.Titulo,
            Descripcion = item.Descripcion,
            Categoria = item.Categoria,
            Marca = item.Marca,
            Modelo = item.Modelo
        };

        Assert.AreEqual(item.Id, dto.Id);
        Assert.AreEqual(item.Titulo, dto.Titulo);
        Assert.AreEqual(item.Descripcion, dto.Descripcion);
        Assert.AreEqual(item.Categoria, dto.Categoria);
        Assert.AreEqual(item.Marca, dto.Marca);
        Assert.AreEqual(item.Modelo, dto.Modelo);
    }
    
    [TestMethod]
    public void ItemEditDataTransfer_CambiosNoModificanItemOriginal()
    {
        var item = new Item
        {
            
            Titulo = "Original",
            Descripcion = "Desc",
            Categoria = "Cat",
            Marca = "M",
            Modelo = "Mod"
        };

        var dto = new ItemDto
        {
            Id = item.Id,
            Titulo = item.Titulo,
            Descripcion = item.Descripcion,
            Categoria = item.Categoria,
            Marca = item.Marca,
            Modelo = item.Modelo
        };
        
        dto.Titulo = "Modificado";
        dto.Descripcion = "Nueva Desc";
        
        Assert.AreEqual("Original", item.Titulo);
        Assert.AreEqual("Desc", item.Descripcion);
    }
    
    [TestMethod]
    public void EliminarItem_EliminaItemDelCatalogo()
    {
        var item1 = new Item("Item 1", "Desc 1");
        var item2 = new Item("Item 2", "Desc 2");

        _catalogo.AgregarItem(item1);
        _catalogo.AgregarItem(item2);

        var dto = ItemDto.FromEntity(item1);
        _sistema.EliminarItem("Catalogo Test", dto);

        Assert.IsFalse(_catalogo.Items.Contains(item1), "Item1 debe ser eliminado del catálogo");
        Assert.IsTrue(_catalogo.Items.Contains(item2), "Item2 debe permanecer en el catálogo");
    }
    
    [TestMethod]
    public void FromEntity_CreaDtoCorrectamente()
    {
        var item = new Item("Título prueba", "Descripción prueba", "Marca1", "Modelo1", "Cat1");
        var dto = ItemDto.FromEntity(item);

        Assert.AreEqual(item.Id, dto.Id);
        Assert.AreEqual(item.Titulo, dto.Titulo);
        Assert.AreEqual(item.Descripcion, dto.Descripcion);
        Assert.AreEqual(item.Categoria, dto.Categoria);
        Assert.AreEqual(item.Marca, dto.Marca);
        Assert.AreEqual(item.Modelo, dto.Modelo);
    }

    [TestMethod]
    public void FromEntity_ItemConCamposNull_CopiaCorrectamente()
    {
        var item = new Item("Titulo", "Descripcion"); 
        var dto = ItemDto.FromEntity(item);

        Assert.AreEqual(item.Id, dto.Id);
        Assert.AreEqual(item.Titulo, dto.Titulo);
        Assert.AreEqual(item.Descripcion, dto.Descripcion);
        Assert.AreEqual(item.Marca, dto.Marca);
        Assert.AreEqual(item.Modelo, dto.Modelo);
        Assert.AreEqual(item.Categoria, dto.Categoria);

    }
    [TestMethod]
    public void EliminarItemYActualizarDuplicados_EliminaDuplicadosGlobalesDelItem()
    {
        var item1 = new Item("Titulo", "Descripcion");
        var item2 = new Item("Titulo", "Descripcion");
        var item3 = new Item("Otro", "Desc");

        _sistema.AltaItemConAltaDuplicados("Catalogo Test", item1);
        _sistema.AltaItemConAltaDuplicados("Catalogo Test", item2);
        _sistema.AltaItemConAltaDuplicados("Catalogo Test", item3);

        var dto = ItemDto.FromEntity(item1);
        _sistema.EliminarItem("Catalogo Test", dto);

        Assert.IsFalse(item2.EstadoDuplicado);
    }

    [TestMethod]
    [ExpectedException(typeof(ExcepcionItem))]
    public void EliminarItem_ItemNoExistente_NoLanzaExcepcion()
    {
        var item = new Item("ItemInexistente", "Desc");
        var dto = ItemDto.FromEntity(item);
        _sistema.EliminarItem("Catalogo Test", dto);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarItem_CatalogoNoExistente_LanzaExcepcion()
    {
        var item = new Item("ItemInexistente", "Desc");
        var dto = ItemDto.FromEntity(item);
        _sistema.EliminarItem("Catalogo Inexistente", dto);
    }

}
*/