namespace NearDupFinder_LogicaDeNegocio.Servicios.Duplicados.ProcesamientoTexto;

public interface IProcesadorTexto
{ 
    string[] AplicarStopwords(string[] tokensEntrada);
    string[] AplicarStemming(string[] tokensEntrada);
    string[] AplicarStopwordsYStemming(string[] tokensEntrada);

    
}