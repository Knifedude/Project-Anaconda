namespace AnacondaMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("anaconda.GameStatistics")]
    public partial class GameStatistic
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public int CreditResult { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public virtual Game Game { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}
