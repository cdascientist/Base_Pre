using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Service")]
[Index("SubServiceA", "SubServiceB", "SubServiceC", Name = "unq_Service_SubService_A", IsUnique = true)]
[Index("SubServiceA", "SubServiceB", Name = "unq_Service_SubService_A_0", IsUnique = true)]
public partial class Service
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("SubService_A")]
    public int? SubServiceA { get; set; }

    [Required]
    [Column("SubService_B")]
    public int? SubServiceB { get; set; }

    [Required]
    [Column("SubService_C")]
    public int? SubServiceC { get; set; }

    [InverseProperty("Service")]
    public virtual ICollection<IterationCycle1> IterationCycle1s { get; set; } = new List<IterationCycle1>();

    [InverseProperty("Service")]
    public virtual ICollection<IterationCycle2> IterationCycle2s { get; set; } = new List<IterationCycle2>();

    [InverseProperty("Service")]
    public virtual ICollection<IterationCycle3> IterationCycle3s { get; set; } = new List<IterationCycle3>();

    [InverseProperty("Service")]
    public virtual ICollection<IterationCycle4> IterationCycle4s { get; set; } = new List<IterationCycle4>();

    [InverseProperty("Service")]
    public virtual ICollection<OperationsStage1> OperationsStage1s { get; set; } = new List<OperationsStage1>();

    [InverseProperty("Service")]
    public virtual ICollection<OperationsStage2> OperationsStage2s { get; set; } = new List<OperationsStage2>();

    [InverseProperty("Service")]
    public virtual ICollection<OperationsStage3> OperationsStage3s { get; set; } = new List<OperationsStage3>();

    [InverseProperty("Service")]
    public virtual ICollection<OperationsStage4> OperationsStage4s { get; set; } = new List<OperationsStage4>();

    [ForeignKey("SubServiceA")]
    [InverseProperty("Services")]
    public virtual SubServiceA SubServiceANavigation { get; set; } = null!;

    [ForeignKey("SubServiceB")]
    [InverseProperty("Services")]
    public virtual SubServiceB SubServiceBNavigation { get; set; } = null!;

    [ForeignKey("SubServiceC")]
    [InverseProperty("Services")]
    public virtual SubServiceC SubServiceCNavigation { get; set; } = null!;
}
