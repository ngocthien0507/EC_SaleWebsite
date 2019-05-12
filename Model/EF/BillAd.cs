namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BillAd")]
    public partial class BillAd
    {
        public long ID { get; set; }

        public long? IdOrderAd { get; set; }

        [StringLength(50)]
        public string NameCus { get; set; }

        [Column("CMND/ID")]
        [StringLength(50)]
        public string CMND_ID { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(20)]
        public string TradingCode { get; set; }

        [Column(TypeName = "money")]
        public decimal? Money { get; set; }

        public DateTime? CreateDate { get; set; }

        public virtual OrderAdvertisement OrderAdvertisement { get; set; }
    }
}
