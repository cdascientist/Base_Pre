﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Client_Order")]
[Index("CustomerId", Name = "unq_Client_Order_Customer_ID", IsUnique = true)]
[Index("OrderId", "CustomerId", "ClientId", Name = "unq_Client_Order_Order_ID", IsUnique = true)]
public partial class ClientOrder
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Client_ID")]
    public int? ClientId { get; set; }

    [Column("Order_ID")]
    public int? OrderId { get; set; }

    [Required]
    [Column("Customer_ID")]
    public int? CustomerId { get; set; }

    [InverseProperty("Customer")]
    public virtual Client? Client { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("ClientOrders")]
    public virtual Client? ClientNavigation { get; set; }

    [ForeignKey("CustomerId, OrderId")]
    [InverseProperty("ClientOrders")]
    public virtual OperationsStage1? OperationsStage1 { get; set; }
}
