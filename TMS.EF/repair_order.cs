namespace TMS.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class repair_order
    {
        [Key]
        public Guid repairOrderId { get; set; }

        public Guid? proposerId { get; set; }

        public Guid? toolId { get; set; }

        public Guid? dealerId { get; set; }

        [StringLength(255)]
        public string faultPicUrl { get; set; }

        public string description { get; set; }

        public int? state { get; set; }
    }
}
