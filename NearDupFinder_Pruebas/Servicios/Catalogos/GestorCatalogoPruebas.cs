using NearDupFinder_Almacenamiento;
using NearDupFinder_Almacenamiento.Repositorios;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorCatalogo;
using NearDupFinder_Pruebas.Utilidades;
using NearDupFinder_Interfaces;
using NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;

namespace NearDupFinder_Pruebas.Servicios.Catalogos;

[TestClass]
public class GestorCatalogoPruebas
{
    private GestorCatalogos _gestorCatalogos = null!;
    private SqlContext _context = null!;

    [TestInitialize]
    public void Setup()
    {
        var opciones = SqlContextFactoryPruebas.CrearOpcionesInMemory("BD_Catalogos");
        _context = SqlContextFactoryPruebas.CrearContexto(opciones);
        SqlContextFactoryPruebas.LimpiarBaseDeDatos(_context);
    

    IRepositorioCatalogos repositorioCatalogos = new RepositorioCatalogos(_context);

        _gestorCatalogos = new GestorCatalogos(repositorioCatalogos);
    }
    
    [TestMethod]
    public void AgregarCatalogo_OkTest()
    {
        var cantidadDeCatalogo = _gestorCatalogos.CantidadDeCatalogos() + 1;
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("xxx"));

        Assert.AreEqual(cantidadDeCatalogo, _gestorCatalogos.CantidadDeCatalogos());
    }

    [TestMethod]
    public void AgregarCatalogo_Duplicado_ErrorTest()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("xxx"));

        var ex = Assert.ThrowsException<ExcepcionCatalogo>(() =>
            _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("xxx")));
        StringAssert.Contains(ex.Message, "Ya existe un catálogo con el título 'xxx'.");
    }

    [TestMethod]
    public void AgregarVariosCatalogos_OkTest()
    {
        var cantidadEsperada = _gestorCatalogos.CantidadDeCatalogos() + 2;

        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("xxx"));
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("xxxx"));

        Assert.AreEqual(cantidadEsperada, _gestorCatalogos.CantidadDeCatalogos());
    }

    [TestMethod]
    public void AgregarCatalogo_SeGuardaCorrectamente_ObtengoPorTitulo_OkTest()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("xxx"));

        Assert.AreEqual("xxx", _gestorCatalogos.ObtenerCatalogoPorTitulo("xxx")!.Titulo);
    }

    [TestMethod]
    public void AgregarCatalogo_SeGuardaCorrectamente_ObtengoPorId_OkTest()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("xxx"));

        var catalogo = _gestorCatalogos.ObtenerCatalogoPorTitulo("xxx");

        Assert.AreEqual("xxx", catalogo.Titulo);
    }


    [TestMethod]
    public void EliminarCatalogo_OkTest()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("xxx"));

        var cantidadEsperada = _gestorCatalogos.CantidadDeCatalogos() - 1;
        
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorTitulo("xxx");
        
        _gestorCatalogos.BorrarCatalogo(new DatosCatalogoEliminar(catalogo!.Id));

        Assert.AreEqual(cantidadEsperada, _gestorCatalogos.CantidadDeCatalogos());
    }

    [TestMethod]
    public void EliminarCatalogo_NoExisteCatalogo_ErrorTest()
    {
        var ex = Assert.ThrowsException<ExcepcionCatalogo>(() =>
            _gestorCatalogos.BorrarCatalogo(new DatosCatalogoEliminar(1)));
        StringAssert.Contains(ex.Message, "No existe un catálogo con Id=1");
    }

    [TestMethod]
    public void EliminarCatalogo_CaseInsensitive_OkTest()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("Cat1"));

        var cantidadEsperada = _gestorCatalogos.CantidadDeCatalogos() - 1;
        
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorTitulo("Cat1");
        
        _gestorCatalogos.BorrarCatalogo(new DatosCatalogoEliminar(catalogo.Id));

        Assert.AreEqual(cantidadEsperada, _gestorCatalogos.CantidadDeCatalogos());
    }

    [TestMethod]
    public void EliminarCatalogo_DobleEliminacion_ErrorTest()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("Cat1"));
        
        var catalogoId = _gestorCatalogos.ObtenerCatalogoPorTitulo("Cat1")!.Id;
        
        _gestorCatalogos.BorrarCatalogo(new DatosCatalogoEliminar(catalogoId));
        
        var ex = Assert.ThrowsException<ExcepcionCatalogo>(() =>
            _gestorCatalogos.BorrarCatalogo(new DatosCatalogoEliminar(catalogoId)));
        StringAssert.Contains(ex.Message, "No existe un catálogo con Id");
    }

    [TestMethod]
    public void CambiarTituloCatalogo_OkTest()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("Original"));
        
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorTitulo("Original");

        _gestorCatalogos.ModificarCatalogo(new DatosCatalogoEditar(catalogo!.Id, "NuevoTitulo"));

        Assert.AreEqual("NuevoTitulo", _gestorCatalogos.ObtenerCatalogoPorId(catalogo.Id)!.Titulo);
    }

    [TestMethod]
    public void CambiarTituloCatalogo_TituloYaExiste_ErrorTest()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("Cat1"));
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("Cat2"));
        
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorTitulo("Cat1");
        
        var ex = Assert.ThrowsException<ExcepcionCatalogo>(() =>
            _gestorCatalogos.ModificarCatalogo(new DatosCatalogoEditar(catalogo!.Id, "Cat2"))
        );
        StringAssert.Contains(ex.Message, "Ya existe un catálogo con el título 'Cat2'");
    }

    [TestMethod]
    public void CambiarTituloCatalogo_TituloMismoCatalogo_OkTest()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("Cat1"));
        
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorTitulo("Cat1");
        
        _gestorCatalogos.ModificarCatalogo(new DatosCatalogoEditar(catalogo!.Id, "Cat1"));

        Assert.AreEqual("Cat1", _gestorCatalogos.ObtenerCatalogoPorId(catalogo.Id)!.Titulo);
    }

    [TestMethod]
    public void ModificarCampos_CatalogoNoExiste_ErrorTest()
    {
        var datosEditar = new DatosCatalogoEditar(1, "NuevoTitulo");

        var ex = Assert.ThrowsException<ExcepcionCatalogo>(() => _gestorCatalogos.ModificarCatalogo(datosEditar)
        );

        StringAssert.Contains(ex.Message, "Catálogo no encontrado (Id=1)");
    }

    [TestMethod]
    public void ModificarCampos_CambiarSoloDescripcion_OkTest()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("CatDesc"));
        var id = _gestorCatalogos.ObtenerCatalogoPorTitulo("CatDesc")!.Id;

        _gestorCatalogos.ModificarCatalogo(new DatosCatalogoEditar(id, null, "Nueva descripción"));

        var cat = _gestorCatalogos.ObtenerCatalogoPorId(id)!;
        Assert.AreEqual("CatDesc", cat.Titulo, "El título no debería cambiar.");
        Assert.AreEqual("Nueva descripción", cat.Descripcion);
    }

    [TestMethod]
    public void ModificarCampos_TituloSoloCambiaCase_NoCambiaNada_OkTest()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("MiTitulo"));
        var id = _gestorCatalogos.ObtenerCatalogoPorTitulo("MiTitulo")!.Id;

        _gestorCatalogos.ModificarCatalogo(new DatosCatalogoEditar(id, "mititulo"));

        var cat = _gestorCatalogos.ObtenerCatalogoPorId(id)!;
        Assert.AreEqual("MiTitulo", cat.Titulo, "No debería modificarse solo por el case.");
    }

    [TestMethod]
    public void ModificarCampos_CambiarTituloYDescripcion_OkTest()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("CatX"));

        var id = _gestorCatalogos.ObtenerCatalogoPorTitulo("CatX")!.Id;

        _gestorCatalogos.ModificarCatalogo(new DatosCatalogoEditar(id, "CatY", "Desc Y"));

        var cat = _gestorCatalogos.ObtenerCatalogoPorId(id)!;
        Assert.AreEqual("CatY", cat.Titulo);
        Assert.AreEqual("Desc Y", cat.Descripcion);
    }

    [TestMethod]
    public void Catalogos_DevuelveLosCatalogosExistentes_OkTest()
    {
        // Arrange
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("Cat1"));
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("Cat2"));

        // Act
        var catalogos = _gestorCatalogos.ObtenerCatalogos();

        // Assert
        Assert.AreEqual(2, catalogos.Count, "La cantidad de catálogos devuelta no es la esperada.");
        Assert.IsTrue(catalogos.Any(c => c.Titulo == "Cat1"), "No se encontró el catálogo 'Cat1'.");
        Assert.IsTrue(catalogos.Any(c => c.Titulo == "Cat2"), "No se encontró el catálogo 'Cat2'.");
    }


    [TestMethod]
    public void ObtenerItemsDelCatalogo_CatalogoNoExiste_DeberiaLanzarExcepcion()
    {
        var idInexistente = 999;

        var ex = Assert.ThrowsException<ExcepcionCatalogo>(() => _gestorCatalogos.ObtenerItemsDelCatalogo(idInexistente)
        );

        StringAssert.Contains(ex.Message, "El id no corresponde con ningún catálogo"
        );
    }

    [TestMethod]
    public void ObtenerItemsDelCatalogo_DevuelveColeccionDelCatalogo_OkTest()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("CatConItems"));
        var id = _gestorCatalogos.ObtenerCatalogoPorTitulo("CatConItems")!.Id;

        var items = _gestorCatalogos.ObtenerItemsDelCatalogo(id);

        Assert.IsNotNull(items, "La colección de ítems no debería ser null.");
        Assert.AreEqual(0, items.Count, "Un catálogo recién creado debería no tener ítems.");
    }
    
    [TestMethod]
    public void ObtenerCatalogoDtoPorId_CuandoExiste_RetornaDtoCorrecto_OkTest()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("CatDto1"));
        var id = _gestorCatalogos.ObtenerCatalogoPorTitulo("CatDto1")!.Id;

        var dto = _gestorCatalogos.ObtenerCatalogoDtoPorId(id);

        Assert.IsNotNull(dto, "Debería retornar un DTO cuando el catálogo existe.");
        Assert.AreEqual(id, dto!.Id);
        Assert.AreEqual("CatDto1", dto.Titulo);
    }

    [TestMethod]
    public void ObtenerCatalogoDtoPorId_CuandoNoExiste_RetornaNull_OkTest()
    {
        var dto = _gestorCatalogos.ObtenerCatalogoDtoPorId(999);

        Assert.IsNull(dto, "Debe retornar null cuando el catálogo no existe.");
    }
    
    [TestMethod]
    public void ObtenerCatalogoDtoPorTitulo_CuandoExiste_RetornaDtoCorrecto()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("CatDtoTitulo"));
        
        var dto = _gestorCatalogos.ObtenerCatalogoDtoPorTitulo("CatDtoTitulo");

        Assert.IsNotNull(dto, "Debería retornar un DTO cuando el catálogo existe.");
        Assert.AreEqual("CatDtoTitulo", dto!.Titulo);
    }

    [TestMethod]
    public void ObtenerCatalogoDtoPorTitulo_CuandoNoExiste_RetornaNull()
    {
        var dto = _gestorCatalogos.ObtenerCatalogoDtoPorTitulo("Inexistente");

        Assert.IsNull(dto, "Debe retornar null cuando el catálogo no existe.");
    }

    [TestMethod]
    public void ObtenerCatalogos_SinCatalogos_RetornaColeccionVacia()
    {
        var resultado = _gestorCatalogos.ObtenerCatalogos();

        Assert.IsNotNull(resultado, "El resultado no debería ser null.");
        Assert.AreEqual(0, resultado.Count, "Debe retornar una colección vacía cuando no hay catálogos.");
    }

    [TestMethod]
    public void ObtenerCatalogos_ConVariosCatalogos_RetornaTodosLosDtos()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("Cat1"));
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("Cat2"));
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("Cat3"));

        var resultado = _gestorCatalogos.ObtenerCatalogos();

        Assert.AreEqual(3, resultado.Count, "Debe retornar un DTO por cada catálogo existente.");
        Assert.IsTrue(resultado.Any(d => d.Titulo == "Cat1"), "No se encontró el DTO correspondiente a 'Cat1'.");
        Assert.IsTrue(resultado.Any(d => d.Titulo == "Cat2"), "No se encontró el DTO correspondiente a 'Cat2'.");
        Assert.IsTrue(resultado.Any(d => d.Titulo == "Cat3"), "No se encontró el DTO correspondiente a 'Cat3'.");
    }

    [TestMethod]
    public void ObtenerCatalogos_VerificaDatosDeLosDtos_CoincidenConCatalogosOriginales()
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("CatA", "DescA"));
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("CatB", "DescB"));

        var resultado = _gestorCatalogos.ObtenerCatalogos().ToList();

        Assert.AreEqual("CatA", resultado[0].Titulo);
        Assert.AreEqual("DescA", resultado[0].Descripcion);
        Assert.AreEqual("CatB", resultado[1].Titulo);
        Assert.AreEqual("DescB", resultado[1].Descripcion);
    }
}