﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Client_Information")]
[Index("ClientId", Name = "unq_Client_Information_Client_ID", IsUnique = true)]
public partial class ClientInformation
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Client_First_Name")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ClientFirstName { get; set; }

    [Column("Client_LastName")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ClientLastName { get; set; }

    [Column("Cleint_Phone")]
    [StringLength(30)]
    [Unicode(false)]
    public string? CleintPhone { get; set; }

    [Column("Client_Address")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ClientAddress { get; set; }

    [Required]
    [Column("Client_ID")]
    public int? ClientId { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("ClientInformation")]
    public virtual Client? Client { get; set; }

    [InverseProperty("GenerateClient")]
    public virtual ICollection<Start> Starts { get; set; } = new List<Start>();
}
