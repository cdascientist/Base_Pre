using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Client_Order")]
public partial class ClientOrder
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Client_ID")]
    public int? ClientId { get; set; }

    [Column("Order_ID")]
    public int? OrderId { get; set; }

    [Column("Customer_ID")]
    public int? CustomerId { get; set; }
}
