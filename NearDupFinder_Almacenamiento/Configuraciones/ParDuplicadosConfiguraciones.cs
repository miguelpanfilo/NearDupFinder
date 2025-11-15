using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Almacenamiento.Configuraciones;

public class ParDuplicadosConfiguraciones: IEntityTypeConfiguration<ParDuplicado>
{
    public void Configure(EntityTypeBuilder<ParDuplicado> builder)
    {
        builder.ToTable("ParesDuplicados");
        
        builder.HasKey(parDuplicado => parDuplicado.Id);
        builder.Property(parDuplicado => parDuplicado.Id).ValueGeneratedOnAdd();
        
        builder.HasOne<Catalogo>().WithMany().HasForeignKey(parDuplicado => parDuplicado.IdCatalogo).OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(parDuplicado => parDuplicado.ItemAComparar).WithMany().OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(parDuplicado => parDuplicado.ItemPosibleDuplicado).WithMany().OnDelete(DeleteBehavior.Restrict);

        var conversorTokensAJson = new ValueConverter<string[], string>(
            arreglo => JsonSerializer.Serialize(arreglo, (JsonSerializerOptions?)null),
            json=> JsonSerializer.Deserialize<string[]>(json, (JsonSerializerOptions?)null) ?? Array.Empty<string>()
        );

        var comparadorArregloDeStrings = new ValueComparer<string[]>(
            (a, b) => ReferenceEquals(a, b) || (a != null && b != null && a.SequenceEqual(b)),
            a => a.Aggregate(17, (hash, token) => hash * 31 + (token.GetHashCode())),
            a => a.ToArray()
        );

        builder.Property(parDuplicado => parDuplicado.TokensCompartidosTitulo).HasConversion(conversorTokensAJson).Metadata.SetValueComparer(comparadorArregloDeStrings);

        builder.Property(parDuplicado => parDuplicado.TokensCompartidosDescripcion).HasConversion(conversorTokensAJson).Metadata.SetValueComparer(comparadorArregloDeStrings);
    }
}