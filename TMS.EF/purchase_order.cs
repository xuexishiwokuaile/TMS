namespace TMS.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class purchase_order
    {
        [Key]
        public Guid purchaseId { get; set; }

        public Guid? proposerId { get; set; }

        public Guid? typeId { get; set; }

        public Guid? toolId { get; set; }

        public Guid? supervisorId { get; set; }

        public Guid? managerId { get; set; }

        public int? state { get; set; }

        [StringLength(10)]
        public string code { get; set; }
    }
}
