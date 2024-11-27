using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Model_DB_Init")]
[Index("CustomerId", Name = "unq_Model_DB_Init_Customer_ID", IsUnique = true)]
public partial class ModelDbInit
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Model_DB_Init_TimeStamp", TypeName = "smalldatetime")]
    public DateTime? ModelDbInitTimeStamp { get; set; }

    [Column("Model_DB_Init_Catagorical_ID")]
    public int? ModelDbInitCatagoricalId { get; set; }

    [Column("Model_DB_Init_Catagorical_Name")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ModelDbInitCatagoricalName { get; set; }

    [Column("Model_DB_Init_ModelData")]
    public bool? ModelDbInitModelData { get; set; }

    public bool? Data { get; set; }

    [Required]
    [Column("Customer_ID")]
    public int? CustomerId { get; set; }

    [InverseProperty("CustomerNavigation")]
    public virtual Client? Client { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("ModelDbInit")]
    public virtual ModelDbMuteP1 Customer { get; set; } = null!;

    [InverseProperty("ModelDbInitNavigation")]
    public virtual ModelDbMuteP1? ModelDbMuteP1 { get; set; }

    [InverseProperty("ModelDbInit")]
    public virtual ICollection<ModelDbMuteP1Customer> ModelDbMuteP1Customers { get; set; } = new List<ModelDbMuteP1Customer>();

    [InverseProperty("ModelDbInit")]
    public virtual ICollection<ModelDbMuteP1Operation> ModelDbMuteP1Operations { get; set; } = new List<ModelDbMuteP1Operation>();

    [InverseProperty("ModelDbInit")]
    public virtual ICollection<ModelDbMuteP1Qa> ModelDbMuteP1Qas { get; set; } = new List<ModelDbMuteP1Qa>();

    [InverseProperty("ModelDbInit")]
    public virtual ICollection<ModelDbMuteP1Sale> ModelDbMuteP1Sales { get; set; } = new List<ModelDbMuteP1Sale>();
}
