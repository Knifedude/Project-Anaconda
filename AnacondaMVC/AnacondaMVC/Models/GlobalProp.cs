namespace AnacondaMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("anaconda.GlobalProps")]
    public partial class GlobalProp
    {
        [Key]
        [StringLength(100)]
        public string Name { get; set; }

        public int PropValue { get; set; }
    }
}
