using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Product")]
[Index("SubProductA", "SubProductB", "SubProductC", Name = "unq_Product_SubProduct_A", IsUnique = true)]
public partial class Product
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("SubProduct_A")]
    public int? SubProductA { get; set; }

    [Required]
    [Column("SubProduct_B")]
    public int? SubProductB { get; set; }

    [Required]
    [Column("SubProduct_C")]
    public int? SubProductC { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<IterationCycle1> IterationCycle1s { get; set; } = new List<IterationCycle1>();

    [InverseProperty("Product")]
    public virtual ICollection<IterationCycle2> IterationCycle2s { get; set; } = new List<IterationCycle2>();

    [InverseProperty("Product")]
    public virtual ICollection<IterationCycle3> IterationCycle3s { get; set; } = new List<IterationCycle3>();

    [InverseProperty("Product")]
    public virtual ICollection<IterationCycle4> IterationCycle4s { get; set; } = new List<IterationCycle4>();

    [InverseProperty("Product")]
    public virtual ICollection<OperationsStage1> OperationsStage1s { get; set; } = new List<OperationsStage1>();

    [InverseProperty("Product")]
    public virtual ICollection<OperationsStage2> OperationsStage2s { get; set; } = new List<OperationsStage2>();

    [InverseProperty("Product")]
    public virtual ICollection<OperationsStage3> OperationsStage3s { get; set; } = new List<OperationsStage3>();

    [InverseProperty("Product")]
    public virtual ICollection<OperationsStage4> OperationsStage4s { get; set; } = new List<OperationsStage4>();

    [ForeignKey("SubProductA")]
    [InverseProperty("Products")]
    public virtual SubProductum SubProductANavigation { get; set; } = null!;

    [ForeignKey("SubProductB")]
    [InverseProperty("Products")]
    public virtual SubProductB SubProductBNavigation { get; set; } = null!;

    [ForeignKey("SubProductC")]
    [InverseProperty("Products")]
    public virtual SubProductC SubProductCNavigation { get; set; } = null!;
}
