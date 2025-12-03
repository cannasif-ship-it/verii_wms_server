using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PHeaderConfiguration : BaseEntityConfiguration<PHeader>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PHeader> builder)
        {
            builder.ToTable("RII_P_HEADER");

            builder.Property(x => x.PackageCode)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("PackageCode");

            builder.Property(x => x.PackageType)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("PackageType");

            builder.Property(x => x.SourceType)
                .HasMaxLength(30)
                .HasColumnName("SourceType");

            builder.Property(x => x.SourceId)
                .HasColumnName("SourceId");

            builder.Property(x => x.WarehouseCode)
                .HasMaxLength(20)
                .HasColumnName("WarehouseCode");

            builder.Property(x => x.LocationCode)
                .HasMaxLength(20)
                .HasColumnName("LocationCode");

            builder.Property(x => x.GrossWeight)
                .HasColumnType("decimal(18,6)")
                .HasColumnName("GrossWeight");

            builder.Property(x => x.NetWeight)
                .HasColumnType("decimal(18,6)")
                .HasColumnName("NetWeight");

            builder.Property(x => x.Volume)
                .HasColumnType("decimal(18,6)")
                .HasColumnName("Volume");

            builder.Property(x => x.Description)
                .HasMaxLength(100)
                .HasColumnName("Description");

            builder.HasIndex(x => x.PackageCode)
                .IsUnique()
                .HasDatabaseName("IX_PHeader_PackageCode");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_PHeader_IsDeleted");
        }
    }
}