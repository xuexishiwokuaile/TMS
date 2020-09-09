namespace TMS.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("user")]
    public partial class user
    {
        public Guid id { get; set; }

        [Required]
        [StringLength(20)]
        public string name { get; set; }

        public int classificationid { get; set; }

        [Required]
        [StringLength(20)]
        public string telnumber { get; set; }

        [StringLength(255)]
        public string apartment { get; set; }

        public int state { get; set; }

        public int gender { get; set; }

        public Guid workcellid { get; set; }

        [Required]
        [StringLength(255)]
        public string pwd { get; set; }

        [Required]
        [StringLength(50)]
        public string email { get; set; }

        public Guid? sessionid { get; set; }
    }
}
