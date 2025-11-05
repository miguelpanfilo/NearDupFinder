using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados.ProcesamientoTexto;

namespace NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;

public enum TipoDuplicado
{
    AlertaDuplicado,
    PosibleDuplicado
}

public struct ItemTokenizado
{
    public string[] TokenTitulo { get; init; }
    public string[] TokenDescripcion { get; init; }
}

public struct ItemNormalizado
{
    public string TituloNormalizado { get; init; }
    public string DescripcionNormalizada { get; init; }
    public string MarcaNormalizada { get; init; }
    public string ModeloNormalizado { get; init; }
}

public readonly record struct ParDuplicado(
    Item ItemAComparar,
    Item ItemPosibleDuplicado,
    float Score,
    TipoDuplicado Tipo,
    float JaccardTitulo,
    float JaccardDescripcion,
    int ScoreMarca,
    int ScoreModelo,
    string[] TokensCompartidosTitulo,
    string[] TokensCompartidosDescripcion,
    int IdCatalogo
);

public class GestorDuplicados (IProcesadorTexto _procesadorTexto)
{
    private const string TokenPattern = @"\W+";
    private const float UmbralAlerta = 0.60f;
    private const float UmbralPosible = 0.75f;

    public List<ParDuplicado> DetectarDuplicados(Item itemA, Catalogo catalogo)
    {
        List<ParDuplicado> listaDuplicados = new List<ParDuplicado>();

        if (catalogo.CantidadItems() == 0)
            return listaDuplicados;

        ItemNormalizado itemNormalizadoA = NormalizarItem(itemA);
        ItemTokenizado itemTokenizadoA = TokenizarItem(itemNormalizadoA);

        foreach (Item itemB in catalogo.Items)
        {
            if (itemB.Id == itemA.Id)
                continue;
            int idCatalogo = catalogo.Id;
            ItemNormalizado itemNormalizadoB = NormalizarItem(itemB);
            ItemTokenizado itemTokenizadoB = TokenizarItem(itemNormalizadoB);

            float jaccardTitulo = CalcularJaccard(itemTokenizadoA.TokenTitulo, itemTokenizadoB.TokenTitulo);
            float jaccardDescripcion =
                CalcularJaccard(itemTokenizadoA.TokenDescripcion, itemTokenizadoB.TokenDescripcion);

            int scoreMarca = IgualdadBinaria(itemNormalizadoA.MarcaNormalizada, itemNormalizadoB.MarcaNormalizada);
            int scoreModelo = IgualdadBinaria(itemNormalizadoA.ModeloNormalizado, itemNormalizadoB.ModeloNormalizado);

            float score = CalcularScore(jaccardTitulo, jaccardDescripcion, scoreMarca, scoreModelo);

            if (score >= UmbralAlerta)
            {
                string[] tokensCompartidosTitulo =
                    itemTokenizadoA.TokenTitulo.Intersect(itemTokenizadoB.TokenTitulo).ToArray();
                string[] tokensCompartidosDescripcion = itemTokenizadoA.TokenDescripcion
                    .Intersect(itemTokenizadoB.TokenDescripcion).ToArray();

                TipoDuplicado tipo = score >= UmbralPosible
                    ? TipoDuplicado.PosibleDuplicado
                    : TipoDuplicado.AlertaDuplicado;

                ParDuplicado duplicado = new ParDuplicado(
                    itemA, itemB, score, tipo,
                    jaccardTitulo, jaccardDescripcion,
                    scoreMarca, scoreModelo,
                    tokensCompartidosTitulo, tokensCompartidosDescripcion, idCatalogo
                );

                listaDuplicados.Add(duplicado);
            }
        }

        listaDuplicados = listaDuplicados
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.ItemPosibleDuplicado.Id)
            .ToList();

        return listaDuplicados;
    }

    private ItemNormalizado NormalizarItem(Item item)
    {
        string tituloNormalizado = Normalizar(item.Titulo);
        string descripcionNormalizada = Normalizar(item.Descripcion);

        if (string.IsNullOrEmpty(tituloNormalizado) || string.IsNullOrEmpty(descripcionNormalizada))
        {
            throw new InvalidOperationException("El título y la descripción no pueden quedar vacío tras normalizar.");
        }

        string marcaNormalizada = Normalizar(item.Marca);
        string modeloNormalizada = Normalizar(item.Modelo);

        return new ItemNormalizado
        {
            TituloNormalizado = tituloNormalizado,
            DescripcionNormalizada = descripcionNormalizada,
            MarcaNormalizada = marcaNormalizada,
            ModeloNormalizado = modeloNormalizada,
        };
    }

    private string Normalizar(string? texto)
    {
        if (string.IsNullOrEmpty(texto))
            return string.Empty;

        texto = texto.ToLowerInvariant();
        texto = texto.Replace("á", "a")
            .Replace("é", "e")
            .Replace("í", "i")
            .Replace("ó", "o")
            .Replace("ú", "u")
            .Replace("ñ", "n")
            .Replace("ü", "u");

        texto = Regex.Replace(texto, @"[^a-z0-9]", " ");
        texto = Regex.Replace(texto, @"\s+", " ").Trim();
        return texto;
    }

    private ItemTokenizado TokenizarItem(ItemNormalizado item)
    {
        string[] tokensTitulo = Tokenizar(item.TituloNormalizado);
        string[] tokensDescripcion = Tokenizar(item.DescripcionNormalizada);

        tokensTitulo = SacarTildesDeTokens(tokensTitulo);
        tokensDescripcion = SacarTildesDeTokens(tokensDescripcion);

        tokensTitulo = _procesadorTexto.AplicarStopwordsYStemming(tokensTitulo);
        tokensDescripcion = _procesadorTexto.AplicarStopwordsYStemming(tokensDescripcion);

        return new ItemTokenizado
        {
            TokenTitulo = tokensTitulo,
            TokenDescripcion = tokensDescripcion
        };
    }
    private string QuitarTildes(string palabra)
    {
        if (string.IsNullOrEmpty(palabra))
            return palabra;

        return palabra
            .Replace("á", "a").Replace("Á", "A")
            .Replace("é", "e").Replace("É", "E")
            .Replace("í", "i").Replace("Í", "I")
            .Replace("ó", "o").Replace("Ó", "O")
            .Replace("ú", "u").Replace("Ú", "U")
            .Replace("ü", "u").Replace("Ü", "U")
            .Replace("ñ", "n").Replace("Ñ", "N");
    }

    private string[] SacarTildesDeTokens(string[] tokens)
    {
        if (tokens == null || tokens.Length == 0)
            return Array.Empty<string>();

        for (int i = 0; i < tokens.Length; i++)
        {
            tokens[i] = QuitarTildes(tokens[i]);
        }

        return tokens;
    }


    private static string[] Tokenizar(string texto)
    {
        return Regex.Split(texto, TokenPattern)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Where(t => t.Length > 1)
            .ToArray();
    }

    private int CalcularNumTokensUnion(string[] tokens1, string[] tokens2)
    {
        return tokens1.Union(tokens2).Count();
    }

    private int CalcularNumTokensInterseccion(string[] tokens1, string[] tokens2)
    {
        return tokens1.Intersect(tokens2).Count();
    }

    private float CalcularJaccard(string[] tokens1, string[] tokens2)
    {
        float numTokensUnion = CalcularNumTokensUnion(tokens1, tokens2);
        if (numTokensUnion == 0)
            return 0;

        float numTokensInterseccion = CalcularNumTokensInterseccion(tokens1, tokens2);
        float valorJaccard = numTokensInterseccion / numTokensUnion;

        return valorJaccard;
    }
    private float CalcularScore(float jaccardTitulo, float jaccardDescripcion, float marcaEq, float modeloEq)
    {
        float constanteDeTitulo = 0.45f;
        float constanteDeDescripcion = 0.35f;
        float constanteDeMarca = 0.10f;
        float constanteDeModelo = 0.10f;

        float score = constanteDeTitulo * jaccardTitulo + constanteDeDescripcion * jaccardDescripcion +
                      constanteDeMarca * marcaEq + constanteDeModelo * modeloEq;

        return score;
    }

    private static int IgualdadBinaria(string textoA, string textoB)
    {
        if (string.IsNullOrEmpty(textoA) || string.IsNullOrEmpty(textoB))
            return 0;

        return string.Equals(textoA, textoB) ? 1 : 0;
    }
}