using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("CSR_2")]
[Index("CsrOpartationalId", Name = "unq_CSR_2_CSR_Opartational_ID", IsUnique = true)]
public partial class Csr2
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("CSR_Opartational_ID")]
    public int? CsrOpartationalId { get; set; }
}
