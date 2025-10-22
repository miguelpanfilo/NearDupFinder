using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlLectorCsv;

namespace NearDupFinder_LogicaDeNegocio.Servicios;

public class GestorControlLectorCsv
{
    private GestorLectorCsv _gestorLectorCsv;

    public GestorControlLectorCsv(GestorLectorCsv gestorLectorCsv)
    {
        _gestorLectorCsv = gestorLectorCsv;
    }

    public void ImportarItemsDesdeCsv(DatosImportarCsv datosImportarCsv)
    {
        _gestorLectorCsv.LeerCsv(datosImportarCsv.Titulos, datosImportarCsv.Cantidad, datosImportarCsv.Filas);
        _gestorLectorCsv.ImportarItems();
        _gestorLectorCsv.Limpiar();
    }
}