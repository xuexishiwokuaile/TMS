namespace TMS.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class spt_values
    {
        [StringLength(35)]
        public string name { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int number { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        public string type { get; set; }

        public int? low { get; set; }

        public int? high { get; set; }

        public int? status { get; set; }
    }
}
