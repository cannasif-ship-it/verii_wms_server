using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class GrLineSerialConfiguration : BaseLineSerialEntityConfiguration<GrLineSerial>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<GrLineSerial> builder)
        {
            builder.ToTable("RII_GR_LINE_SERIAL");

            builder.Property(x => x.ImportLineId)
                .HasColumnName("ImportLineId");

            builder.HasOne(x => x.ImportLine)
                .WithMany()
                .HasForeignKey(x => x.ImportLineId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_GrLineSerial_GrImportLine");

            builder.Property(x => x.ClientKey)
                .HasMaxLength(100)
                .HasColumnName("ClientKey");

            builder.HasIndex(x => x.ImportLineId)
                .HasDatabaseName("IX_GrLineSerial_ImportLineId");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_GrLineSerial_IsDeleted");

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
