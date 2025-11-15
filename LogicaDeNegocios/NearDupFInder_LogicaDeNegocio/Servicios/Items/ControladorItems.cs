using System.Reflection;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaDuplicados;
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
    private readonly GestorItems _gestorItems;
    
    public ControladorItems(
        GestorItems gestorItems,
        GestorCatalogos gestorCatalogos,
        ControladorDuplicados controladorDuplicados,
        GestorControlClusters gestorControlClusters,
        GestorAuditoria gestorAuditoria)
    {
        _gestorItems = gestorItems;
        _gestorCatalogos = gestorCatalogos;
        _controladorDuplicados = controladorDuplicados;
        _gestorControlClusters = gestorControlClusters;
        _gestorAuditoria = gestorAuditoria;
    }
    
    public Item CrearItem(DatosCrearItem datos)
    {
        if (string.IsNullOrWhiteSpace(datos.Titulo) || string.IsNullOrWhiteSpace(datos.Descripcion))
            throw new ExcepcionItem("Título y Descripción son obligatorios.");

        var catalogo = _gestorCatalogos.ObtenerCatalogoPorId(datos.IdCatalogo)
                       ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={datos.IdCatalogo}).");

        var item = _gestorItems.CrearNuevoItem(datos);

        if (ElItemFueImportado(datos))
        {
            int idImportado = (int)datos.IdImportado;
            if (_gestorItems.ExisteItemConEseId(idImportado))
                throw new ExcepcionItem($"El Id importado {idImportado} ya existe.");
            
            item.ModificarIdEnCasoDeImportacion(idImportado);
        }
        else
        {
            _gestorItems.AsegurarIdUnico(item);
        }

        _gestorItems.AgregarItemACatalogo(catalogo, item);
        
        _gestorAuditoria.RegistrarLog(
            EntradaDeLog.AccionLog.AltaItem,
            $"Item agregado: '{item.Titulo}' en catálogo '{catalogo.Titulo}'."
        );

        _controladorDuplicados.ProcesarDuplicados(catalogo.Id, item.Id);
        _gestorItems.ActualizarItem(item);

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
        if (itemDtoActualizar.Stock is int nuevoStock)
        {
            if (nuevoStock < 0)
                throw new ExcepcionItem("El stock no puede ser negativo.");

            item.Stock = nuevoStock;
        }
        
        _gestorItems.ActualizarItem(item);
        _controladorDuplicados.ActualizarDuplicadosPara(
            new DatosActualizarDuplicados(itemDtoActualizar.IdCatalogo, itemDtoActualizar.IdItem)
        );
        _gestorAuditoria.RegistrarLog(
            EntradaDeLog.AccionLog.EditarItem,
            $"Ítem actualizado (Id={item.Titulo}) en catálogo '{catalogo.Titulo}'."
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

        _controladorDuplicados.EliminarDuplicadosPrevios(itemAEliminar, catalogo.Id);
        _controladorDuplicados.ActualizarEstadoDuplicadosEnCatalogo(catalogo);
        _gestorItems.EliminarItem(itemAEliminar);

        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.EliminarItem,
            $"Item eliminado: '{itemAEliminar.Titulo}' del catálogo '{catalogo.Titulo}'");
    }
    
    public List<DatosItemListaItems> ObtenerItemsDelCatalogo(int idCatalogo)
    {
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorId(idCatalogo)
                       ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={idCatalogo}).");

        return catalogo.Items
            .Select(DatosItemListaItems.FromEntity)
            .ToList();
    }

    private bool ElItemFueImportado(DatosCrearItem datos)
    {
        return datos.IdImportado is int idImportado;
    }
}
