using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Almacenamiento.Configuraciones;

public class ItemConfiguraciones : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedNever();

        builder.HasOne<Catalogo>()                     
            .WithMany("Items")                        
            .HasForeignKey("CatalogoId")               
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);          

        builder.HasIndex("CatalogoId");

        
    }
}