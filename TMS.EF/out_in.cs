namespace TMS.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class out_in
    {
        [Key]
        public Guid recordId { get; set; }

        public DateTime? date { get; set; }

        public Guid? recorderId { get; set; }

        public Guid? passerId { get; set; }

        [Column("out")]
        public bool? _out { get; set; }

        [StringLength(50)]
        public string location { get; set; }

        public int? productLineId { get; set; }
    }
}
