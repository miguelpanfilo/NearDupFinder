using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_Interfaces;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorCatalogo;

namespace NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;

public class GestorCatalogos
{
    private readonly IRepositorioCatalogos _repoCatalogos;
    private readonly IRepositorioClusters  _repoClusters;
    private readonly IRepositorioItems     _repoItems;

    public GestorCatalogos(
        IRepositorioCatalogos repoCatalogos,
        IRepositorioClusters repoClusters,
        IRepositorioItems repoItems)
    {
        _repoCatalogos = repoCatalogos;
        _repoClusters  = repoClusters;
        _repoItems     = repoItems;
    }

    public void CrearCatalogo(DatosCatalogoCrear datosCatalogoCrear)
    {
        if (ElTituloYaEstaRegistrado(datosCatalogoCrear.Titulo))
            throw new ExcepcionCatalogo($"Ya existe un catálogo con el título '{datosCatalogoCrear.Titulo}'.");

        var nuevoCatalogo = new Catalogo(datosCatalogoCrear.Titulo);
        CambiarDescripcionCatalogo(nuevoCatalogo, datosCatalogoCrear.Descripcion!);

        _repoCatalogos.Agregar(nuevoCatalogo);
        _repoCatalogos.GuardarCambios();
    }

    public void BorrarCatalogo(DatosCatalogoEliminar datosCatalogoEliminar)
    {
        var catalogoAEliminar = _repoCatalogos.ObtenerParaEliminacionPorId(datosCatalogoEliminar.Id)
                                ?? throw new ExcepcionCatalogo($"No existe un catálogo con Id={datosCatalogoEliminar.Id}");
        
        _repoClusters.LimpiarCanonicoPorCatalogo(catalogoAEliminar.Id); // Clusters.CanonicoId = NULL
        _repoItems.OrfanearPorCatalogo(catalogoAEliminar.Id);
        
        _repoCatalogos.LimpiarSeguimiento();
        
        _repoCatalogos.Eliminar(catalogoAEliminar);
        _repoCatalogos.GuardarCambios();
    }

    public void ModificarCatalogo(DatosCatalogoEditar datosCatalogoEditar)
    {
        var catalogo = ObtenerCatalogoPorId(datosCatalogoEditar.Id)
                       ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={datosCatalogoEditar.Id})");

        if (datosCatalogoEditar.Titulo is not null &&
            !catalogo.Titulo.Equals(datosCatalogoEditar.Titulo, StringComparison.OrdinalIgnoreCase))
            CambiarTituloCatalogo(catalogo, datosCatalogoEditar.Titulo);

        if (datosCatalogoEditar.Descripcion is not null)
            CambiarDescripcionCatalogo(catalogo, datosCatalogoEditar.Descripcion);

        _repoCatalogos.Actualizar(catalogo);
        _repoCatalogos.GuardarCambios();
    }

    private void CambiarTituloCatalogo(Catalogo catalogo, string titulo)
    {
        if (ElTituloYaEstaRegistrado(titulo))
            throw new ExcepcionCatalogo($"Ya existe un catálogo con el título '{titulo}'.");

        catalogo.CambiarTitulo(titulo);
    }

    private void CambiarDescripcionCatalogo(Catalogo catalogo, string descripcion)
    {
        catalogo.CambiarDescripcion(descripcion);
    }

    public IReadOnlyCollection<DatosPublicosCatalogo> ObtenerCatalogos()
    {
        var catalogos = _repoCatalogos.ObtenerTodos();
        return catalogos.Select(DatosPublicosCatalogo.FromEntity).ToList();
    }

    public IReadOnlyCollection<Item> ObtenerItemsDelCatalogo(int id)
    {
        var catalogo = ObtenerCatalogoPorId(id)
                       ?? throw new ExcepcionCatalogo("El id no corresponde con ningún catálogo");

        return catalogo.Items;
    }

    public DatosPublicosCatalogo? ObtenerCatalogoDtoPorId(int id)
    {
        var catalogo = ObtenerCatalogoPorId(id);
        return catalogo is null ? null : DatosPublicosCatalogo.FromEntity(catalogo);
    }

    public DatosPublicosCatalogo? ObtenerCatalogoDtoPorTitulo(string? titulo)
    {
        var catalogo = ObtenerCatalogoPorTitulo(titulo);
        return catalogo is null ? null : DatosPublicosCatalogo.FromEntity(catalogo);
    }

    public Catalogo? ObtenerCatalogoPorId(int id)
    {
        return _repoCatalogos.ObtenerPorId(id);
    }

    public Catalogo? ObtenerCatalogoPorTitulo(string? titulo)
    {
        return _repoCatalogos.ObtenerPorTitulo(titulo!);
    }

    public int CantidadDeCatalogos()
    {
        return _repoCatalogos.ObtenerTodos().Count;
    }

    private bool ElTituloYaEstaRegistrado(string titulo)
    {
        return _repoCatalogos.ObtenerPorTitulo(titulo) is not null;
    }
}
