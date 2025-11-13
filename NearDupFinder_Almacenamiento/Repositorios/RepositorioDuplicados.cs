using Microsoft.EntityFrameworkCore;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Interfaces;

namespace NearDupFinder_Almacenamiento.Repositorios;

public class RepositorioDuplicados(SqlContext contexto) : IRepositorioDuplicados
{
    public IReadOnlyList<ParDuplicado> ObtenerListaDeDuplicados()
    {
        return contexto.Set<ParDuplicado>()
            .Include(parDuplicado => parDuplicado.ItemAComparar)
            .Include(parDuplicado => parDuplicado.ItemPosibleDuplicado)
            .AsNoTracking()
            .ToList();
    }

    public void AgregarDuplicado(ParDuplicado parDuplicado)
    {
        contexto.Attach(parDuplicado.ItemAComparar);
        contexto.Attach(parDuplicado.ItemPosibleDuplicado);

        contexto.Set<ParDuplicado>().Add(parDuplicado);
        contexto.SaveChanges();
    }    

    public void EliminarDuplicado(ParDuplicado parDuplicado)
    {
        ParDuplicado? existente = contexto.Set<ParDuplicado>().Find(parDuplicado.Id);
        if (existente is null)
            throw new InvalidOperationException("El par duplicado no existe o ya fue eliminado.");

        contexto.Remove(existente);
        contexto.SaveChanges();
    }
}