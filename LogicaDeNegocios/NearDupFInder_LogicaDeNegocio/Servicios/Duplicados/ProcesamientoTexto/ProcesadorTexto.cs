using Lucene.Net.Tartarus.Snowball.Ext;
using System.Collections.Generic;
using System.Linq;
namespace NearDupFinder_LogicaDeNegocio.Servicios.Duplicados.ProcesamientoTexto;

public class ProcesadorTexto : IProcesadorTexto
{
    private readonly SpanishStemmer _stemmer = new();

    public string[] AplicarStopwords(string[] tokensEntrada)
    {
        if (tokensEntrada == null || tokensEntrada.Length == 0)
            return Array.Empty<string>();

        return tokensEntrada
            .Where(t => !_stopwords.Contains(t))
            .ToArray();
        
    }

    public string[] AplicarStemming(string[] tokensEntrada)
    {
        if (tokensEntrada == null || tokensEntrada.Length == 0)
            return Array.Empty<string>();

        var resultado = new List<string>();

        foreach (var token in tokensEntrada)
        {
            _stemmer.SetCurrent(token);
            _stemmer.Stem();
            resultado.Add(_stemmer.Current);
        }

        return resultado.Distinct().ToArray();
    }

    public string[] AplicarStopwordsYStemming(string[] tokensEntrada)
    {
        var sinStopwords = AplicarStopwords(tokensEntrada);
        return AplicarStemming(sinStopwords);
        
    }

    private static readonly HashSet<string> _stopwords = new(StringComparer.OrdinalIgnoreCase)
    {
        "el", "la", "los", "las", "de", "y", "en", "un", "una", "para", "por"
    };
}
