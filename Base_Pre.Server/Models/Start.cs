using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Start")]
public partial class Start
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Generate_Client_ID")]
    public int? GenerateClientId { get; set; }

    [Column("Client_Name")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [ForeignKey("GenerateClientId")]
    [InverseProperty("Starts")]
    public virtual ClientInformation? GenerateClient { get; set; }
}
