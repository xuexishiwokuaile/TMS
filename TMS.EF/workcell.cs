namespace TMS.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("workcell")]
    public partial class workcell
    {
        public Guid id { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }
        public Guid? managerid { get; set; }

        [Column(TypeName = "date")]
        public DateTime? date { get; set; }
    }
}
