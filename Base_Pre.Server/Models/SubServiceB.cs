using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("SubService_B")]
public partial class SubServiceB
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Service_Name")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ServiceName { get; set; }

    [Column("Service_Type")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ServiceType { get; set; }

    public int? Quantity { get; set; }

    [Column(TypeName = "money")]
    public decimal? Price { get; set; }

    [InverseProperty("SubServiceBNavigation")]
    public virtual ICollection<OperationsStage1> OperationsStage1s { get; set; } = new List<OperationsStage1>();
}
