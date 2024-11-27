using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Operations_1")]
[Index("OperationalId", Name = "unq_Operations_1_Operational_ID", IsUnique = true)]
public partial class Operations1
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("Operational_ID")]
    public int? OperationalId { get; set; }

    [InverseProperty("Operational")]
    public virtual Operation? Operation { get; set; }

    [ForeignKey("OperationalId")]
    [InverseProperty("Operations1")]
    public virtual IterationCycle1 Operational { get; set; } = null!;

    [ForeignKey("OperationalId")]
    [InverseProperty("Operations1")]
    public virtual IterationCycle3 Operational1 { get; set; } = null!;

    [ForeignKey("OperationalId")]
    [InverseProperty("Operations1")]
    public virtual IterationCycle4 Operational2 { get; set; } = null!;

    [ForeignKey("OperationalId")]
    [InverseProperty("Operations1")]
    public virtual OperationsStage1 Operational3 { get; set; } = null!;

    [ForeignKey("OperationalId")]
    [InverseProperty("Operations1")]
    public virtual OperationsStage2 Operational4 { get; set; } = null!;

    [ForeignKey("OperationalId")]
    [InverseProperty("Operations1")]
    public virtual OperationsStage3 Operational5 { get; set; } = null!;

    [ForeignKey("OperationalId")]
    [InverseProperty("Operations1")]
    public virtual OperationsStage4 Operational6 { get; set; } = null!;

    [ForeignKey("OperationalId")]
    [InverseProperty("Operations1")]
    public virtual IterationCycle2 OperationalNavigation { get; set; } = null!;
}
