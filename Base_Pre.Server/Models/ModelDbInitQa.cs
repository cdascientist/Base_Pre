using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Model_DB__Init_QA")]
public partial class ModelDbInitQa
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Customer_ID")]
    public int? CustomerId { get; set; }

    [Unicode(false)]
    public string? Data { get; set; }

    [Column("Order_ID")]
    public int? OrderId { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("ModelDbInitQas")]
    public virtual ClientOrder? Order { get; set; }
}
