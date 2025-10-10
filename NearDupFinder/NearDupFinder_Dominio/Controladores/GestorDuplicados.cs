using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Dominio.Controladores;

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

[ExcludeFromCodeCoverage]
public readonly record struct ParDuplicado(
    Item ItemA,
    Item ItemB,
    float Score,
    TipoDuplicado Tipo,
    float JaccardTitulo,
    float JaccardDescripcion,
    int ScoreMarca,
    int ScoreModelo,
    string[] TokensCompartidosTitulo,
    string[] TokensCompartidosDescripcion
);

public class GestorDuplicados
{ 
    private const string TokenPattern = @"\W+";
    private const float UmbralAlerta = 0.60f;
    private const float UmbralPosible = 0.75f;

    public List<ParDuplicado> DetectarDuplicados(Item? itemA, Catalogo? catalogo)
    {
        List<ParDuplicado> listaDuplicados = new List<ParDuplicado>();

        if (itemA is null || catalogo is null || catalogo.CantidadItems() == 0)
            return listaDuplicados;

        Item itemNormalizadoA = NormalizarItem(itemA);
        ItemTokenizado itemTokenizadoA = TokenizarItem(itemNormalizadoA);

        foreach (Item itemB in catalogo.Items)
        {
            if (itemB.Id == itemA.Id)
                continue;

            Item itemNormalizadoB = NormalizarItem(itemB);
            ItemTokenizado itemTokenizadoB = TokenizarItem(itemNormalizadoB);

            float jaccardTitulo = CalcularJaccard(itemTokenizadoA.TokenTitulo, itemTokenizadoB.TokenTitulo);
            float jaccardDescripcion = CalcularJaccard(itemTokenizadoA.TokenDescripcion, itemTokenizadoB.TokenDescripcion);

            int scoreMarca = IgualdadBinaria(itemNormalizadoA.Marca, itemNormalizadoB.Marca);
            int scoreModelo = IgualdadBinaria(itemNormalizadoA.Modelo, itemNormalizadoB.Modelo);

            float score = CalcularScore(jaccardTitulo, jaccardDescripcion, scoreMarca, scoreModelo);

            if (score >= UmbralAlerta)
            {
                string[] tokensCompartidosTitulo = itemTokenizadoA.TokenTitulo.Intersect(itemTokenizadoB.TokenTitulo).ToArray();
                string[] tokensCompartidosDescripcion = itemTokenizadoA.TokenDescripcion.Intersect(itemTokenizadoB.TokenDescripcion).ToArray();
                
                TipoDuplicado tipo = score >= UmbralPosible 
                    ? TipoDuplicado.PosibleDuplicado 
                    : TipoDuplicado.AlertaDuplicado;

                ParDuplicado duplicado = new ParDuplicado(
                    itemA, itemB, score, tipo,
                    jaccardTitulo, jaccardDescripcion,
                    scoreMarca, scoreModelo,
                    tokensCompartidosTitulo, tokensCompartidosDescripcion
                );
                
                listaDuplicados.Add(duplicado);
            }
        }

        listaDuplicados = listaDuplicados
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.ItemB.Id)
            .ToList();

        return listaDuplicados;
    }
    
    public Item NormalizarItem(Item item)
    {
        string tituloNormalizado = Normalizar(item.Titulo);
        string descripcionNormalizada = Normalizar(item.Descripcion);

        if (string.IsNullOrEmpty(tituloNormalizado) || string.IsNullOrEmpty(descripcionNormalizada))
        {
            throw new InvalidOperationException("El título y la descripción no pueden quedar vacío tras normalizar.");
        }

        string marcaNormalizada = Normalizar(item.Marca);
        string modeloNormalizada = Normalizar(item.Modelo);
        string categoriaNormalizada = Normalizar(item.Categoria);

        return new Item
        {
            Titulo = tituloNormalizado,
            Descripcion = descripcionNormalizada,
            Marca = marcaNormalizada,
            Modelo = modeloNormalizada,
            Categoria = categoriaNormalizada
        };
    }

    public string Normalizar(string texto)
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

    public ItemTokenizado TokenizarItem(Item item)
    {
        return new ItemTokenizado
        {
            TokenTitulo = Tokenizar(item.Titulo),
            TokenDescripcion = Tokenizar(item.Descripcion)
        };
    }

    private static string[] Tokenizar(string texto)
    {
        return Regex.Split(texto, TokenPattern)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Where(t => t.Length > 1)
            .ToArray();
    }

    public int CalcularNumTokensUnion(string[] tokens1, string[] tokens2)
    {
        return tokens1.Union(tokens2).Count();
    }

    public int CalcularNumTokensInterseccion(string[] tokens1, string[] tokens2)
    {
        return tokens1.Intersect(tokens2).Count();
    }

    public float CalcularJaccard(string[] tokens1, string[] tokens2)
    {
        float numTokensUnion = CalcularNumTokensUnion(tokens1, tokens2);
        if (numTokensUnion == 0)
            return 0;

        float numTokensInterseccion = CalcularNumTokensInterseccion(tokens1, tokens2);
        float valorJaccard = numTokensInterseccion / numTokensUnion;

        return valorJaccard;
    }

    private void ValidarValoresDentroDeLosRangosEsperados(float jaccardTitulo, float jaccardDescripcion, float marcaEq, float modeloEq)
    {
        if (jaccardTitulo < 0f || jaccardTitulo > 1f || jaccardDescripcion < 0f || jaccardDescripcion > 1f)
            throw new ArgumentOutOfRangeException();
        if (marcaEq is not (0f or 1f) || modeloEq is not (0f or 1f))
            throw new ArgumentOutOfRangeException();
    }

    public float CalcularScore(float jaccardTitulo, float jaccardDescripcion, float marcaEq, float modeloEq)
    {
        ValidarValoresDentroDeLosRangosEsperados(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq);
        
        float score = 0.45f * jaccardTitulo + 0.35f * jaccardDescripcion + 0.10f * marcaEq + 0.10f * modeloEq;

        return score;
    }

    private static int IgualdadBinaria(string a, string b)
    {
        if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))
            return 0;

        return string.Equals(a, b, StringComparison.Ordinal) ? 1 : 0;
    }
}