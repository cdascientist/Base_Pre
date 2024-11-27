using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Model_DB_Mute_P1_Operations")]
[Index("DbMuteP1CustomerId", Name = "unq_Model_DB_Mute_P1_Operations_DB_Mute_P1_Customer_ID", IsUnique = true)]
public partial class ModelDbMuteP1Operation
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Model_DB_Init_ID")]
    public int? ModelDbInitId { get; set; }

    [Required]
    [Column("DB_Mute_P1_Customer_ID")]
    public int? DbMuteP1CustomerId { get; set; }

    [Column("Model_DB_Mute_P1_Operations_TimeStamp", TypeName = "smalldatetime")]
    public DateTime? ModelDbMuteP1OperationsTimeStamp { get; set; }

    public bool? Data { get; set; }

    [Column("Operational_ID")]
    public int? OperationalId { get; set; }

    [Column("Employee_Operations_ID")]
    public int? EmployeeOperationsId { get; set; }

    [ForeignKey("DbMuteP1CustomerId")]
    [InverseProperty("ModelDbMuteP1Operation")]
    public virtual ModelDbMuteP1? DbMuteP1Customer { get; set; }

    [ForeignKey("EmployeeOperationsId")]
    [InverseProperty("ModelDbMuteP1Operations")]
    public virtual EmployeeOperation? EmployeeOperations { get; set; }

    [ForeignKey("ModelDbInitId")]
    [InverseProperty("ModelDbMuteP1Operations")]
    public virtual ModelDbInit? ModelDbInit { get; set; }

    [InverseProperty("DbMuteP1Customer")]
    public virtual ICollection<ModelDbMuteP1OperationsStage2A> ModelDbMuteP1OperationsStage2As { get; set; } = new List<ModelDbMuteP1OperationsStage2A>();

    [InverseProperty("DbMuteP1Customer")]
    public virtual ICollection<ModelDbMuteP1OperationsStage2B> ModelDbMuteP1OperationsStage2Bs { get; set; } = new List<ModelDbMuteP1OperationsStage2B>();
}
