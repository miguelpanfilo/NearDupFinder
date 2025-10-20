using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;

namespace NearDupFinder_LogicaDeNegocio.Servicios;

public class GestorItems
{
    private readonly GestorCatalogos _gestorCatalogos;
    private readonly GestorControlDuplicados _gestorControlDuplicados;
    private readonly HashSet<int> _idsItemsGlobal;
    private readonly GestorAuditoria _gestorAuditoria;

    public GestorItems(
        GestorCatalogos gestorCatalogos,
        GestorControlDuplicados gestorControlDuplicados,
        GestorAuditoria gestorAuditoria,
        HashSet<int> idsItemsGlobal)
    {
        _gestorCatalogos = gestorCatalogos;
        _gestorControlDuplicados = gestorControlDuplicados;
        _gestorAuditoria = gestorAuditoria;
        _idsItemsGlobal = idsItemsGlobal;
    }


    public void CrearItem(DatosCrearItem datos)
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
            if (IdExisteEnListaDeIdGlobal(idImportado))
                throw new ExcepcionItem($"El Id importado {idImportado} ya existe.");
            item.ModificarIdEnCasoDeImportacion(idImportado);
            _idsItemsGlobal.Add(idImportado);
        }
        else
        {
            AsegurarIdUnico(item);
        }

        catalogo.AgregarItem(item);

        _gestorAuditoria.RegistrarLog(
            EntradaDeLog.AccionLog.AltaItem,
            $"Item agregado: '{item.Titulo}' (Id={item.Id}) en catálogo '{catalogo.Titulo}' (Id={catalogo.Id})."
        );

        _gestorControlDuplicados.ProcesarDuplicadosPorAlta(catalogo.Id, item.Id);
    }


    private void AsegurarIdUnico(Item item)
    {
        int idApropiado = item.Id;
        while (IdExisteEnListaDeIdGlobal(idApropiado))
            idApropiado++;
        item.AjustarId(idApropiado);
        _idsItemsGlobal.Add(idApropiado);
    }

    public bool IdExisteEnListaDeIdGlobal(int id)
    {
        return _idsItemsGlobal.Contains(id);
    }

    public int CantidadDeItemsGlobal()
    {
        return _idsItemsGlobal.Count;
    }

    public void ActualizarItemEnCatalogo(DatosActualizarItem datosActualizarItem)
    {
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorId(datosActualizarItem.IdCatalogo)
                       ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={datosActualizarItem.IdCatalogo}).");

        var item = catalogo.ObtenerItemPorId(datosActualizarItem.IdItem)
                   ?? throw new ExcepcionItem($"Ítem no encontrado (Id={datosActualizarItem.IdItem}).");

        if (datosActualizarItem.Titulo is not null)
            item.EditarTitulo(datosActualizarItem.Titulo);
        if (datosActualizarItem.Descripcion is not null)
            item.EditarDescripcion(datosActualizarItem.Descripcion);
        if (datosActualizarItem.Categoria is not null)
            item.EditarCategoria(datosActualizarItem.Categoria);
        if (datosActualizarItem.Marca is not null)
            item.EditarMarca(datosActualizarItem.Marca);
        if (datosActualizarItem.Modelo is not null)
            item.EditarModelo(datosActualizarItem.Modelo);

        _gestorAuditoria.RegistrarLog(
            EntradaDeLog.AccionLog.EditarItem,
            $"Ítem actualizado (Id={item.Id}) en catálogo '{catalogo.Titulo}' (Id={catalogo.Id})."
        );
    }

    public void EliminarItem(DatosEliminarItem datosEliminarItem)
    {
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorId(datosEliminarItem.IdCatalogo)
                       ?? throw new ArgumentException("El catálogo no existe.");

        var item = catalogo.Items.FirstOrDefault(i => i.Id == datosEliminarItem.IdItem)
                   ?? throw new ExcepcionItem("El item no existe en el catálogo.");

        catalogo.EliminarItem(item);

        _gestorControlDuplicados.EliminarDuplicadosPrevios(item);
        _gestorControlDuplicados.ActualizarEstadoDuplicadosEnCatalogo(catalogo);

        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.EliminarItem,
            $"Item eliminado: '{item.Titulo}' del catálogo '{catalogo.Titulo}'");
    }
}