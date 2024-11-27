﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Iteration_Cycle_1")]
[Index("CsrOpartationalId", Name = "unq_Iteration_Cycle_1_CSR_Opartational_ID", IsUnique = true)]
[Index("IterationCycleId", Name = "unq_Iteration_Cycle_1_Iteration_Cycle_ID", IsUnique = true)]
[Index("OperationalId", Name = "unq_Iteration_Cycle_1_Operational_ID", IsUnique = true)]
[Index("OrderId", Name = "unq_Iteration_Cycle_1_Order_ID", IsUnique = true)]
public partial class IterationCycle1
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("Iteration_Cycle_ID")]
    public int? IterationCycleId { get; set; }

    [Required]
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

    [InverseProperty("CsrOpartational")]
    public virtual Csr1? Csr1 { get; set; }

    [InverseProperty("CsrOpartational")]
    public virtual Csr2? Csr2 { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("IterationCycle1s")]
    public virtual Client? Customer { get; set; }

    [ForeignKey("IterationCycleId")]
    [InverseProperty("IterationCycle1")]
    public virtual IterationCycle2? IterationCycle { get; set; }

    [InverseProperty("IterationCycle")]
    public virtual IterationCycle4? IterationCycle4 { get; set; }

    [InverseProperty("ModelDbMuteP1Operations")]
    public virtual ICollection<ModelDbMuteP1> ModelDbMuteP1s { get; set; } = new List<ModelDbMuteP1>();

    [InverseProperty("Operational")]
    public virtual Operations1? Operations1 { get; set; }

    [InverseProperty("Operational")]
    public virtual Operations2? Operations2 { get; set; }

    [InverseProperty("Order")]
    public virtual OperationsStage2? OperationsStage2 { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("IterationCycle1")]
    public virtual OperationsStage3? Order { get; set; }

    [ForeignKey("SubProductA, SubProductB, SubProductC")]
    [InverseProperty("IterationCycle1s")]
    public virtual Product? Product { get; set; }

    [ForeignKey("SubServiceA, SubServiceB, SubServiceC")]
    [InverseProperty("IterationCycle1s")]
    public virtual Service? Service { get; set; }
}
