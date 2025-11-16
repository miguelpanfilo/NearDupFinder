using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Almacenamiento.Configuraciones;

public class ClusterConfiguraciones : IEntityTypeConfiguration<Cluster>
{
    public void Configure(EntityTypeBuilder<Cluster> builder)
    {
        builder.ToTable("Clusters");
        builder.HasKey(cl => cl.Id);
        builder.Property(cl => cl.Id).ValueGeneratedNever();

        builder.HasOne<Catalogo>()
            .WithMany(nameof(Catalogo.Clusters))
            .HasForeignKey("CatalogoId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex("CatalogoId");

        builder.Metadata.FindNavigation(nameof(Cluster.PertenecientesCluster))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne(c => c.Canonico)
            .WithMany()
            .HasForeignKey("CanonicoId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Property(c => c.ImagenCanonicaBase64)
            .HasColumnType("nvarchar(max)")
            .HasColumnName("ImagenCanonicaBase64");

        builder.Property(c => c.StockMinimoCanonico)
            .HasColumnName("StockMinimoCanonico");

        builder.Property(c => c.PrecioCanonico)
            .HasColumnName("PrecioCanonico");
    }
}