using NearDupFinder_Almacenamiento;
using NearDupFinder_Almacenamiento.Repositorios;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Pruebas.Utilidades;

namespace NearDupFinder_Pruebas.Repositorios;

[TestClass]
public class RepositorioItemsPruebas
{
    private SqlContext _context = null!;
    private RepositorioItems _repo = null!;

    [TestInitialize]
    public void Setup()
    {
        var opciones = SqlContextFactoryPruebas.CrearOpcionesInMemory("BD_Items");
        _context = SqlContextFactoryPruebas.CrearContexto(opciones);
        SqlContextFactoryPruebas.LimpiarBaseDeDatos(_context);

        _repo = new RepositorioItems(_context);
    }

    [TestMethod]
    public void OrfanearPorCatalogo_QuitaClusterIdSoloDelCatalogoIndicado()
    {
        var cat1 = new Catalogo("Cat1");
        var cat2 = new Catalogo("Cat2");

        _context.Add(cat1);
        _context.Add(cat2);
        _context.SaveChanges();

        var i1 = Item.Crear("A", "d");
        var i2 = Item.Crear("B", "d");
        var i3 = Item.Crear("C", "d");

        _context.AddRange(i1, i2, i3);

        _context.Entry(i1).Property("CatalogoId").CurrentValue = cat1.Id;
        _context.Entry(i2).Property("CatalogoId").CurrentValue = cat1.Id;
        _context.Entry(i3).Property("CatalogoId").CurrentValue = cat2.Id;

        _context.Entry(i1).Property("ClusterId").CurrentValue = 1;
        _context.Entry(i2).Property("ClusterId").CurrentValue = 2;
        _context.Entry(i3).Property("ClusterId").CurrentValue = 3;

        _context.SaveChanges();

        _repo.OrfanearPorCatalogo(cat1.Id);

        var a1 = _context.Items.First(i => i.Id == i1.Id);
        var a2 = _context.Items.First(i => i.Id == i2.Id);
        var a3 = _context.Items.First(i => i.Id == i3.Id);

        Assert.IsNull(_context.Entry(a1).Property("ClusterId").CurrentValue);
        Assert.IsNull(_context.Entry(a2).Property("ClusterId").CurrentValue);
        Assert.AreEqual(3, _context.Entry(a3).Property("ClusterId").CurrentValue);
    }

    [TestMethod]
    public void OrfanearPorCatalogo_NoAfectaItemsSinCluster()
    {
        var cat = new Catalogo("Cat");

        _context.Add(cat);
        _context.SaveChanges();

        var i1 = Item.Crear("A", "d");
        var i2 = Item.Crear("B", "d");

        _context.AddRange(i1, i2);

        _context.Entry(i1).Property("CatalogoId").CurrentValue = cat.Id;
        _context.Entry(i2).Property("CatalogoId").CurrentValue = cat.Id;

        _context.Entry(i1).Property("ClusterId").CurrentValue = null;
        _context.Entry(i2).Property("ClusterId").CurrentValue = null;

        _context.SaveChanges();

        _repo.OrfanearPorCatalogo(cat.Id);

        var a1 = _context.Items.First(i => i.Id == i1.Id);
        var a2 = _context.Items.First(i => i.Id == i2.Id);

        Assert.IsNull(_context.Entry(a1).Property("ClusterId").CurrentValue);
        Assert.IsNull(_context.Entry(a2).Property("ClusterId").CurrentValue);
    }

    [TestMethod]
    public void OrfanearPorCatalogo_NoAfectaItemsDeOtrosCatalogos()
    {
        var cat1 = new Catalogo("Cat1");
        var cat2 = new Catalogo("Cat2");

        _context.Add(cat1);
        _context.Add(cat2);
        _context.SaveChanges();

        var i1 = Item.Crear("A", "d");
        var i2 = Item.Crear("B", "d");

        _context.AddRange(i1, i2);

        _context.Entry(i1).Property("CatalogoId").CurrentValue = cat1.Id;
        _context.Entry(i2).Property("CatalogoId").CurrentValue = cat2.Id;

        _context.Entry(i1).Property("ClusterId").CurrentValue = 10;
        _context.Entry(i2).Property("ClusterId").CurrentValue = 20;

        _context.SaveChanges();

        _repo.OrfanearPorCatalogo(cat1.Id);

        var a1 = _context.Items.First(i => i.Id == i1.Id);
        var a2 = _context.Items.First(i => i.Id == i2.Id);

        Assert.IsNull(_context.Entry(a1).Property("ClusterId").CurrentValue);
        Assert.AreEqual(20, _context.Entry(a2).Property("ClusterId").CurrentValue);
    }
}