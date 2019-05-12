namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rating")]
    public partial class Rating
    {
        public long ID { get; set; }

        public int? Point { get; set; }

        public long? IDMer { get; set; }

        public virtual Account Account { get; set; }
    }
}
