using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("CSR_1")]
[Index("CsrOpartationalId", Name = "unq_CSR_1_CSR_Opartational_ID", IsUnique = true)]
public partial class Csr1
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("CSR_Opartational_ID")]
    public int? CsrOpartationalId { get; set; }

    [InverseProperty("CsrOpartational")]
    public virtual Csr? Csr { get; set; }

    [ForeignKey("CsrOpartationalId")]
    [InverseProperty("Csr1")]
    public virtual IterationCycle1 CsrOpartational { get; set; } = null!;

    [ForeignKey("CsrOpartationalId")]
    [InverseProperty("Csr1")]
    public virtual IterationCycle3 CsrOpartational1 { get; set; } = null!;

    [ForeignKey("CsrOpartationalId")]
    [InverseProperty("Csr1")]
    public virtual IterationCycle4 CsrOpartational2 { get; set; } = null!;

    [ForeignKey("CsrOpartationalId")]
    [InverseProperty("Csr1")]
    public virtual OperationsStage1 CsrOpartational3 { get; set; } = null!;

    [ForeignKey("CsrOpartationalId")]
    [InverseProperty("Csr1")]
    public virtual OperationsStage2 CsrOpartational4 { get; set; } = null!;

    [ForeignKey("CsrOpartationalId")]
    [InverseProperty("Csr1")]
    public virtual OperationsStage3 CsrOpartational5 { get; set; } = null!;

    [ForeignKey("CsrOpartationalId")]
    [InverseProperty("Csr1")]
    public virtual OperationsStage4 CsrOpartational6 { get; set; } = null!;

    [ForeignKey("CsrOpartationalId")]
    [InverseProperty("Csr1")]
    public virtual IterationCycle2 CsrOpartationalNavigation { get; set; } = null!;
}
