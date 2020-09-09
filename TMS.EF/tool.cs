namespace TMS.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tool")]
    public partial class tool
    {
        public Guid toolId { get; set; }

        public Guid? typeId { get; set; }

        public int? seqId { get; set; }

        [StringLength(20)]
        public string billNo { get; set; }

        public Guid? operatorId { get; set; }

        public DateTime? regDate { get; set; }

        public int? usedCount { get; set; }

        [StringLength(50)]
        public string location { get; set; }

        public DateTime? proDate { get; set; }

        public string picUrl { get; set; }

        [Column("out")]
        public bool? _out { get; set; }
    }
}
