using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio;
using NearDupFinder_LogicaDeNegocio.DTO;
using NearDupFinder_LogicaDeNegocio.Servicios;

namespace NearDupFinder_Pruebas.Dominio.Clases;

[TestClass]
public class SistemaPruebas
{
    private Sistema _sistema = null!;
    private Catalogo _catalogo = null!;
    private AlmacenamientoDeDatos _almacenamiento = null!;
    [TestInitialize]
    public void Setup()
    {
        _sistema = new Sistema();
        _almacenamiento = new AlmacenamientoDeDatos();
        _catalogo = new Catalogo("Catalogo Test");
        _sistema.AgregarCatalogo(_catalogo);
    }

    [TestMethod]
    public void BuscarUsuarioPorId_IDNoExsite_RetornaNulo()
    {
        int idInexistente = Int32.MaxValue;
        Usuario? usuarioABuscar = _almacenamiento.BuscarUsuarioPorId(idInexistente);
        Assert.IsNull(usuarioABuscar);
    }

    [TestMethod]
    public void BuscarUsuarioPorId_IDExsite_RetornaUsuarioValido()
    {
        Sistema sistema = new Sistema();
        Usuario? admin = _almacenamiento.ObtenerUsuarios().FirstOrDefault();
        int idValido = admin!.Id;
        Usuario? usuarioABuscar = _almacenamiento.BuscarUsuarioPorId(idValido);
        Assert.AreEqual(admin, usuarioABuscar);
    }
    
    [TestMethod]
    public void ObtenerItemsDelCatalogo_CatalogoVacio_RetornaCero()
    {
        Sistema sistema = new Sistema();
        int numeroDeItemsDelCatalogo = sistema.ObtenerItemsDelCatalogo(_catalogo).Count;
        Assert.AreEqual(numeroDeItemsDelCatalogo, 0);
    }
    
    [TestMethod]
    public void ObtenerItemsDelCatalogo_CatalogoConItem_RetornaUno()
    {
        var item = new Item("Original", "Descripcion original")
        {
            Categoria = "Cat 1",
            Marca = "Marca 1",
            Modelo = "Modelo 1"
        };  
        _sistema.AltaItemConAltaDuplicados(_catalogo.Titulo, item);
        int numeroDeItemsDelCatalogo = _sistema.ObtenerItemsDelCatalogo(_catalogo).Count;
        Assert.AreEqual(numeroDeItemsDelCatalogo, 1);
    }

    [TestMethod]
    public void ActualizarItemEnCatalogo_ModificaTituloYDescripcion()
    {
        var sistema = new Sistema();
        var catalogo = new Catalogo("Catálogo Test");
        var item = new Item("Original", "Descripcion original")
        {
            Categoria = "Cat 1",
            Marca = "Marca 1",
            Modelo = "Modelo 1"
        };
        catalogo.AgregarItem(item);
        var dto = new ItemDto
        {
            Id = item.Id,
            Titulo = "Nuevo Título",
            Descripcion = "Nueva Descripción",
            Categoria = "Cat 1",
            Marca = "Marca 1",
            Modelo = "Modelo 1"
        };
        sistema.ActualizarItemEnCatalogo(catalogo, dto);
        Assert.AreEqual("Nuevo Título", item.Titulo);
        Assert.AreEqual("Nueva Descripción", item.Descripcion);
    }

    [TestMethod]
    public void ActualizarItemEnCatalogo_ModificaCategoriaMarcaModelo()
    {
        var sistema = new Sistema();
        var catalogo = new Catalogo("Catálogo Test");
        var item = new Item("Original", "Descripcion original")
        {
            Categoria = "Cat 1",
            Marca = "Marca 1",
            Modelo = "Modelo 1"
        };
        catalogo.AgregarItem(item);
        var dto = new ItemDto
        {
            Id = item.Id,
            Titulo = "Original",
            Descripcion = "Descripcion original",
            Categoria = "Cat 2",
            Marca = "Marca 2",
            Modelo = "Modelo 2"
        };
        sistema.ActualizarItemEnCatalogo(catalogo, dto);
        Assert.AreEqual("Cat 2", item.Categoria);
        Assert.AreEqual("Marca 2", item.Marca);
        Assert.AreEqual("Modelo 2", item.Modelo);
    }

    [TestMethod]
    public void ActualizarItemEnCatalogo_ItemNoExiste_Excepcion()
    {
        var sistema = new Sistema();
        var catalogo = new Catalogo("Catálogo Test");

        var dto = new ItemDto
        {
            Id = 999, 
            Titulo = "Título",
            Descripcion = "Descripcion",
            Categoria = "Cat",
            Marca = "Marca",
            Modelo = "Modelo"
        };
        var error = Assert.ThrowsException<ExcepcionDeItem>(() => sistema.ActualizarItemEnCatalogo(catalogo, dto)
        );

        Assert.AreEqual("No se encontró el item a actualizar.", error.Message);
    }

    [TestMethod]
    public void AltaItem_AgregaItemAlCatalogo()
    {
        var sistema = new Sistema();
        var catalogo = new Catalogo("Catálogo Test");
        sistema.AgregarCatalogo(catalogo);
        var nuevoItem = new Item("Item 1", "Descripción 1");

        sistema.AltaItemConAltaDuplicados("Catálogo Test", nuevoItem);
        var items = catalogo.Items;

        Assert.AreEqual(1, items.Count);
        Assert.AreEqual("Item 1", items.First().Titulo);
        Assert.AreEqual("Descripción 1", items.First().Descripcion);
    }

    [TestMethod]
    [ExpectedException(typeof(ExcepcionDeItem))]
    public void AltaItem_LanzaExcepcionSiTituloOVacio()
    {
        var sistema = new Sistema();
        var catalogo = new Catalogo("Catálogo Test");
        sistema.AgregarCatalogo(catalogo);

        var nuevoItem = new Item("", "Descripción 1");
        sistema.AltaItemConAltaDuplicados("Catálogo Test", nuevoItem);
    }

    [TestMethod]
    [ExpectedException(typeof(ExcepcionDeItem))]
    public void AltaItem_DescripcionVacia_Excepcion()
    {
        var sistema = new Sistema();
        var catalogo = new Catalogo("Catálogo Test");
        sistema.AgregarCatalogo(catalogo);

        var nuevoItem = new Item("Titulo", "");
        sistema.AltaItemConAltaDuplicados("Catálogo Test", nuevoItem);
    }

    [TestMethod]
    [ExpectedException(typeof(ExcepcionDeItem))]
    public void AltaItem_Nulo_Excepcion()
    {
        _sistema.AltaItemConAltaDuplicados("Catalogo Test", null);
    }

    [TestMethod]
    public void AltaItemConAltaDuplicados_AgregaItemYGeneraDuplicadoEnListaGlobal()
    {
        var catalogo = new Catalogo("Catálogo Test");
        _sistema.AgregarCatalogo(catalogo); 

        var item1 = new Item("Titulo 1", "Descripcion 1");
        var item2 = new Item("Titulo 1", "Descripcion 1");

        _sistema.AltaItemConAltaDuplicados("Catálogo Test", item1);
        _sistema.AltaItemConAltaDuplicados("Catálogo Test", item2);

        int cantidadDuplicadoGlobalesUno = 1;

        Assert.AreEqual(cantidadDuplicadoGlobalesUno, _sistema.DuplicadosGlobales.Count);
    }

    [TestMethod]
    public void ActualizarDuplicados_MarcaEstadoDuplicadoEnItems()
    {
        var item1 = new Item("Titulo", "Descripcion");
        var item2 = new Item("Titulo", "Descripcion");
        _catalogo.AgregarItem(item1);
        _catalogo.AgregarItem(item2);

        _sistema.ActualizarDuplicadosPara(_catalogo, item1);

        Assert.IsTrue(item1.EstadoDuplicado);
        Assert.IsTrue(item2.EstadoDuplicado);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ActualizarDuplicadosPara_ExcepcionSiCatalogoEsNull()
    {
        var item = new Item("Titulo", "Descripcion");
        _sistema.ActualizarDuplicadosPara(null, item);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ActualizarDuplicadosPara_LanzaExcepcionSiItemEsNull()
    {
        _sistema.ActualizarDuplicadosPara(_catalogo, null);
    }

    [TestMethod]
    public void ActualizarDuplicados_EliminaDuplicadosPreviosDelItem()
    {
        var item1 = new Item("Item 1", "Desc 1");
        var item2 = new Item("Item 2", "Desc 2");

        _sistema.AltaItemConAltaDuplicados("Catalogo Test", item1);
        _sistema.AltaItemConAltaDuplicados("Catalogo Test", item2);

        item1.Titulo = " Editado";
        item1.Descripcion = "Editada";

        _sistema.ActualizarDuplicadosPara(_catalogo, item1);

        Assert.AreEqual(0, _sistema.DuplicadosGlobales.Count);
    }

    [TestMethod]
    public void AltaItemConDuplicados_ItemTieneDuplicado_EstadoDuplicadoEsTrue()
    {
        var item1 = new Item("Titulo", "Descripcion");
        var item2 = new Item("Titulo", "Descripcion");

        _sistema.AltaItemConAltaDuplicados(_catalogo.Titulo, item1);
        _sistema.AltaItemConAltaDuplicados(_catalogo.Titulo, item2);


        Assert.IsTrue(item1.EstadoDuplicado, "Item1 debería estar marcado como duplicado");
        Assert.IsTrue(item2.EstadoDuplicado, "Item2 debería estar marcado como duplicado");
        int cantidadDuplicadosCero = 0;

        Assert.IsTrue(_sistema.DuplicadosGlobales.Count > cantidadDuplicadosCero);
    }

    [TestMethod]
    public void ActualizarDuplicados_ItemEditado_RecalculaDuplicadosGlobales()
    {
        var item1 = new Item("Titulo 1", "Desc");
        var item2 = new Item("Titulo 1", "Desc");
        var item3 = new Item("TituloNuevo", "DescNueva");

        _catalogo.AgregarItem(item1);
        _catalogo.AgregarItem(item2);
        _catalogo.AgregarItem(item3);

        item1.Titulo = "TituloNuevo";
        item1.Descripcion = "DescNueva";
        _sistema.ActualizarDuplicadosPara(_catalogo, item1);
        int cantidadDuplicadosCero = 0;

        Assert.IsTrue(_sistema.DuplicadosGlobales.Count > cantidadDuplicadosCero);

    }

    [TestMethod]
    public void ActualizarDuplicados_RecalculaDuplicadosParaOtrosItems()
    {
        var item1 = new Item("Titulo", "Descripcion");
        var item2 = new Item("Titulo", "Descripcion");
        var item3 = new Item("TituloDiferente", "DescripcionDiferente");

        _catalogo.AgregarItem(item1);
        _catalogo.AgregarItem(item2);
        _catalogo.AgregarItem(item3);

        item1.Titulo = "TituloDiferente";
        item1.Descripcion = "DescripcionDiferente";

        _sistema.ActualizarDuplicadosPara(_catalogo, item3);
        Assert.IsTrue(item3.EstadoDuplicado, "Item3 debería estar marcado como duplicado");
    }

    [TestMethod]
    public void ActualizarDuplicados_MarcaDuplicadosCorrectamente()
    {

        var item1 = new Item("Titulo", "Descripcion");
        var item2 = new Item("Titulo", "Descripcion");
        var item3 = new Item("Otro Titulo", "Otra Descripcion");
        var item4 = new Item("Titulo", "Descripcion");

        _sistema.AltaItemConAltaDuplicados("Catalogo Test", item1);
        _sistema.AltaItemConAltaDuplicados("Catalogo Test", item2);
        _sistema.AltaItemConAltaDuplicados("Catalogo Test", item3);
        _sistema.AltaItemConAltaDuplicados("Catalogo Test", item4);

        item1.Titulo = "nosoyuntitle1111111111";
        item1.Descripcion = "nosoydescripcion11111111";
        _sistema.ActualizarDuplicadosPara(_catalogo, item1);

        Assert.IsFalse(item1.EstadoDuplicado, "Item1 debe estar marcado como no duplicado");
        Assert.IsTrue(item2.EstadoDuplicado, "Item2 debe estar marcado como  duplicado");
    }

    [TestMethod]
    public void AltaItemConDuplicados_AgregaItemConIdNoValido_CambiandoleElId()
    {
        var item1 = new Item("Titulo", "Descripcion");
        var item2 = new Item("Titulo", "Descripcion");
        int idItem1 = item1.Id;
        item2.AjustarId(idItem1);

        _sistema.AltaItemConAltaDuplicados(_catalogo.Titulo, item1);
        _sistema.AltaItemConAltaDuplicados(_catalogo.Titulo, item2);

        bool losIdsNoSonIguales = item1.Id != item2.Id;
        bool item1Existe = _sistema.IdExisteEnListaDeIdGlobal(item1.Id);
        bool item2Existe = _sistema.IdExisteEnListaDeIdGlobal(item2.Id);

        Assert.IsTrue(losIdsNoSonIguales);
        Assert.IsTrue(item1Existe);
        Assert.IsTrue(item2Existe);
    }

    [TestMethod]
    public void CantidadDeItemsGlobal_SinItems_RetornaCero()
    {
        int numeroDeItems = _sistema.CantidadDeItemsGlobal();
        Assert.AreEqual(0, numeroDeItems);
    }

    [TestMethod]
    public void CantidadDeItemsGlobal_ConItems_RetornaDistintoDeCero()
    {
        var nuevoItem = new Item("Item 1", "Descripción 1");
        _sistema.AltaItemConAltaDuplicados(_catalogo.Titulo, nuevoItem);

        int numeroDeItems = _sistema.CantidadDeItemsGlobal();

        Assert.AreNotEqual(0, numeroDeItems);
        Assert.AreEqual(1, numeroDeItems);
    }

    [TestMethod]
    public void IdExisteEnListaDeIdGlobal_ConItemNoExistente_RetornaFalso()
    {
        var nuevoItem = new Item("Item 1", "Descripción 1");

        bool existeItem = _sistema.IdExisteEnListaDeIdGlobal(nuevoItem.Id);

        Assert.IsFalse(existeItem);
    }

    [TestMethod]
    public void IdExisteEnListaDeIdGlobal_ConItemExistente_RetornaVerdadero()
    {
        var nuevoItem = new Item("Item 1", "Descripción 1");
        _sistema.AltaItemConAltaDuplicados(_catalogo.Titulo, nuevoItem);
        bool existeItem = _sistema.IdExisteEnListaDeIdGlobal(nuevoItem.Id);

        Assert.IsTrue(existeItem);
    }

    [TestMethod]
    public void ImportarItemsDesdeCsv_AgregaItems()
    {
        var titulos = new List<string> { "id", "titulo" };
        var filas = new List<Fila> { new Fila("1", "t", "m", "x", "d", "c", "Cat 1") };
       
        _sistema.ImportarItemsDesdeCsv(titulos, 1, filas);
        
        bool itemExiste = _sistema.IdExisteEnListaDeIdGlobal(1);
        Assert.IsTrue(itemExiste);
    }

    [TestMethod]
    public void DescratarDuplicado_DeberiaEliminarDuplicadoYActualizarEstado()
    {

        var item1 = new Item { Titulo = "Item1", Descripcion = "Desc1" };
        var item2 = new Item { Titulo = "Item2", Descripcion = "Desc2" };
        var duplicado = new ParDuplicado
        {
            ItemAComparar = item1,
            ItemPosibleDuplicado = item2,
            Score = 0.9f,
            Tipo = TipoDuplicado.PosibleDuplicado
        };

        _sistema.DuplicadosGlobales.Add(duplicado);
        item1.EstadoDuplicado = true;
        item2.EstadoDuplicado = true;
        _sistema.DescartarParDuplicado(duplicado);

        Assert.IsFalse(_sistema.DuplicadosGlobales.Contains(duplicado), "El duplicado debería haberse eliminado");
    }
    [TestMethod]
    public void DescratarDuplicado_ActualizarEstado()
    {

        var item1 = new Item { Titulo = "Item1", Descripcion = "Desc1" };
        var item2 = new Item { Titulo = "Item2", Descripcion = "Desc2" };
        var duplicado = new ParDuplicado
        {
            ItemAComparar = item1,
            ItemPosibleDuplicado = item2,
            Score = 0.9f,
            Tipo = TipoDuplicado.PosibleDuplicado
        };

        _sistema.DuplicadosGlobales.Add(duplicado);
        item1.EstadoDuplicado = true;
        item2.EstadoDuplicado = true;
        _sistema.DescartarParDuplicado(duplicado);

      
        Assert.IsFalse(item1.EstadoDuplicado, "ItemA ya no debería estar marcado como duplicado");
        Assert.IsFalse(item2.EstadoDuplicado, "ItemB ya no debería estar marcado como duplicado");
    }


    [TestMethod]
    public void MarcarNoDuplicado_ItemConOtroDuplicado_EstadoDuplicadoPermaneceTrue()
    {
        var item1 = new Item { Titulo = "Item1", Descripcion = "Desc1" };
        var item2 = new Item { Titulo = "Item2", Descripcion = "Desc2" };
        var item3 = new Item { Titulo = "Item3", Descripcion = "Desc3" };

        var duplicado1 = new ParDuplicado
        {
            ItemAComparar = item1,
            ItemPosibleDuplicado = item2,
            Score = 0.9f,
            Tipo = TipoDuplicado.PosibleDuplicado
        };
        var duplicado2 = new ParDuplicado
        {
            ItemAComparar = item1,
            ItemPosibleDuplicado = item3,
            Score = 0.8f,
            Tipo = TipoDuplicado.PosibleDuplicado
        };
        _sistema.DuplicadosGlobales.Add(duplicado1);
        _sistema.DuplicadosGlobales.Add(duplicado2);

        item1.EstadoDuplicado = true;
        item2.EstadoDuplicado = true;
        item3.EstadoDuplicado = true;

        _sistema.DescartarParDuplicado(duplicado1);
        Assert.IsFalse(_sistema.DuplicadosGlobales.Contains(duplicado1));
        Assert.IsTrue(item1.EstadoDuplicado);
        Assert.IsFalse(item2.EstadoDuplicado);
        Assert.IsTrue(item3.EstadoDuplicado);
    }

    
    [TestMethod]
    public void ConfirmarClusterDesdeSistema()
    {
        var item1 = new Item { Titulo = "Item1", Descripcion = "Desc1" , Marca = "x"};
        var item2 = new Item { Titulo = "Item1", Descripcion = "Desc1" , Marca = "x" };
        
        _sistema.AltaItemConAltaDuplicados(_catalogo.Titulo, item1);
        _sistema.AltaItemConAltaDuplicados(_catalogo.Titulo, item2);

        var parDuplicados = _sistema.DuplicadosGlobales.First();
        var clusters = _catalogo.Clusters.ToList();

        var cantidadEsperada = 0;
        Assert.AreEqual(cantidadEsperada,clusters.Count());
        
        _sistema.ConfirmarParDuplicado(parDuplicados);
        clusters = _catalogo.Clusters.ToList();
        cantidadEsperada = 1;
        
        Assert.AreEqual(cantidadEsperada,clusters.Count());
    }
    
    [TestMethod]
    public void RemoverClusterDesdeSistema()
    {
        var item1 = new Item { Titulo = "Item1", Descripcion = "Desc1" , Marca = "x"};
        var item2 = new Item { Titulo = "Item1", Descripcion = "Desc1" , Marca = "x" };
        
        _sistema.AltaItemConAltaDuplicados(_catalogo.Titulo, item1);
        _sistema.AltaItemConAltaDuplicados(_catalogo.Titulo, item2);

        var parDuplicados = _sistema.DuplicadosGlobales.First();
        int cantidadEsperada = 1;
        
        _sistema.ConfirmarParDuplicado(parDuplicados);
        
        var clusters = _catalogo.Clusters.ToList();
        
        Assert.AreEqual(cantidadEsperada,clusters.Count());
        
        _sistema.RemoverItemDelCluster(_catalogo, item1);
        
        clusters = _catalogo.Clusters.ToList();
        cantidadEsperada = 0;
        
        Assert.AreEqual(cantidadEsperada ,clusters.Count());
    }
    [TestMethod]
    public void FucionarCampos_AsignarItemCanonicoDeCluster()
    {
        var item1 = new Item { Titulo = "Item1", Descripcion = "Desc1 111111" , Marca = "x"};
        var item2 = new Item { Titulo = "Item1", Descripcion = "Desc1" , Marca = "x", Categoria = "alguna"};
        
        _sistema.AltaItemConAltaDuplicados(_catalogo.Titulo, item1);
        _sistema.AltaItemConAltaDuplicados(_catalogo.Titulo, item2);

        var parDuplicados = _sistema.DuplicadosGlobales.First();
        _sistema.ConfirmarParDuplicado(parDuplicados);
        
        var clusters = _catalogo.Clusters.ToList();
        _sistema.FusionarItemsEnElCLuster(clusters.First());
        var canonico = clusters.First().Canonico;
        
        Assert.IsNotNull(canonico);
        Assert.AreEqual("alguna",canonico.Categoria);
        
    }
    [TestMethod]
    public void SetearElItemCanonicoDeCluster_Null()
    {
        var item1 = new Item { Titulo = "Item1", Descripcion = "Desc1" , Marca = "x"};
        var item2 = new Item { Titulo = "Item1", Descripcion = "Desc1" , Marca = "x" };
        
        _sistema.AltaItemConAltaDuplicados(_catalogo.Titulo, item1);
        _sistema.AltaItemConAltaDuplicados(_catalogo.Titulo, item2);

        var parDuplicados = _sistema.DuplicadosGlobales.First();
        _sistema.ConfirmarParDuplicado(parDuplicados);
        var clusters = _catalogo.Clusters.ToList();
        
        _sistema.FusionarItemsEnElCLuster(clusters.First());
        Assert.IsNotNull(clusters.First().Canonico);
        
        _sistema.AsignarNuloCanonico(clusters.First());
        Assert.IsNull(clusters.First().Canonico);
    }
    
    [TestMethod]
    public void RetornaFalse_SiCatalogoEsNull()
    {
        var dto = new ItemDto { Id = 1 };
        var resultado = _sistema.ItemEstaEnCluster(null, dto);
        Assert.IsFalse(resultado);
    }
    
    [TestMethod]
    public void RetornaFalse_SiDtoEsNull()
    {
        var resultado = _sistema.ItemEstaEnCluster(_catalogo, null);
        Assert.IsFalse(resultado);
    }
    [TestMethod]
    public void RetornaFalse_SiItemNoExisteEnCatalogo()
    {
        var item = new Item {  Titulo = "Item 1" };
        _catalogo.AgregarItem(item);
        var dto = new ItemDto { Id = 999 }; 
        var resultado = _sistema.ItemEstaEnCluster(_catalogo, dto);
        Assert.IsFalse(resultado);
    }
    
    [TestMethod]
    public void RetornaFalse_SiItemExistePeroNoTieneCluster()
    {
        var item = new Item { Titulo = "Item sin cluster" };
        _catalogo.AgregarItem(item);
        var dto = new ItemDto { Id = item.Id };
        var resultado = _sistema.ItemEstaEnCluster(_catalogo, dto);
        Assert.IsFalse(resultado);
    }
    
    [TestMethod]
    public void RetornaTrue_SiItemPerteneceAUnCluster()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "d" };
        var itemB = new Item { Titulo = "B", Descripcion = "d" };
        _catalogo.AgregarItem(itemA);
        _catalogo.AgregarItem(itemB);
        _catalogo.ConfirmarClusters(itemA, itemB);
        _catalogo.ConfirmarClusters(itemB, itemA);
       
        var dto = new ItemDto { Id = itemA.Id };
        var resultado = _sistema.ItemEstaEnCluster(_catalogo, dto);
        
        Assert.IsTrue(resultado);
    }
    
}