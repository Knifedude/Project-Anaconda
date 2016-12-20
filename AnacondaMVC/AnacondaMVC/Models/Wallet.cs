namespace AnacondaMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("anaconda.Wallet")]
    public partial class Wallet
    {
        [Key]
        public string UserId { get; set; }

        public int Credits { get; set; }

        public int CasinoCredits { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}
