using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Client_Order")]
[Index("OrderId", Name = "unq_Client_Order_Order_ID", IsUnique = true)]
public partial class ClientOrder
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("Order_ID")]
    public int? OrderId { get; set; }

    [Column("Customer_ID")]
    public int? CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("ClientOrders")]
    public virtual ModelDbInit? Customer { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<ModelDbInitOperation> ModelDbInitOperations { get; set; } = new List<ModelDbInitOperation>();

    [InverseProperty("Order")]
    public virtual ICollection<ModelDbInitQa> ModelDbInitQas { get; set; } = new List<ModelDbInitQa>();

    [InverseProperty("Order")]
    public virtual ICollection<ModelDbInitSale> ModelDbInitSales { get; set; } = new List<ModelDbInitSale>();
}
