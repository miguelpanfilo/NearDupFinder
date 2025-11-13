using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorCatalogo;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;
using NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;
using NearDupFinder_LogicaDeNegocio.Servicios.Items;

namespace NearDupFinder_LogicaDeNegocio.Servicios.Importacion;

public readonly struct Fila(
    string id,
    string titulo,
    string marca,
    string modelo,
    string descripción,
    string categoria,
    string catalogo)
{
    public string Id { get; } = id;
    public string Titulo { get; } = titulo;
    public string Marca { get; } = marca;
    public string Modelo { get; } = modelo;
    public string Descripción { get; } = descripción;
    public string Categoría { get; } = categoria;
    public string Catalogo { get; } = catalogo;
}

public class GestorLectorCsv(GestorCatalogos gestorCatalogos, GestorItems gestorItems, ControladorItems controladorItems)
{
    public List<string> Titulos { get; private set; } = [];
    public int CantidadDeFilas { get; private set; }
    public List<Fila> Filas { get; private set; } = [];

    public void LeerCsv(List<string> titulos, int cantidad, List<Fila> filas)
    {
        Titulos = titulos;
        CantidadDeFilas = cantidad;
        Filas = filas;
    }

    public void Limpiar()
    {
        Titulos = [];
        CantidadDeFilas = 0;
        Filas = [];
    }

    public void ImportarItems()
    {
        foreach (Fila fila in Filas)
        {
            if (!VerificarOAgregarCatalogoValido(fila.Catalogo))
                continue;

            if (ExisteId(fila.Id))
                continue;
            
            var catalogo = gestorCatalogos.ObtenerCatalogoPorTitulo(fila.Catalogo);

            int? idImportado = null;
            if (IdEsValido(fila.Id) && int.TryParse(fila.Id, out var parsed))
                idImportado = parsed;

            if(!ValidarTituloYDescripcion(fila.Titulo,fila.Descripción))
                continue;
            
            var dto = new DatosCrearItem(
                IdCatalogo: catalogo!.Id,
                Titulo: fila.Titulo,
                Descripcion: fila.Descripción,
                Categoria: fila.Categoría,
                Marca: fila.Marca,
                Modelo: fila.Modelo,
                IdImportado: idImportado
            );
            
            controladorItems.CrearItem(dto);
        }
    }

    private bool ValidarTituloYDescripcion(string filaTitulo, string filaDescripción)
    {
        if(string.IsNullOrEmpty(filaTitulo) || string.IsNullOrWhiteSpace(filaDescripción))
            return false;
        
        return true;
    }

    private bool VerificarOAgregarCatalogoValido(string? nombreCatalogo)
    {
        if (string.IsNullOrWhiteSpace(nombreCatalogo))
            return false;

        var catalogo = gestorCatalogos.ObtenerCatalogoPorTitulo(nombreCatalogo);
        if (catalogo is null)
        {
            try
            {
                gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear(nombreCatalogo));
            }
            catch
            {
                return false;
            }
        }

        return true;
    }

    private bool ExisteId(string id)
    {
        if (IdEsValido(id))
        {
            bool idYaExiste = gestorItems.ExisteItemConEseId(int.Parse(id));
            if (idYaExiste)
                return true;
        }

        return false;
    }

    private bool IdEsValido(string id)
    {
        return !string.IsNullOrWhiteSpace(id) && int.Parse(id) != 0;
    }
}