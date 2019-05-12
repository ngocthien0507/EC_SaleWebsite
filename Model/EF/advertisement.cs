namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("advertisement")]
    public partial class advertisement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public advertisement()
        {
            OrderAdvertisements = new HashSet<OrderAdvertisement>();
        }

        public long ID { get; set; }

        [StringLength(200)]
        public string Link { get; set; }

        [Column(TypeName = "ntext")]
        public string Content { get; set; }

        [StringLength(50)]
        public string Image { get; set; }

        public DateTime? ActiveDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? Status { get; set; }

        [StringLength(10)]
        public string Location { get; set; }

        public long? Merchant { get; set; }

        public long? CTR { get; set; }

        public virtual Account Account { get; set; }

        public virtual LocationAd LocationAd { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderAdvertisement> OrderAdvertisements { get; set; }
    }
}
