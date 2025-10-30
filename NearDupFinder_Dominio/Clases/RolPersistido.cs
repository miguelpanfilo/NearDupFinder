namespace NearDupFinder_Dominio.Clases;

public class RolPersistido
{
    public string Valor { get; set; } = string.Empty;
    private RolPersistido() {}
    public RolPersistido(string valor) => Valor = valor;
    
}