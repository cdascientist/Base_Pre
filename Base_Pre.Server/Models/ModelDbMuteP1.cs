using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Model_DB_Mute_P1")]
[Index("CustomerId", Name = "unq_Model_DB_Mute_P1_Customer_ID", IsUnique = true)]
[Index("ModelDbInitId", Name = "unq_Model_DB_Mute_P1_Model_DB_Init_ID", IsUnique = true)]
[Index("ModelDbMuteP1CustomerId", Name = "unq_Model_DB_Mute_P1_Model_DB_Mute_P1_Customer_ID", IsUnique = true)]
public partial class ModelDbMuteP1
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("Model_DB_Init_ID")]
    public int? ModelDbInitId { get; set; }

    [Column("Model_DB_Mute_P1_TimeStamp", TypeName = "smalldatetime")]
    public DateTime? ModelDbMuteP1TimeStamp { get; set; }

    [Required]
    [Column("Model_DB_Mute_P1_Customer_ID")]
    public int? ModelDbMuteP1CustomerId { get; set; }

    [Column("Model_DB_Mute_P1_Catagorical_ID")]
    public int? ModelDbMuteP1CatagoricalId { get; set; }

    [Column("Model_DB_Mute_P1_Sales_ID")]
    public int? ModelDbMuteP1SalesId { get; set; }

    [Column("Model_DB_Mute_P1_Operations_ID")]
    public int? ModelDbMuteP1OperationsId { get; set; }

    [Column("Model_DB_Mute_P1_QA_ID")]
    public int? ModelDbMuteP1QaId { get; set; }

    public bool? Data { get; set; }

    [Required]
    [Column("Customer_ID")]
    public int? CustomerId { get; set; }

    [InverseProperty("Customer")]
    public virtual ModelDbInit? ModelDbInit { get; set; }

    [ForeignKey("ModelDbInitId")]
    [InverseProperty("ModelDbMuteP1")]
    public virtual ModelDbInit? ModelDbInitNavigation { get; set; }

    [InverseProperty("ModelDbInitNavigation")]
    public virtual ICollection<ModelDbMuteP1Customer> ModelDbMuteP1Customers { get; set; } = new List<ModelDbMuteP1Customer>();

    [InverseProperty("DbMuteP1Customer")]
    public virtual ModelDbMuteP1Operation? ModelDbMuteP1Operation { get; set; }

    [ForeignKey("ModelDbMuteP1OperationsId")]
    [InverseProperty("ModelDbMuteP1s")]
    public virtual IterationCycle1? ModelDbMuteP1Operations { get; set; }

    [ForeignKey("ModelDbMuteP1OperationsId")]
    [InverseProperty("ModelDbMuteP1s")]
    public virtual IterationCycle3? ModelDbMuteP1Operations1 { get; set; }

    [ForeignKey("ModelDbMuteP1OperationsId")]
    [InverseProperty("ModelDbMuteP1s")]
    public virtual IterationCycle4? ModelDbMuteP1Operations2 { get; set; }

    [ForeignKey("ModelDbMuteP1OperationsId")]
    [InverseProperty("ModelDbMuteP1s")]
    public virtual OperationsStage1? ModelDbMuteP1Operations3 { get; set; }

    [ForeignKey("ModelDbMuteP1OperationsId")]
    [InverseProperty("ModelDbMuteP1s")]
    public virtual OperationsStage2? ModelDbMuteP1Operations4 { get; set; }

    [ForeignKey("ModelDbMuteP1OperationsId")]
    [InverseProperty("ModelDbMuteP1s")]
    public virtual OperationsStage4? ModelDbMuteP1Operations5 { get; set; }

    [ForeignKey("ModelDbMuteP1OperationsId")]
    [InverseProperty("ModelDbMuteP1s")]
    public virtual OperationsStage3? ModelDbMuteP1Operations6 { get; set; }

    [ForeignKey("ModelDbMuteP1OperationsId")]
    [InverseProperty("ModelDbMuteP1s")]
    public virtual IterationCycle2? ModelDbMuteP1OperationsNavigation { get; set; }

    [InverseProperty("DbMuteP1Customer")]
    public virtual ModelDbMuteP1Qa? ModelDbMuteP1Qa { get; set; }

    [InverseProperty("DbMuteP1Customer")]
    public virtual ModelDbMuteP1Sale? ModelDbMuteP1Sale { get; set; }

    [InverseProperty("ModelDbMuteP1")]
    public virtual ICollection<ModelDbMuteP2> ModelDbMuteP2s { get; set; } = new List<ModelDbMuteP2>();
}
