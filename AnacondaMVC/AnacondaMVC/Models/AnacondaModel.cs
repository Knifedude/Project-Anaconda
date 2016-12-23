namespace AnacondaMVC.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AnacondaModel : DbContext
    {
        public AnacondaModel()
            : base("name=AnacondaModel")
        {
        }

        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<GameStatistic> GameStatistics { get; set; }
        public virtual DbSet<GlobalProp> GlobalProps { get; set; }
        public virtual DbSet<UserDaily> UserDailies { get; set; }
        public virtual DbSet<UserStatistic> UserStatistics { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasMany(e => e.GameStatistics)
                .WithRequired(e => e.Game)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.GameStatistics)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasOptional(e => e.UserDaily)
                .WithRequired(e => e.AspNetUser);

            modelBuilder.Entity<AspNetUser>()
                .HasOptional(e => e.UserStatistic)
                .WithRequired(e => e.AspNetUser);

            modelBuilder.Entity<AspNetUser>()
                .HasOptional(e => e.Wallet)
                .WithRequired(e => e.AspNetUser);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);
        }
    }
}
