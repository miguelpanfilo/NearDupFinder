using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;

namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaDuplicados;

public class DatosParDuplicado(
    int idItemAComparar,
    int idItemPosibleDuplicado,
    string tituloItemAComparar,
    string tituloItemPosibleDuplicado,
    string descripcionItemAComparar,
    string descripcionItemPosibleDuplicado,
    string marcaItemAComparar,
    string modeloItemAComparar,
    string marcaItemPosibleDuplicado,
    string modeloItemPosibleDuplicado,
    float score,
    string tipo,
    float jaccardTitulo,
    float jaccardDescripcion,
    float scoreMarca,
    float scoreModelo,
    string[] tokensCompartidosTitulo,
    string[] tokensCompartidosDescripcion,
    int idCatalogo)
{
    public readonly int IdItemAComparar = idItemAComparar;
    public readonly int IdItemPosibleDuplicado = idItemPosibleDuplicado;
    public readonly string TituloItemAComparar = tituloItemAComparar;
    public readonly string TituloItemPosibleDuplicado = tituloItemPosibleDuplicado;
    public readonly string  DescripcionItemAComparar = descripcionItemAComparar;
    public readonly string DescripcionItemPosibleDuplicado = descripcionItemPosibleDuplicado;
    public readonly string MarcaItemAComparar = marcaItemAComparar;
    public readonly string ModeloItemAComparar = modeloItemAComparar;
    public readonly string MarcaItemPosibleDuplicado = marcaItemPosibleDuplicado;
    public readonly string ModeloItemPosibleDuplicado = modeloItemPosibleDuplicado;
    public readonly float Score = score;
    public readonly string Tipo = tipo;
    public readonly float JaccardTitulo = jaccardTitulo;
    public readonly float JaccardDescripcion = jaccardDescripcion;
    public readonly float ScoreMarca = scoreMarca;
    public readonly float ScoreModelo = scoreModelo;
    public readonly string[] TokensCompartidosTitulo = tokensCompartidosTitulo;
    public readonly string[] TokensCompartidosDescripcion = tokensCompartidosDescripcion;
    public readonly int IdCatalogo = idCatalogo;

    

    public static DatosParDuplicado FromEntity (ParDuplicado parDuplicado)
    {
        string tipoDelParDuplicado;
        if (parDuplicado.Tipo == TipoDuplicado.AlertaDuplicado)
        {
            tipoDelParDuplicado = "POSIBLE DUPLICADO";
        }
        else
        {
            tipoDelParDuplicado = "DUPLICADO SUGERIDO";
        }

        return new DatosParDuplicado(
            parDuplicado.ItemAComparar.Id,
            parDuplicado.ItemPosibleDuplicado.Id,
            parDuplicado.ItemAComparar.Titulo!,
            parDuplicado.ItemPosibleDuplicado.Titulo!,
            parDuplicado.ItemAComparar.Descripcion!,
            parDuplicado.ItemPosibleDuplicado.Descripcion!,
            parDuplicado.ItemAComparar.Marca!,
            parDuplicado.ItemAComparar.Modelo!,
            parDuplicado.ItemPosibleDuplicado.Marca!,
            parDuplicado.ItemPosibleDuplicado.Modelo!,
            parDuplicado.Score,
            tipoDelParDuplicado,
            parDuplicado.JaccardTitulo,
            parDuplicado.JaccardDescripcion,
            parDuplicado.ScoreMarca,
            parDuplicado.ScoreModelo,
            parDuplicado.TokensCompartidosTitulo,
            parDuplicado.TokensCompartidosDescripcion,
            parDuplicado.IdCatalogo);
    }
}

