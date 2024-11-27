using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Employee_Operations")]
[Index("EmployeeOperationsId", Name = "unq_Employee_Operations_Employee_Operations_ID", IsUnique = true)]
public partial class EmployeeOperation
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("Employee_Operations_ID")]
    public int? EmployeeOperationsId { get; set; }

    [ForeignKey("EmployeeOperationsId")]
    [InverseProperty("EmployeeOperation")]
    public virtual Csr? EmployeeOperations { get; set; }

    [ForeignKey("EmployeeOperationsId")]
    [InverseProperty("EmployeeOperation")]
    public virtual Operation? EmployeeOperationsNavigation { get; set; }

    [InverseProperty("EmployeeOperations")]
    public virtual ICollection<ModelDbMuteP1Operation> ModelDbMuteP1Operations { get; set; } = new List<ModelDbMuteP1Operation>();

    [InverseProperty("EmployeeOperations")]
    public virtual ICollection<ModelDbMuteP1OperationsStage2A> ModelDbMuteP1OperationsStage2As { get; set; } = new List<ModelDbMuteP1OperationsStage2A>();

    [InverseProperty("EmployeeOperations")]
    public virtual ICollection<ModelDbMuteP1OperationsStage2B> ModelDbMuteP1OperationsStage2Bs { get; set; } = new List<ModelDbMuteP1OperationsStage2B>();
}
