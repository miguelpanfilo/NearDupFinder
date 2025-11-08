using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_Interfaces;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorCatalogo;

namespace NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;

public class GestorCatalogos
{
    private readonly IRepositorioCatalogos _repositorioCatalogos;

    public GestorCatalogos(IRepositorioCatalogos repositorioCatalogos)
    {
        _repositorioCatalogos = repositorioCatalogos;
    }

    public void CrearCatalogo(DatosCatalogoCrear datosCatalogoCrear)
    {
        if (ElTituloYaEstaRegistrado(datosCatalogoCrear.Titulo))
            throw new ExcepcionCatalogo($"Ya existe un catálogo con el título '{datosCatalogoCrear.Titulo}'.");

        var nuevoCatalogo = new Catalogo(datosCatalogoCrear.Titulo);
        CambiarDescripcionCatalogo(nuevoCatalogo, datosCatalogoCrear.Descripcion!);

        _repositorioCatalogos.Agregar(nuevoCatalogo);
        _repositorioCatalogos.GuardarCambios();
    }

    public void BorrarCatalogo(DatosCatalogoEliminar datosCatalogoEliminar)
    {
        var catalogoAEliminar = ObtenerCatalogoPorId(datosCatalogoEliminar.Id)
                                ?? throw new ExcepcionCatalogo($"No existe un catálogo con Id={datosCatalogoEliminar.Id}");

        _repositorioCatalogos.Eliminar(catalogoAEliminar);
        _repositorioCatalogos.GuardarCambios();
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

        _repositorioCatalogos.Actualizar(catalogo);
        _repositorioCatalogos.GuardarCambios();
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
        var catalogos = _repositorioCatalogos.ObtenerTodos();
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
        return _repositorioCatalogos.ObtenerPorId(id);
    }

    public Catalogo? ObtenerCatalogoPorTitulo(string? titulo)
    {
        return _repositorioCatalogos.ObtenerPorTitulo(titulo!);
    }

    public int CantidadDeCatalogos()
    {
        return _repositorioCatalogos.ObtenerTodos().Count;
    }

    private bool ElTituloYaEstaRegistrado(string titulo)
    {
        return _repositorioCatalogos.ObtenerPorTitulo(titulo) is not null;
    }
}
