namespace NearDupFInder_LogicaDeNegocio.DTOs.ParaGestorAuditoria
{
    public class AuditoriaDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty;
        public string Detalles { get; set; } = string.Empty;
    }

}