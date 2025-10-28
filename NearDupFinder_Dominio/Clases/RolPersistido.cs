namespace NearDupFinder_Dominio.Clases;

public class RolPersistido
{
    public string Valor { get; private set; } = null!;
    private RolPersistido() {}
    public RolPersistido(string valor) => Valor = valor;
}