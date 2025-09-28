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
}
