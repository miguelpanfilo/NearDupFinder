using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorCatalogo;

namespace NearDupFinder_LogicaDeNegocio.Servicios;

public class GestorCatalogos
{
    private AlmacenamientoDeDatos _almacenamientoDeDatos;

    public GestorCatalogos(AlmacenamientoDeDatos almacenamientoDeDatos)
    {
        _almacenamientoDeDatos = almacenamientoDeDatos;
    }

    public void CrearCatalogo(DatosCatalogoCrear datosCatalogoCrear)
    {
        if (ElTituloYaEstaRegistrado(datosCatalogoCrear.Titulo))
        {
            throw new ExcepcionCatalogo($"Ya existe un catálogo con el título '{datosCatalogoCrear.Titulo}'.");
        }

        var nuevoCatalogo = datosCatalogoCrear.ToEntity();

        AgregarALaListaDeCatalogo(nuevoCatalogo);
    }

    public void BorrarCatalogo(DatosCatalogoEliminar datosCatalogoEliminar)
    {
        var catalogoAEliminar = ObtenerCatalogoPorId(datosCatalogoEliminar.Id) ??
                                throw new ExcepcionCatalogo($"No existe un catálogo con Id={datosCatalogoEliminar.Id}");

        RemoverDeLaListaDeCatalogos(catalogoAEliminar!);
    }

    public void ModificarCampos(DatosCatalogoEditar datosCatalogoEditar)
    {
        var catalogo = ObtenerCatalogoPorId(datosCatalogoEditar.Id)
                       ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={datosCatalogoEditar.Id})");

        if (datosCatalogoEditar.Titulo is not null &&
            !catalogo.Titulo.Equals(datosCatalogoEditar.Titulo, StringComparison.OrdinalIgnoreCase))
            CambiarTituloCatalogo(catalogo, datosCatalogoEditar.Titulo);

        if (datosCatalogoEditar.Descripcion is not null)
            CambiarDescripcionCatalogo(catalogo, datosCatalogoEditar.Descripcion);
    }

    private void CambiarTituloCatalogo(Catalogo catalogo, string titulo)
    {
        if (ElTituloYaEstaRegistrado(titulo))
        {
            throw new ExcepcionCatalogo($"Ya existe un catálogo con el título '{titulo}'.");
        }

        catalogo.CambiarTitulo(titulo);
    }

    private void CambiarDescripcionCatalogo(Catalogo catalogo, string descripcion)
    {
        catalogo.CambiarDescripcion(descripcion);
    }

    private void RemoverDeLaListaDeCatalogos(Catalogo catalogo)
    {
        _almacenamientoDeDatos.RemoverCatalogo(catalogo);
    }

    private void AgregarALaListaDeCatalogo(Catalogo catalogo)
    {
        _almacenamientoDeDatos.AgregarCatalogo(catalogo);
    }

    public IReadOnlyCollection<Catalogo> Catalogos => _almacenamientoDeDatos.ObtenerCatalogos().AsReadOnly();

    public IReadOnlyCollection<Item> ObtenerItemsDelCatalogo(int id)
    {
        var catalogo = ObtenerCatalogoPorId(id);

        if (catalogo is null) throw new ExcepcionCatalogo("El id no corresponde con ningun catalogo");

        return catalogo.Items;
    }

    public Catalogo? ObtenerCatalogoPorId(int id)
    {
        return _almacenamientoDeDatos.ObtenerCatalogoPorId(id);
    }


    public Catalogo? ObtenerCatalogoPorTitulo(string? titulo)
    {
        return _almacenamientoDeDatos.ObtenerCatalogoPorTitulo(titulo);
    }

    public int CantidadDeCatalogos()
    {
        return _almacenamientoDeDatos.ObtenerCatalogos().Count;
    }

    private bool ElTituloYaEstaRegistrado(string titulo)
    {
        if (_almacenamientoDeDatos.ObtenerCatalogoPorTitulo(titulo) is not null)
            return true;

        return false;
    }
}