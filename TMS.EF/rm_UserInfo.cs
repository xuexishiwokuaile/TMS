namespace TMS.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class rm_UserInfo
    {
        [Key]
        public Guid UserGuid { get; set; }

        [StringLength(20)]
        public string UserName { get; set; }

        [StringLength(36)]
        public string UserPassWord { get; set; }

        [StringLength(11)]
        public string PhoneNum { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
