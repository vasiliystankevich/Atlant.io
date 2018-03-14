using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logs
{
    public abstract class BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    [Table("Seeding")]
    public class SeedModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool IsSeed { get; set; }
    }

    public class BaseLogModel:BaseModel
    {
        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(255)]
        public string Thread { get; set; }

        [Required]
        [StringLength(50)]
        public string Level { get; set; }

        [Required]
        [StringLength(255)]
        public string Logger { get; set; }

        [Required]
        [StringLength(4000)]
        public string Message { get; set; }

        [Required]
        [StringLength(2000)]
        public string Exception { get; set; }
    }

    [Table("FrontendWebLogs")]
    public class FrontendWebLogsModel : BaseLogModel { }

    [Table("CoreBitconServiceLogs")]
    public class CoreBitconServiceLogsModel : BaseLogModel { }
}
