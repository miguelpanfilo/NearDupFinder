namespace NearDupFInder_LogicaDeNegocio.DTOs.ParaDuplicados;

public record DatosDuplicados(
    int IdCatalogo,
    int IdItemAComparar,
    int IdItemPosibleDuplicado
);