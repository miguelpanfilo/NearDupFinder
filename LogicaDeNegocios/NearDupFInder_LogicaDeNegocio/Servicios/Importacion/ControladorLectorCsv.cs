using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlLectorCsv;

namespace NearDupFinder_LogicaDeNegocio.Servicios.Importacion;

public class ControladorLectorCsv
{
    private GestorLectorCsv _gestorLectorCsv;

    public ControladorLectorCsv(GestorLectorCsv gestorLectorCsv)
    {
        _gestorLectorCsv = gestorLectorCsv;
    }
    
    public void ImportarItemsDesdeContenido(string contenidoCsv)
    {
        _gestorLectorCsv.ImportarDesdeContenido(contenidoCsv);
    }
}