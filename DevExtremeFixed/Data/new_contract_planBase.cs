namespace DevExtremeFixed.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class new_contract_planBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public new_contract_planBase()
        {
            new_contract_plan_productBase = new HashSet<new_contract_plan_productBase>();
        }

        [Key]
        public Guid new_contract_planId { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public Guid? CreatedOnBehalfBy { get; set; }

        public Guid? ModifiedOnBehalfBy { get; set; }

        public Guid OwnerId { get; set; }

        public int OwnerIdType { get; set; }

        public Guid? OwningBusinessUnit { get; set; }

        public int statecode { get; set; }

        public int? statuscode { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] VersionNumber { get; set; }

        public int? ImportSequenceNumber { get; set; }

        public DateTime? OverriddenCreatedOn { get; set; }

        public int? TimeZoneRuleVersionNumber { get; set; }

        public int? UTCConversionTimeZoneCode { get; set; }

        [StringLength(4)]
        public string new_year { get; set; }

        public int? new_quarter { get; set; }

        public Guid? new_link_businessunit_id { get; set; }

        public Guid? new_link_product_group_id { get; set; }

        public Guid? new_link_product_id { get; set; }

        public int? new_service_type { get; set; }

        public decimal? new_product_sum { get; set; }

        public Guid? new_link_territory_id { get; set; }

        public decimal? new_1_auarter { get; set; }

        public DateTime? new_1_auarter_Date { get; set; }

        public int? new_1_auarter_State { get; set; }

        public int? new_acception_result { get; set; }

        public Guid? new_contract_plan_acception_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<new_contract_plan_productBase> new_contract_plan_productBase { get; set; }

        public virtual new_d_product_catalogBase new_d_product_catalogBase { get; set; }

        public virtual new_d_product_groupsBase new_d_product_groupsBase { get; set; }
    }
}
