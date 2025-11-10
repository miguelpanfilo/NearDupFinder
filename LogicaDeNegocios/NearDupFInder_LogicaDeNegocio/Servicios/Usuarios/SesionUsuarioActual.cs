namespace NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;

public class SesionUsuarioActual
{
    public void Asignar(string email)
    {
        EmailActual = email;
    }

    public string EmailActual
    {
        get => _emailActual;
        private set
        {
            _emailActual = value;
        }
    }
    private string _emailActual = "No hay usuario logueado";

    public void Desasignar() => EmailActual = "No hay usuario logueado";
}
