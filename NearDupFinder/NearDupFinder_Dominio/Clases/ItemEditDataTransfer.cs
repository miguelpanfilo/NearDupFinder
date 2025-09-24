namespace NearDupFinder_Dominio.Clases;

public class ItemEditDataTransfer
{
    public int Id { get; set; }               // Id del item original
    public string Titulo { get; set; } = ""; // Inicializado para no ser null
    public string Descripcion { get; set; } = "";
    public string Categoria { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
}