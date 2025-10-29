using System.IO;
using System.Linq;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Snowball;
using Lucene.Net.Util;

namespace NearDupFinder_LogicaDeNegocio.Servicios.ProcesadorDeTexto
{
    public class ProcesadorTexto : IProcesadorTexto
    {
        private readonly Analyzer _analyzer;

        public ProcesadorTexto(Analyzer analyzer)
        {
            _analyzer = analyzer;
        }
        

        public string[] AplicarStopwords(string[] tokensEntrada)
        {
            if (tokensEntrada == null || tokensEntrada.Length == 0)
                return [];

            return tokensEntrada
                .Where(t => !_stopwords.Contains(t.ToLowerInvariant()))
                .ToArray();
        }

        public string[] AplicarStemming(string[] tokensEntrada)
        {
            if (tokensEntrada == null || tokensEntrada.Length == 0)
                return [];

            var texto = string.Join(" ", tokensEntrada);
            var resultado = new List<string>();

            using var reader = new StringReader(texto);
            using var tokenStream = _analyzer.GetTokenStream("contenido", reader);
            var termAttr = tokenStream.AddAttribute<Lucene.Net.Analysis.TokenAttributes.ICharTermAttribute>();

            tokenStream.Reset();
            while (tokenStream.IncrementToken())
            {
                resultado.Add(termAttr.ToString());
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
}