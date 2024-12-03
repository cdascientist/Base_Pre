using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("SubProduct_A")]
public partial class SubProductum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Product_Name")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ProductName { get; set; }

    [Column("Product_Type")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ProductType { get; set; }

    public int? Quantity { get; set; }

    [Column(TypeName = "money")]
    public decimal? Price { get; set; }

    [InverseProperty("SubProductANavigation")]
    public virtual ICollection<OperationsStage1> OperationsStage1s { get; set; } = new List<OperationsStage1>();
}
