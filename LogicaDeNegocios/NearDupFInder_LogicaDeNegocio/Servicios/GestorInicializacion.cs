using NearDupFinder_Almacenamiento;

namespace NearDupFinder_LogicaDeNegocio.Servicios;

public class GestorInicializacion
{
    private readonly AlmacenamientoDeDatos _almacenamiento;
    private bool _inicializado;

    public GestorInicializacion(AlmacenamientoDeDatos almacenamiento)
    {
        _almacenamiento = almacenamiento;
    }

    public void AsegurarInicializacion()
    {
        if (_inicializado)
            return;

        // Acá iría la carga inicial si la necesitás

        _inicializado = true;
    }
}