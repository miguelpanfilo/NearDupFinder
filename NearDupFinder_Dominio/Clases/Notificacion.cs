namespace NearDupFinder_Dominio.Clases;

public class Notificacion
{
    public int Id { get; private set; }

    public string EmailUsuario { get; private set; }
    public string Mensaje { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public bool Leida { get; private set; }

    private Notificacion() { } 

    public Notificacion(string emailUsuario, string mensaje)
    {
        EmailUsuario  = emailUsuario;
        Mensaje       = mensaje;
        FechaCreacion = DateTime.Now;
        Leida         = false;
    }

    public void MarcarComoLeida()
    {
        Leida = true;
    }
}