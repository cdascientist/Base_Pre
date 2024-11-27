using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Iteration_Cycle_2")]
[Index("CsrOpartationalId", Name = "unq_Iteration_Cycle_2_CSR_Opartational_ID", IsUnique = true)]
[Index("IterationCycleId", Name = "unq_Iteration_Cycle_2_Iteration_Cycle_ID", IsUnique = true)]
[Index("OperationalId", Name = "unq_Iteration_Cycle_2_Operational_ID", IsUnique = true)]
public partial class IterationCycle2
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("Iteration_Cycle_ID")]
    public int? IterationCycleId { get; set; }

    [Column("Order_ID")]
    public int? OrderId { get; set; }

    [Required]
    [Column("CSR_Opartational_ID")]
    public int? CsrOpartationalId { get; set; }

    [Required]
    [Column("Operational_ID")]
    public int? OperationalId { get; set; }

    [Column("Customer_ID")]
    public int? CustomerId { get; set; }

    [Column("Sales_ID")]
    public int? SalesId { get; set; }

    [Column("Operations_ID")]
    public int? OperationsId { get; set; }

    [Column("SubService_A")]
    public int? SubServiceA { get; set; }

    [Column("SubService_B")]
    public int? SubServiceB { get; set; }

    [Column("SubService_C")]
    public int? SubServiceC { get; set; }

    [Column("SubProduct_A")]
    public int? SubProductA { get; set; }

    [Column("SubProduct_B")]
    public int? SubProductB { get; set; }

    [Column("SubProduct_C")]
    public int? SubProductC { get; set; }

    [InverseProperty("CsrOpartationalNavigation")]
    public virtual Csr1? Csr1 { get; set; }

    [InverseProperty("CsrOpartationalNavigation")]
    public virtual Csr2? Csr2 { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("IterationCycle2s")]
    public virtual Client? Customer { get; set; }

    [ForeignKey("IterationCycleId")]
    [InverseProperty("IterationCycle2")]
    public virtual IterationCycle3 IterationCycle { get; set; } = null!;

    [InverseProperty("IterationCycle")]
    public virtual IterationCycle1? IterationCycle1 { get; set; }

    [InverseProperty("ModelDbMuteP1OperationsNavigation")]
    public virtual ICollection<ModelDbMuteP1> ModelDbMuteP1s { get; set; } = new List<ModelDbMuteP1>();

    [InverseProperty("OperationalNavigation")]
    public virtual Operations1? Operations1 { get; set; }

    [InverseProperty("OperationalNavigation")]
    public virtual Operations2? Operations2 { get; set; }

    [ForeignKey("SubProductA, SubProductB, SubProductC")]
    [InverseProperty("IterationCycle2s")]
    public virtual Product? Product { get; set; }

    [ForeignKey("SubServiceA, SubServiceB, SubServiceC")]
    [InverseProperty("IterationCycle2s")]
    public virtual Service? Service { get; set; }
}
