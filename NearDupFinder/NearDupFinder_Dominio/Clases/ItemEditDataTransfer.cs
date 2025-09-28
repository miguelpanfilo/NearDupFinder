namespace NearDupFinder_Dominio.Clases;
public class ItemEditDataTransfer
{
    public int Id { get; set; }             
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Categoria { get; set; } =string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; }=string.Empty;
}
