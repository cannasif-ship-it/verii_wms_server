using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_P_LINE")]
    public class PLine : BaseEntity
    {

    // Paket barkodu (kutu, koli, palet)
    [Required, MaxLength(50)]
    public string PackageCode { get; set; } = null!;

    // Paket tipi: BOX, CARTON, PALLET, BAG...
    [Required, MaxLength(20)]
    public string PackageType { get; set; } = "BOX";

    // Kaynak tipi: ORDER, PRODUCTION, COUNTING, TRANSFER vb.
    [MaxLength(30)]
    public string? SourceType { get; set; }

    // Kaynak ID (siparişId / üretimId / sayımId / transferId)
    public long? SourceId { get; set; }

    // Depo ve raf bilgisi
    [MaxLength(20)]
    public string? WarehouseCode { get; set; }
    [MaxLength(20)]
    public string? LocationCode { get; set; }

    // Ağırlık, ölçü, hacim
    [Column(TypeName = "decimal(18,6)")]
    public decimal? GrossWeight { get; set; }
    [Column(TypeName = "decimal(18,6)")]
    public decimal? NetWeight { get; set; }
    [Column(TypeName = "decimal(18,6)")]
    public decimal? Volume { get; set; }

    // Koli açıklamaları
    [MaxLength(100)]
    public string? Description { get; set; }

    // Navigasyon özellikleri
    public virtual ICollection<PLine> Lines { get; set; } = new List<PLine>();
        
    }
}
