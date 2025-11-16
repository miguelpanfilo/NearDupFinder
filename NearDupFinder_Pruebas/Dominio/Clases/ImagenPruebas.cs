using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Pruebas.Dominio.Clases;

[TestClass]
public class ImagenPruebas
{
    [TestMethod]
    public void CrearDesdeBase64_Base64Valido_Ok()
    {
        byte[] bytes = new byte[] { 1, 2, 3, 4 };
        string base64 = Convert.ToBase64String(bytes);

        var imagen = Imagen.CrearDesdeBase64(base64);

        Assert.AreEqual(base64, imagen.Base64);
    }

    [TestMethod]
    [ExpectedException(typeof(ExcepcionItem))]
    public void CrearDesdeBase64_Base64Invalido_LanzaExcepcion()
    {
        string base64Invalido = "esto no es base64";

        Imagen.CrearDesdeBase64(base64Invalido);
    }

    [TestMethod]
    [ExpectedException(typeof(ExcepcionItem))]
    public void CrearDesdeBase64_MayorAlMaximo_LanzaExcepcion()
    {
        int tamaño = 1024 * 1024 + 1;
        byte[] bytes = new byte[tamaño];
        string base64 = Convert.ToBase64String(bytes);

        Imagen.CrearDesdeBase64(base64);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ExcepcionItem))]
    public void CrearDesdeBase64_CadenaVacia_LanzaExcepcion()
    {
        string base64Vacio = "";

        Imagen.CrearDesdeBase64(base64Vacio);
    }

    [TestMethod]
    [ExpectedException(typeof(ExcepcionItem))]
    public void CrearDesdeBase64_SoloEspacios_LanzaExcepcion()
    {
        string base64Espacios = "   ";

        Imagen.CrearDesdeBase64(base64Espacios);
    }

    [TestMethod]
    [ExpectedException(typeof(ExcepcionItem))]
    public void CrearDesdeBase64_Null_LanzaExcepcion()
    {
        // Act
        Imagen.CrearDesdeBase64(null!);
    }
}