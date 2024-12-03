using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Model_DB_Init_Operations")]
[Index("OperationsId", Name = "unq_Model_DB_Init_Operations_Operations_ID", IsUnique = true)]
public partial class ModelDbInitOperation
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Customer_ID")]
    public int? CustomerId { get; set; }

    [Unicode(false)]
    public string? Data { get; set; }

    [Required]
    [Column("Operations_ID")]
    public int? OperationsId { get; set; }

    [Column("Order_ID")]
    public int? OrderId { get; set; }

    [InverseProperty("Operations")]
    public virtual ICollection<OperationsStage1> OperationsStage1s { get; set; } = new List<OperationsStage1>();

    [ForeignKey("OrderId")]
    [InverseProperty("ModelDbInitOperations")]
    public virtual ClientOrder? Order { get; set; }
}
