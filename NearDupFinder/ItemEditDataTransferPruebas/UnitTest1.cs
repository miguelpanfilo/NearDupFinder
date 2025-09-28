using Interfaz.Components.Pages;
using NearDupFinder_Dominio.Clases;

namespace ItemEditDataTransferPruebas;
[TestClass]
public class ListaCRUDTests
{
    [TestMethod]
    public void CrearItemEditDataTransfer_ConPropiedades()
    { 
        var itemEdit = new ItemEditDataTransfer();

        
        itemEdit.Titulo = "Mi Titulo";
        itemEdit.Descripcion = "Mi descripcion";

        Assert.AreEqual("Mi Titulo", itemEdit.Titulo);
        Assert.AreEqual("Mi descripcion", itemEdit.Descripcion);
    }

    [TestMethod]
    public void ItemEditDataTransfer_PuedeAlmacenarTodasLasPropiedades()
    {
        var itemEdit = new ItemEditDataTransfer();

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
        var itemEdit = new ItemEditDataTransfer();

        Assert.IsNotNull(itemEdit.Titulo);
        Assert.IsNotNull(itemEdit.Descripcion);
        Assert.IsNotNull(itemEdit.Categoria);
        Assert.IsNotNull(itemEdit.Marca);
        Assert.IsNotNull(itemEdit.Modelo);
    }

}
