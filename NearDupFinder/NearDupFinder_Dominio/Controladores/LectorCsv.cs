using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Dominio.Controladores;

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
    public string Marca { get;} = marca;
    public string Modelo { get;} = modelo;
    public string Descripción { get;} = descripción;
    public string Categoría { get;} = categoria;
    public string Catalogo { get;} = catalogo;
}

public class LectorCsv(Sistema sistema)
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

            if(ExisteId(fila.Id))
                continue;
            
            try
            {
                Item nuevoItem = new Item(fila.Titulo,fila.Descripción,fila.Marca,fila.Modelo,fila.Categoría);
                
                if(IdEsValido(fila.Id))
                    nuevoItem.ModificarId(int.Parse(fila.Id));
                
                sistema.AltaItemConAltaDuplicados(fila.Catalogo, nuevoItem);
            }
            catch
            {
                continue;
            }
        }
    }

    private bool VerificarOAgregarCatalogoValido(string nombreCatalogo)
    {
        if (string.IsNullOrWhiteSpace(nombreCatalogo))
            return false;
            
        var catalogo = sistema.ObtenerCatalogoPorTitulo(nombreCatalogo);
        if(catalogo is null)
        {
            try
            {
                catalogo = new Catalogo(nombreCatalogo);
            }
            catch
            {
                return false;
            }
            sistema.AgregarCatalogo(catalogo);
        }

        return true;
    }

    private bool ExisteId(string id)
    {
        if (IdEsValido(id))
        {
            bool idYaExiste = sistema.IdExisteEnListaDeIdGlobal(int.Parse(id));
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

