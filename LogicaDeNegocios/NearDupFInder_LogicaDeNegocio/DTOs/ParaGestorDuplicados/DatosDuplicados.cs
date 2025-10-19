namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorDuplicados;

public record DatosDuplicados(
    int IdCatalogo,
    int IdItemAComparar,
    int IdItemPosibleDuplicado
);