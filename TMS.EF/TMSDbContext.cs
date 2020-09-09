namespace TMS.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TMSDbContext : DbContext
    {
        public TMSDbContext()
            : base("name=TMSDbContext")
        {
        }

        public virtual DbSet<classification> classification { get; set; }
        public virtual DbSet<out_in> out_in { get; set; }
        public virtual DbSet<purchase_order> purchase_order { get; set; }
        public virtual DbSet<repair_order> repair_order { get; set; }
        public virtual DbSet<rm_UserInfo> rm_UserInfo { get; set; }
        public virtual DbSet<scrap_order> scrap_order { get; set; }
        public virtual DbSet<tool> tool { get; set; }
        public virtual DbSet<tool_type> tool_type { get; set; }
        public virtual DbSet<user> user { get; set; }
        public virtual DbSet<workcell> workcell { get; set; }
        public virtual DbSet<MSreplication_options> MSreplication_options { get; set; }
        public virtual DbSet<spt_fallback_db> spt_fallback_db { get; set; }
        public virtual DbSet<spt_fallback_dev> spt_fallback_dev { get; set; }
        public virtual DbSet<spt_fallback_usg> spt_fallback_usg { get; set; }
        public virtual DbSet<spt_monitor> spt_monitor { get; set; }
        public virtual DbSet<spt_values> spt_values { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<classification>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<repair_order>()
                .Property(e => e.faultPicUrl)
                .IsUnicode(false);

            modelBuilder.Entity<rm_UserInfo>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<rm_UserInfo>()
                .Property(e => e.UserPassWord)
                .IsUnicode(false);

            modelBuilder.Entity<rm_UserInfo>()
                .Property(e => e.PhoneNum)
                .IsUnicode(false);

            modelBuilder.Entity<tool>()
                .Property(e => e.billNo)
                .IsFixedLength();

            modelBuilder.Entity<tool_type>()
                .Property(e => e.workcellId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tool_type>()
                .Property(e => e.familyId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.telnumber)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.apartment)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.pwd)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<workcell>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<spt_fallback_db>()
                .Property(e => e.xserver_name)
                .IsUnicode(false);

            modelBuilder.Entity<spt_fallback_db>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<spt_fallback_dev>()
                .Property(e => e.xserver_name)
                .IsUnicode(false);

            modelBuilder.Entity<spt_fallback_dev>()
                .Property(e => e.xfallback_drive)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<spt_fallback_dev>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<spt_fallback_dev>()
                .Property(e => e.phyname)
                .IsUnicode(false);

            modelBuilder.Entity<spt_fallback_usg>()
                .Property(e => e.xserver_name)
                .IsUnicode(false);

            modelBuilder.Entity<spt_values>()
                .Property(e => e.type)
                .IsFixedLength();
        }
    }
}
