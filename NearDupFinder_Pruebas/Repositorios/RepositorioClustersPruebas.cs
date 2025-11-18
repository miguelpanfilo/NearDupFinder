using NearDupFinder_Almacenamiento;
using NearDupFinder_Almacenamiento.Repositorios;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Pruebas.Utilidades;

namespace NearDupFinder_Pruebas.Repositorios;

[TestClass]
public class RepositorioClustersPruebas
{
    private SqlContext _context = null!;
    private RepositorioClusters _repo = null!;

    [TestInitialize]
    public void Setup()
    {
        var opciones = SqlContextFactoryPruebas.CrearOpcionesInMemory("BD_Clusters");
        _context = SqlContextFactoryPruebas.CrearContexto(opciones);
        SqlContextFactoryPruebas.LimpiarBaseDeDatos(_context);
        _repo = new RepositorioClusters(_context);
    }

    [TestMethod]
    public void LimpiarCanonicoPorCatalogo_LimpiaCanonicoSoloDelCatalogo()
    {
        var cat1 = new Catalogo("Cat1");
        var cat2 = new Catalogo("Cat2");

        _context.Add(cat1);
        _context.Add(cat2);
        _context.SaveChanges();

        var itemA = Item.Crear("A", "d");
        var itemB = Item.Crear("B", "d");
        var itemC = Item.Crear("C", "d");

        _context.AddRange(itemA, itemB, itemC);

        var cl1 = new Cluster(1, new HashSet<Item>());
        cl1.Canonico = itemA;

        var cl2 = new Cluster(2, new HashSet<Item>());
        cl2.Canonico = itemB;

        var cl3 = new Cluster(3, new HashSet<Item>());
        cl3.Canonico = itemC;

        _context.AddRange(cl1, cl2, cl3);

        _context.Entry(cl1).Property("CatalogoId").CurrentValue = cat1.Id;
        _context.Entry(cl2).Property("CatalogoId").CurrentValue = cat1.Id;
        _context.Entry(cl3).Property("CatalogoId").CurrentValue = cat2.Id;

        _context.SaveChanges();

        _repo.LimpiarCanonicoPorCatalogo(cat1.Id);

        var actualizado1 = _context.Clusters.First(c => c.Id == cl1.Id);
        var actualizado2 = _context.Clusters.First(c => c.Id == cl2.Id);
        var actualizado3 = _context.Clusters.First(c => c.Id == cl3.Id);

        Assert.IsNull(actualizado1.Canonico);
        Assert.IsNull(actualizado2.Canonico);
        Assert.IsNotNull(actualizado3.Canonico);
        Assert.AreEqual(itemC.Id, actualizado3.Canonico!.Id);
    }

    [TestMethod]
    public void LimpiarCanonicoPorCatalogo_NoRompeSiNoHayCanonicosEnEseCatalogo()
    {
        var cat1 = new Catalogo("Cat1");
        var cat2 = new Catalogo("Cat2");

        _context.Add(cat1);
        _context.Add(cat2);
        _context.SaveChanges();

        var itemA = Item.Crear("A", "d");
        _context.Add(itemA);

        var cl1 = new Cluster(10, new HashSet<Item>());
        var cl2 = new Cluster(20, new HashSet<Item>());
        var cl3 = new Cluster(30, new HashSet<Item>());
        cl3.Canonico = itemA;

        _context.AddRange(cl1, cl2, cl3);

        _context.Entry(cl1).Property("CatalogoId").CurrentValue = cat1.Id;
        _context.Entry(cl2).Property("CatalogoId").CurrentValue = cat1.Id;
        _context.Entry(cl3).Property("CatalogoId").CurrentValue = cat2.Id;

        _context.SaveChanges();

        _repo.LimpiarCanonicoPorCatalogo(cat1.Id);

        var actualizado1 = _context.Clusters.First(c => c.Id == cl1.Id);
        var actualizado2 = _context.Clusters.First(c => c.Id == cl2.Id);
        var actualizado3 = _context.Clusters.First(c => c.Id == cl3.Id);

        Assert.IsNull(actualizado1.Canonico);
        Assert.IsNull(actualizado2.Canonico);
        Assert.IsNotNull(actualizado3.Canonico);
    }

    [TestMethod]
    public void LimpiarCanonicoPorCatalogo_NoAfectaClustersDeOtrosCatalogos()
    {
        var cat1 = new Catalogo("Cat1");
        var cat2 = new Catalogo("Cat2");

        _context.Add(cat1);
        _context.Add(cat2);
        _context.SaveChanges();

        var itemA = Item.Crear("A", "d");
        var itemB = Item.Crear("B", "d");

        _context.AddRange(itemA, itemB);

        var cl1 = new Cluster(100, new HashSet<Item>());
        cl1.Canonico = itemA;

        var cl2 = new Cluster(200, new HashSet<Item>());
        cl2.Canonico = itemB;

        _context.AddRange(cl1, cl2);

        _context.Entry(cl1).Property("CatalogoId").CurrentValue = cat1.Id;
        _context.Entry(cl2).Property("CatalogoId").CurrentValue = cat2.Id;

        _context.SaveChanges();

        _repo.LimpiarCanonicoPorCatalogo(cat1.Id);

        var actualizado1 = _context.Clusters.First(c => c.Id == cl1.Id);
        var actualizado2 = _context.Clusters.First(c => c.Id == cl2.Id);

        Assert.IsNull(actualizado1.Canonico);
        Assert.IsNotNull(actualizado2.Canonico);
        Assert.AreEqual(itemB.Id, actualizado2.Canonico!.Id);
    }
}