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

        builder.Property<int?>("ClusterId");
        builder.HasOne<Cluster>()
            .WithMany(nameof(Cluster.PertenecientesCluster))
            .HasForeignKey("ClusterId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasIndex("ClusterId");
        
        builder.Property(item => item.ImagenBase64)
            .HasColumnName("ImagenBase64")
            .HasColumnType("nvarchar(max)")
            .IsRequired(false);
    }
}