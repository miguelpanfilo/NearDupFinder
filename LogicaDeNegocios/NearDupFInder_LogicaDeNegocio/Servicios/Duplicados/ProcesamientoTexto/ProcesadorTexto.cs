namespace NearDupFinder_LogicaDeNegocio.Servicios.Duplicados.ProcesamientoTexto;

public class ProcesadorTexto : IProcesadorTexto
{
    public string[] AplicarStopwords(string[] tokensEntrada)
    {
        if (tokensEntrada == null || tokensEntrada.Length == 0)
            return Array.Empty<string>();

        return tokensEntrada
            .Where(t => !_stopwords.Contains(t))
            .ToArray();
        
    }
    private static readonly HashSet<string> _stopwords = new(StringComparer.OrdinalIgnoreCase)
    {
        "el", "la", "los", "las", "de", "y", "en", "un", "una", "para", "por"
    };
}
