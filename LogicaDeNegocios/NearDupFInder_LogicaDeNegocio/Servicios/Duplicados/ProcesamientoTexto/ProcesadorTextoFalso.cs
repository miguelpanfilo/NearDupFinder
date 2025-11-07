namespace NearDupFinder_LogicaDeNegocio.Servicios.Duplicados.ProcesamientoTexto;

public class ProcesadorTextoFalso : IProcesadorTexto
{
    public string[] AplicarStopwords(string[] tokensEntrada) => tokensEntrada;
    public string[] AplicarStemming(string[] tokensEntrada) => tokensEntrada;
    public string[] AplicarStopwordsYStemming(string[] tokensEntrada) => tokensEntrada;
}
