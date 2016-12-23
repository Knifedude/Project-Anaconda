namespace AnacondaMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("anaconda.UserDaily")]
    public partial class UserDaily
    {
        public string Id { get; set; }

        public DateTime? LastDaily { get; set; }

        public DateTime? LastHourly { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}
