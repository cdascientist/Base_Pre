using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Client")]
public partial class Client
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Client_ID")]
    public int? ClientId { get; set; }

    [Column("Customer_ID")]
    public int? CustomerId { get; set; }
}
