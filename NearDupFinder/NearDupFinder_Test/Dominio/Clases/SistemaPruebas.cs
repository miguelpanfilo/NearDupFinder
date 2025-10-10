using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Controladores;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Test.Dominio.Clases;

[TestClass]
public class SistemaPruebas
{
    private Sistema _sistema;
    private Catalogo _catalogo;

    [TestInitialize]
    public void Setup()
    {
        _sistema = new Sistema();
        _catalogo = new Catalogo("Catalogo Test");
        _sistema.AgregarCatalogo(_catalogo);
    }

    [TestMethod]
    public void BuscarUsuarioPorId_IDNoExsite_RetornaNulo()
    {
        Sistema sistema = new Sistema();
        int idInexistente = Int32.MaxValue;

        Usuario? usuarioABuscar = sistema.BuscarUsuarioPorId(idInexistente);

        Assert.IsNull(usuarioABuscar);
    }

    [TestMethod]
    public void BuscarUsuarioPorId_IDExsite_RetornaUsuarioValido()
    {
        Sistema sistema = new Sistema();
        Usuario? admin = sistema.ObtenerUsuarios().FirstOrDefault();
        int idValido = admin!.Id;

        Usuario? usuarioABuscar = sistema.BuscarUsuarioPorId(idValido);

        Assert.AreEqual(admin, usuarioABuscar);
    }
    // Inicio Pruebas Catalogo

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

    // Inicio Pruebas Items

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
        var dto = new ItemEditDataTransfer
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

        var dto = new ItemEditDataTransfer
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

        var dto = new ItemEditDataTransfer
        {
            Id = 999, // Id inexistente
            Titulo = "Título",
            Descripcion = "Descripcion",
            Categoria = "Cat",
            Marca = "Marca",
            Modelo = "Modelo"
        };

        var ex = Assert.ThrowsException<ItemException>(() => sistema.ActualizarItemEnCatalogo(catalogo, dto)
        );

        Assert.AreEqual("No se encontró el item a actualizar.", ex.Message);
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
    [ExpectedException(typeof(ItemException))]
    public void AltaItem_LanzaExcepcionSiTituloOVacio()
    {
        var sistema = new Sistema();
        var catalogo = new Catalogo("Catálogo Test");
        sistema.AgregarCatalogo(catalogo);

        var nuevoItem = new Item("", "Descripción 1");
        sistema.AltaItemConAltaDuplicados("Catálogo Test", nuevoItem);
    }

    [TestMethod]
    [ExpectedException(typeof(ItemException))]
    public void AltaItem_DescripcionVacia_Excepcion()
    {
        var sistema = new Sistema();
        var catalogo = new Catalogo("Catálogo Test");
        sistema.AgregarCatalogo(catalogo);

        var nuevoItem = new Item("Titulo", "");
        sistema.AltaItemConAltaDuplicados("Catálogo Test", nuevoItem);
    }

    [TestMethod]
    [ExpectedException(typeof(ItemException))]
    public void AltaItem_Nulo_Excepcion()
    {
        _sistema.AltaItemConAltaDuplicados("Catalogo Test", null);
    }

    [TestMethod]
    public void AltaItemConAltaDuplicados_AgregaItemYGeneraDuplicadoEnListaGlobal()
    {

        var catalogo = new Catalogo("Catálogo Test");
        _sistema.AgregarCatalogo(catalogo); // Necesitás este método en tu sistema

        var item1 = new Item("Titulo 1", "Descripcion 1");
        var item2 = new Item("Titulo 1", "Descripcion 1");

        _sistema.AltaItemConAltaDuplicados("Catálogo Test", item1);
        _sistema.AltaItemConAltaDuplicados("Catálogo Test", item2);

        Assert.AreEqual(1, _sistema.DuplicadosGlobales.Count);
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

        Assert.AreEqual(0, _sistema.DuplicadosGlobales.Count, "Los duplicados previos deberían eliminarse.");
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

        Assert.IsTrue(_sistema.DuplicadosGlobales.Count > 0, "Debe existir al menos un duplicado global");
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

        Assert.IsTrue(_sistema.DuplicadosGlobales.Count > 0, "Debe existir el duplicado tras editar item 1 con item 3");

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
    public void EliminarItem_EliminaItemDelCatalogo()
    {
        var item1 = new Item("Item 1", "Desc 1");
        var item2 = new Item("Item 2", "Desc 2");

        _catalogo.AgregarItem(item1);
        _catalogo.AgregarItem(item2);

        var dto = ItemEditDataTransfer.FromEntity(item1);
        _sistema.EliminarItem("Catalogo Test", dto);

        Assert.IsFalse(_catalogo.Items.Contains(item1), "Item1 debe ser eliminado del catálogo");
        Assert.IsTrue(_catalogo.Items.Contains(item2), "Item2 debe permanecer en el catálogo");
    }

    [TestMethod]
    public void EliminarItemYActualizarDuplicados_EliminaDuplicadosGlobalesDelItem()
    {
        var item1 = new Item("Titulo", "Descripcion");
        var item2 = new Item("Titulo", "Descripcion");
        var item3 = new Item("Otro", "Desc");

        _sistema.AltaItemConAltaDuplicados("Catalogo Test", item1);
        _sistema.AltaItemConAltaDuplicados("Catalogo Test", item2);
        _sistema.AltaItemConAltaDuplicados("Catalogo Test", item3);

        var dto = ItemEditDataTransfer.FromEntity(item1);
        _sistema.EliminarItem("Catalogo Test", dto);

        Assert.IsFalse(item2.EstadoDuplicado);
    }

    [TestMethod]
    [ExpectedException(typeof(ItemException))]
    public void EliminarItem_ItemNoExistente_NoLanzaExcepcion()
    {
        var item = new Item("ItemInexistente", "Desc");
        var dto = ItemEditDataTransfer.FromEntity(item);
        _sistema.EliminarItem("Catalogo Test", dto);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarItem_CatalogoNoExistente_LanzaExcepcion()
    {
        var item = new Item("ItemInexistente", "Desc");
        var dto = ItemEditDataTransfer.FromEntity(item);
        _sistema.EliminarItem("Catalogo Inexistente", dto);
    }

    [TestMethod]
    public void AltaItemConDuplicados_AgregaItemConIdNoValido_CambiandoleElId()
    {
        var item1 = new Item("Titulo", "Descripcion");
        var item2 = new Item("Titulo", "Descripcion");
        int idItem1 = item1.Id;
        item2.ModificarId(idItem1);

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
            ItemA = item1,
            ItemB = item2,
            Score = 0.9f,
            Tipo = TipoDuplicado.PosibleDuplicado
        };

        _sistema.DuplicadosGlobales.Add(duplicado);

        item1.EstadoDuplicado = true;
        item2.EstadoDuplicado = true;

        _sistema.DescartarParDuplicado(duplicado);


        Assert.IsFalse(_sistema.DuplicadosGlobales.Contains(duplicado), "El duplicado debería haberse eliminado");
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
            ItemA = item1,
            ItemB = item2,
            Score = 0.9f,
            Tipo = TipoDuplicado.PosibleDuplicado
        };

        var duplicado2 = new ParDuplicado
        {
            ItemA = item1,
            ItemB = item3,
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
    public void FromEntity_CreaDtoCorrectamente()
    {
        var item = new Item("Título prueba", "Descripción prueba", "Marca1", "Modelo1", "Cat1");

        var dto = ItemEditDataTransfer.FromEntity(item);

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

        var dto = ItemEditDataTransfer.FromEntity(item);

        Assert.AreEqual(item.Id, dto.Id);
        Assert.AreEqual(item.Titulo, dto.Titulo);
        Assert.AreEqual(item.Descripcion, dto.Descripcion);
        Assert.AreEqual(item.Marca, dto.Marca);
        Assert.AreEqual(item.Modelo, dto.Modelo);
        Assert.AreEqual(item.Categoria, dto.Categoria);

    }
}