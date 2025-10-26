using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlLectorCsv;

namespace NearDupFinder_LogicaDeNegocio.Servicios.Importacion;

public class ControladorLectorCsv
{
    private GestorLectorCsv _gestorLectorCsv;

    public ControladorLectorCsv(GestorLectorCsv gestorLectorCsv)
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