using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio.DeteccionDeDuplicados;

[TestClass]
public class JaccardPruebas
{
    private readonly Sistema _sis = new Sistema(); 

    [TestMethod]
    public void CalcularNumTokensUnion_AmbosValidos_NoVacios()
    {
        string[] a = ["a", "b", "b" ]; 
        string[] b = [ "b", "c" ];
        
        int numTokens = _sis.CalcularNumTokensUnion(a, b);
        
        Assert.AreEqual(3, numTokens);
    }
    
    [TestMethod]
    public void CalcularNumTokensUnion_AmbosValidos_UnoVacio()
    {
        string[] a = ["a", "b", "b" ]; 
        string[] b = [];

        int numTokens = _sis.CalcularNumTokensUnion(a, b);

        Assert.AreEqual(2, numTokens);
    }
    
    [TestMethod]
    public void CalcularNumTokensUnion_AmbosValidos_AmbosVacios_RetornaCero()
    {
        string[] a = [];
        string[] b = [];

        int numTokens = _sis.CalcularNumTokensUnion(a, b);

        Assert.AreEqual(0, numTokens);
    }
    
    [TestMethod]
    public void CalcularNumTokensUnion_UnoInvalido_LanzaArgumentNullException()
    {
        string[]? a = null;
        string[] b = ["x"];
        
        Assert.ThrowsException<ArgumentNullException>(() => _sis.CalcularNumTokensUnion(a!, b));
    }
    
    [TestMethod]
    public void CalcularNumTokensUnion_AmbosInvalidos_LanzaArgumentNullException()
    {
        string[]? a = null;
        string[]? b = null;

        Assert.ThrowsException<ArgumentNullException>(() => _sis.CalcularNumTokensUnion(a!, b));
    }
    
    [TestMethod]
    public void CalcularNumTokensInterseccion_AmbosValidos_NoVacios()
    {
        string[] a = ["a", "b", "b"];
        string[] b = ["b", "c", "b"];

        int numTokens = _sis.CalcularNumTokensInterseccion(a, b);

        Assert.AreEqual(1, numTokens);
    }
    
    [TestMethod]
    public void CalcularNumTokensInterseccion_AmbosValidos_UnoVacio_RetornaCero()
    {
        string[] a = ["a", "b"];
        string[] b = [];

        int numTokens = _sis.CalcularNumTokensInterseccion(a, b);
        
        Assert.AreEqual(0, numTokens);
    }
    
    [TestMethod]
    public void CalcularNumTokensInterseccion_AmbosValidos_AmbosVacios_RetornaCero()
    {
        string[] a = [];
        string[] b = [];

        int numTokens = _sis.CalcularNumTokensInterseccion(a, b);

        Assert.AreEqual(0, numTokens);
    }
    
    [TestMethod]
    public void CalcularNumTokensInterseccion_UnoInvalido_LanzaArgumentNullException()
    {
        string[]? a = null;
        string[] b =["a"];
    
        Assert.ThrowsException<ArgumentNullException>(() => _sis.CalcularNumTokensInterseccion(a!, b));
    }
    
    
    [TestMethod]
    public void CalcularNumTokensInterseccion_AmbosInvalidos_RetornaMenosUno()
    {
        string[]? a = null;
        string[]? b = null;

        int numTokens = _sis.CalcularNumTokensInterseccion(a, b);

        Assert.AreEqual(-1, numTokens);
    }
    
    [TestMethod]
    public void CalcularJaccard_AmbosValidos_NoVacios()
    {
        string[] a = ["a", "b"];
        string[] b = ["b", "c"];

        float valorJaccard = _sis.CalcularJaccard(a, b);

        Assert.AreEqual(1f/3f, valorJaccard);
    }
    
    [TestMethod]
    public void CalcularJaccard_AmbosValidos_UnoVacios_RetornaCero()
    {
        string[] a = ["x", "y"];
        string[] b = [];

        float valorJaccard = _sis.CalcularJaccard(a, b);

        Assert.AreEqual(0f, valorJaccard);
    }
    
    [TestMethod]
    public void CalcularJaccard_AmbosValidos_AmbosVacios_RetornaCero()
    {
        string[] a = [];
        string[] b = [];

        float valorJaccard = _sis.CalcularJaccard(a, b);

        Assert.AreEqual(0f, valorJaccard);
    }
    
    [TestMethod]
    public void CalcularJaccard_UnoInvalido_RetornaMenosUno()
    {
        string[]? a = null;
        string[]  b = ["x"];

        float valor = _sis.CalcularJaccard(a, b);

        Assert.AreEqual(-1f, valor);
    }
    
    [TestMethod]
    public void CalcularJaccard_AmbosInvalidos_RetornaMenosUno()
    {
        string[]? a = null;
        string[]? b = null;

        float valor = _sis.CalcularJaccard(a, b);

        Assert.AreEqual(-1f, valor);
    }
}