using NearDupFinder_Dominio.Clases;
using NearDupFInder_LogicaDeNegocio.DTOs.ParaGestorItems;

namespace NearDupFinder_Pruebas.DataTranferObjects;

[TestClass]
public class ListaItemsTests
{
    private Catalogo? _catalogo;

    [TestInitialize]
    public void Setup()
    {
        _catalogo = new Catalogo("Catálogo Test");
    }

    [TestMethod]
    public void CrearItemDataListaItems_ConPropiedadesAsignadas_DeberiaMantenerValores()
    {
        var itemData = new DatosItemListaItems
        {
            Titulo = "Mi Título",
            Descripcion = "Mi Descripción",
            Categoria = "Categoría A",
            Marca = "Marca X",
            Modelo = "Modelo 123",
            EstadoDuplicado = true
        };

        Assert.AreEqual("Mi Título", itemData.Titulo);
        Assert.AreEqual("Mi Descripción", itemData.Descripcion);
        Assert.AreEqual("Categoría A", itemData.Categoria);
        Assert.AreEqual("Marca X", itemData.Marca);
        Assert.AreEqual("Modelo 123", itemData.Modelo);
        Assert.IsTrue(itemData.EstadoDuplicado);
    }

    [TestMethod]
    public void FromEntity_DeberiaMapearCorrectamenteDesdeItem()
    {
        var item = new Item("Título Original", "Descripción Original")
        {
            Categoria = "Cat B",
            Marca = "Marca Y",
            Modelo = "Modelo Z"
        };

        var copiaDatosDesdeItem = DatosItemListaItems.FromEntity(item);

        Assert.AreEqual(item.Id, copiaDatosDesdeItem.Id);
        Assert.AreEqual("Título Original", copiaDatosDesdeItem.Titulo);
        Assert.AreEqual("Descripción Original", copiaDatosDesdeItem.Descripcion);
        Assert.AreEqual("Cat B", copiaDatosDesdeItem.Categoria);
        Assert.AreEqual("Marca Y", copiaDatosDesdeItem.Marca);
        Assert.AreEqual("Modelo Z", copiaDatosDesdeItem.Modelo);
    }


    [TestMethod]
    public void ItemEditDataTransfer_ValoresPorDefectoNoSonNull()
    {
        var itemEdit = new DatosItemListaItems();

        Assert.IsNotNull(itemEdit.Titulo);
        Assert.IsNotNull(itemEdit.Descripcion);
        Assert.IsNotNull(itemEdit.Categoria);
        Assert.IsNotNull(itemEdit.Marca);
        Assert.IsNotNull(itemEdit.Modelo);
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

        var dto = new DatosItemListaItems
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
    public void FromEntity_CreaDtoCorrectamente()
    {
        var item = new Item
        {
            Titulo = "Titulo",
            Descripcion = "Descripcion",
            Marca = "Marca1",
            Modelo = "Modelo1",
            Categoria = "Cat1"
        };

        var dto = DatosItemListaItems.FromEntity(item);

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

        var dto = DatosItemListaItems.FromEntity(item);
        
        Assert.AreEqual(item.Id, dto.Id);
        Assert.AreEqual(item.Titulo, dto.Titulo);
        Assert.AreEqual(item.Descripcion, dto.Descripcion);

        Assert.AreEqual(string.Empty, dto.Marca);
        Assert.AreEqual(string.Empty, dto.Modelo);
        Assert.AreEqual(string.Empty, dto.Categoria);
    }




}
