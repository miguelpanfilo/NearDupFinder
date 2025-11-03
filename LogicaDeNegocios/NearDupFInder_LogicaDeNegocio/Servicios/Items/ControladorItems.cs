using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlClusters;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;
using NearDupFInder_LogicaDeNegocio.Servicios.Clusters;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;

namespace NearDupFinder_LogicaDeNegocio.Servicios.Items;

public class ControladorItems
{
    private readonly GestorCatalogos _gestorCatalogos;
    private readonly ControladorDuplicados _controladorDuplicados;
    private readonly GestorControlClusters _gestorControlClusters;
    private readonly GestorAuditoria _gestorAuditoria;
    private readonly HashSet<int> _idsItemsGlobal;
    private readonly GestorItems _gestorItems;
    
    public ControladorItems(
        GestorItems gestorItems,
        GestorCatalogos gestorCatalogos,
        ControladorDuplicados controladorDuplicados,
        GestorControlClusters gestorControlClusters,
        GestorAuditoria gestorAuditoria,
        HashSet<int> idsItemsGlobal)
    {
        _gestorItems=gestorItems;
        _gestorCatalogos = gestorCatalogos;
        _controladorDuplicados = controladorDuplicados;
        _gestorControlClusters = gestorControlClusters;
        _gestorAuditoria = gestorAuditoria;
        _idsItemsGlobal = idsItemsGlobal;
    }
    
    public Item  CrearItem(DatosCrearItem datos)
    {
        if (string.IsNullOrWhiteSpace(datos.Titulo) || string.IsNullOrWhiteSpace(datos.Descripcion))
            throw new ExcepcionItem("Título y Descripción son obligatorios.");

        var catalogo = _gestorCatalogos.ObtenerCatalogoPorId(datos.IdCatalogo)
                       ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={datos.IdCatalogo}).");

        var item = new Item()
        {
            Titulo = datos.Titulo,
            Descripcion = datos.Descripcion,
            Categoria = datos.Categoria,
            Marca = datos.Marca,
            Modelo = datos.Modelo
        };

        if (datos.IdImportado is int idImportado)
        {
            if (_gestorItems.IdExisteEnListaDeIdGlobal(idImportado))
                throw new ExcepcionItem($"El Id importado {idImportado} ya existe.");
            item.ModificarIdEnCasoDeImportacion(idImportado);
            _idsItemsGlobal.Add(idImportado);
        }
        else
        {
            _gestorItems.AsegurarIdUnicoPublic(item);
        }

        catalogo.AgregarItem(item);

        _gestorAuditoria.RegistrarLog(
            EntradaDeLog.AccionLog.AltaItem,
            $"Item agregado: '{item.Titulo}' (Id={item.Id}) en catálogo '{catalogo.Titulo}' (Id={catalogo.Id})."
        );

        _controladorDuplicados.ProcesarDuplicados(catalogo.Id, item.Id);
        
        return item;
    }
    
    public void ActualizarItemEnCatalogo(DatosActualizarItem itemDtoActualizar)
    {
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorId(itemDtoActualizar.IdCatalogo)
                       ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={itemDtoActualizar.IdCatalogo}).");

        var item = catalogo.ObtenerItemPorId(itemDtoActualizar.IdItem)
                   ?? throw new ExcepcionItem($"Ítem no encontrado (Id={itemDtoActualizar.IdItem}).");

        if (itemDtoActualizar.Titulo is not null)
            item.EditarTitulo(itemDtoActualizar.Titulo);
        if (itemDtoActualizar.Descripcion is not null)
            item.EditarDescripcion(itemDtoActualizar.Descripcion);
        if (itemDtoActualizar.Categoria is not null)
            item.EditarCategoria(itemDtoActualizar.Categoria);
        if (itemDtoActualizar.Marca is not null)
            item.EditarMarca(itemDtoActualizar.Marca);
        if (itemDtoActualizar.Modelo is not null)
            item.EditarModelo(itemDtoActualizar.Modelo);
        
        
        _gestorAuditoria.RegistrarLog(
            EntradaDeLog.AccionLog.EditarItem,
            $"Ítem actualizado (Id={item.Id}) en catálogo '{catalogo.Titulo}' (Id={catalogo.Id})."
        );
    }
    
    public void EliminarItem(DatosEliminarItem itemDtoAEliminar)
    {
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorId(itemDtoAEliminar.IdCatalogo)
                       ?? throw new ExcepcionCatalogo("El catálogo no existe.");

        var itemAEliminar = catalogo.Items.FirstOrDefault(i => i.Id == itemDtoAEliminar.IdItem)
                            ?? throw new ExcepcionItem("El item no existe en el catálogo.");
        
        _gestorControlClusters.BorrarItemDelCluster(new DatosRemoverItemCluster(itemDtoAEliminar.IdItem, catalogo.Id));
        catalogo.EliminarItem(itemAEliminar);
        

        _controladorDuplicados.EliminarDuplicadosPrevios(itemAEliminar);
        _controladorDuplicados.ActualizarEstadoDuplicadosEnCatalogo(catalogo);

        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.EliminarItem,
            $"Item eliminado: '{itemAEliminar.Titulo}' del catálogo '{catalogo.Titulo}'");
    }
    
    
}
