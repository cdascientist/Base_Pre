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

    [Column("SubProduct_A")]
    public int? SubProductA { get; set; }

    [Column("SubProduct_B")]
    public int? SubProductB { get; set; }

    [Column("SubProduct_C")]
    public int? SubProductC { get; set; }
}
