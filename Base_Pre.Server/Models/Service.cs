using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Service")]
[Index("SubServiceA", "SubServiceB", "SubServiceC", Name = "unq_Service_SubService_A", IsUnique = true)]
[Index("SubServiceA", "SubServiceB", Name = "unq_Service_SubService_A_0", IsUnique = true)]
public partial class Service
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("SubService_A")]
    public int? SubServiceA { get; set; }

    [Column("SubService_B")]
    public int? SubServiceB { get; set; }

    [Column("SubService_C")]
    public int? SubServiceC { get; set; }
}
