namespace AnacondaMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("anaconda.UserStatistics")]
    public partial class UserStatistic
    {
        public string Id { get; set; }

        public int Experience { get; set; }

        public int Luck { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}
