namespace NearDupFinder_Dominio.Clases;

public enum TipoDuplicado
{
    AlertaDuplicado,
    PosibleDuplicado
}
public class ParDuplicado
{
    public int Id { get; set; }
    public Item ItemAComparar { get; set; }
    public Item ItemPosibleDuplicado{ get; set; }
    public float Score { get; set; }
    public TipoDuplicado Tipo { get; set; }
    public float JaccardTitulo{ get; set; }
    public float JaccardDescripcion { get; set; }
    public int ScoreMarca { get; set; }
    public int ScoreModelo { get; set; }
    public string[] TokensCompartidosTitulo{ get; set; }
    public string[] TokensCompartidosDescripcion{ get; set; }
    
    public int IdCatalogo { get; set; }

    public ParDuplicado()
    {
    }
    public ParDuplicado(Item itemAComparar,Item itemPosibleDuplicado, float score, TipoDuplicado tipo, float jaccardTitulo, float jaccardDescripcion, int scoreMarca, int scoreModelo, string[] tokensCompartidosTitulo, string[] tokensCompartidosDescripcion, int idCatalogo)
    {
        ItemAComparar = itemAComparar;
        ItemPosibleDuplicado = itemPosibleDuplicado;
        Score = score;
        Tipo = tipo;
        JaccardTitulo = jaccardTitulo;
        JaccardDescripcion = jaccardDescripcion;
        ScoreMarca = scoreMarca;
        ScoreModelo = scoreModelo;
        TokensCompartidosTitulo = tokensCompartidosTitulo;
        TokensCompartidosDescripcion = tokensCompartidosDescripcion;
        IdCatalogo = idCatalogo;
    }
}