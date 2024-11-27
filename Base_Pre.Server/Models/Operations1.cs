using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Operations_1")]
public partial class Operations1
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Operational_ID")]
    public int? OperationalId { get; set; }
}
