namespace TMS.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class scrap_order
    {
        [Key]
        public Guid scrapId { get; set; }

        public Guid? proposerId { get; set; }

        public Guid? supervisorId { get; set; }

        public Guid? managerId { get; set; }

        public Guid? toolId { get; set; }

        public int? lifeCount { get; set; }

        public string description { get; set; }

        public int? state { get; set; }
    }
}
