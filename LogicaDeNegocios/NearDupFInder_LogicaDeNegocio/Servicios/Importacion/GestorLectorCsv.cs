using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
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
    string catalogo,
    string rutaImagen,
    string precio,
    string stock)
{
    public string Id { get; } = id;
    public string Titulo { get; } = titulo;
    public string Marca { get; } = marca;
    public string Modelo { get; } = modelo;
    public string Descripción { get; } = descripción;
    public string Categoría { get; } = categoria;
    public string Catalogo { get; } = catalogo;
    public string RutaImagen { get; } = rutaImagen;
    public string Precio { get; } = precio;
    public string Stock { get; } = stock;
}
public class GestorLectorCsv(GestorCatalogos gestorCatalogos, GestorItems gestorItems, ControladorItems controladorItems)
{
    public List<string> Titulos { get; private set; } = [];
    public int CantidadDeFilas { get; private set; }
    public List<Fila> Filas { get; private set; } = [];
    
    private const int MaximoDeFilas = 15;

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
        int importados = 0;

        foreach (Fila fila in Filas)
        {
            if (importados >= MaximoDeFilas)
                break;

            if (!VerificarOAgregarCatalogoValido(fila.Catalogo))
                continue;

            if (ExisteId(fila.Id))
                continue;

            var catalogo = gestorCatalogos.ObtenerCatalogoPorTitulo(fila.Catalogo);

            int? idImportado = null;
            if (IdEsValido(fila.Id) && int.TryParse(fila.Id, out var parsed))
                idImportado = parsed;

            if (!ValidarTituloYDescripcion(fila.Titulo, fila.Descripción))
                continue;

            string? imagenBase64 = ObtenerImagenBase64DesdeRuta(fila.RutaImagen);

            int? precioImportado = ParseEnteroPositivoONull(fila.Precio);

            int stockImportado = ParseEnteroPositivoOZero(fila.Stock);

            var dto = new DatosCrearItem(
                IdCatalogo: catalogo!.Id,
                Titulo: fila.Titulo,
                Descripcion: fila.Descripción,
                Categoria: fila.Categoría,
                Marca: fila.Marca,
                Modelo: fila.Modelo,
                IdImportado: idImportado,
                ImagenBase64: imagenBase64,
                Precio: precioImportado,
                Stock: stockImportado
            );

            controladorItems.CrearItem(dto);
            importados++;
        }
    }

    public void ImportarDesdeContenido(string contenidoCsv)
    {
        using var lector = new StringReader(contenidoCsv);

        var configuracion = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim,
            BadDataFound = null,
            MissingFieldFound = null
        };

        using var csv = new CsvReader(lector, configuracion);

        if (!csv.Read())
            throw new InvalidOperationException("El archivo CSV está vacío.");

        csv.ReadHeader();
        if (csv.HeaderRecord is null
            || csv.HeaderRecord.Length == 0
            || csv.HeaderRecord.All(h => string.IsNullOrWhiteSpace(h)))
        {
            throw new InvalidOperationException("El CSV no contiene encabezados.");
        }


        List<string> titulos = csv.HeaderRecord.ToList();
        List<Fila> filas = new List<Fila>();
        int total = 0;

        while (csv.Read())
        {
            total++;

            if (filas.Count >= MaximoDeFilas)
                continue;

            string id = csv.TryGetField("id", out string idLeido) ? idLeido : string.Empty;
            string titulo = csv.TryGetField("titulo", out string tituloLeido) ? tituloLeido : string.Empty;
            string marca = csv.TryGetField("marca", out string marcaLeida) ? marcaLeida : string.Empty;
            string modelo = csv.TryGetField("modelo", out string modeloLeido) ? modeloLeido : string.Empty;
            string descripcion = csv.TryGetField("descripción", out string descripcionLeida) ? descripcionLeida : string.Empty;
            string categoria = csv.TryGetField("categoría", out string categoriaLeida) ? categoriaLeida : string.Empty;
            string catalogo = csv.TryGetField("catalogo", out string catalogoLeido) ? catalogoLeido : string.Empty;
            string rutaImagen = csv.TryGetField("imagen", out string imagenLeida) ? imagenLeida : string.Empty;
            string precio = csv.TryGetField("precio", out string precioLeido) ? precioLeido : string.Empty;
            string stock = csv.TryGetField("stock", out string stockLeido) ? stockLeido : string.Empty;

            filas.Add(new Fila(
                id!,
                titulo!,
                marca!,
                modelo!,
                descripcion!,
                categoria!,
                catalogo!,
                rutaImagen!,
                precio!,
                stock!
            ));
        }

        LeerCsv(titulos, total, filas);
        ImportarItems();
        Limpiar();
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
    
    private string? ObtenerImagenBase64DesdeRuta(string? ruta)
    {
        if (string.IsNullOrWhiteSpace(ruta))
            return null;

        try
        {
            if (!File.Exists(ruta))
                return null;

            byte[] bytes = File.ReadAllBytes(ruta);
            string base64 = Convert.ToBase64String(bytes);

            Imagen.CrearDesdeBase64(base64);

            return base64;
        }
        catch
        {
            return null;
        }
    }

    private int? ParseEnteroPositivoONull(string valor)
    {
        if (int.TryParse(valor, out var parsed) && parsed >= 0)
            return parsed;

        return null;
    }

    private int ParseEnteroPositivoOZero(string valor)
    {
        if (int.TryParse(valor, out var parsed) && parsed >= 0)
            return parsed;

        return 0;
    }
}