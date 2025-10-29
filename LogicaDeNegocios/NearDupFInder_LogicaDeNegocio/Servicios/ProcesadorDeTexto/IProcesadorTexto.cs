namespace NearDupFinder_LogicaDeNegocio.Servicios.ProcesadorDeTexto
{
    public interface IProcesadorTexto
    {
        string[] AplicarStopwords(string[] tokensEntrada);
        string[] AplicarStemming(string[] tokensEntrada);
        string[] AplicarStopwordsYStemming(string[] tokensEntrada);
    }
}