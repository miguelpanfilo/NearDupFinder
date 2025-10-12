using NearDupFinder_Dominio.Clases;
using NearDupFInder_LogicaDeNegocio.DTO;

namespace NearDupFinder_Pruebas.DataTranferObjects;
[TestClass]
public class ListaItemsTests
{
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
}
