namespace TMS.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tool_type
    {
        [Key]
        public Guid typeId { get; set; }

        [StringLength(10)]
        public string workcellId { get; set; }

        [StringLength(10)]
        public string familyId { get; set; }

        [StringLength(10)]
        public string code { get; set; }

        [StringLength(20)]
        public string name { get; set; }

        [StringLength(40)]
        public string model { get; set; }

        [StringLength(40)]
        public string partNo { get; set; }

        [StringLength(20)]
        public string usedFor { get; set; }

        public int? ulp { get; set; }

        public Guid? ownerId { get; set; }

        public int? pmPeriod { get; set; }

        public DateTime? recOn { get; set; }

        public Guid? recById { get; set; }

        public DateTime? editOn { get; set; }

        public Guid? editById { get; set; }
    }
}
