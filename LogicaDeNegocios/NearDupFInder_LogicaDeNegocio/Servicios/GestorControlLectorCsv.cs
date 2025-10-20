using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlLectorCsv;

namespace NearDupFinder_LogicaDeNegocio.Servicios;

public class GestorControlLectorCsv
{
    private LectorCsv _lectorCsv;

    public GestorControlLectorCsv(LectorCsv lectorCsv)
    {
        _lectorCsv = lectorCsv;
    }

    public void ImportarItemsDesdeCsv(DatosImportarCsv datosImportarCsv)
    {
        _lectorCsv.LeerCsv(datosImportarCsv.Titulos, datosImportarCsv.Cantidad, datosImportarCsv.Filas);
        _lectorCsv.ImportarItems();
        _lectorCsv.Limpiar();
    }
}