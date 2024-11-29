using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Client_Information")]
[Index("CustomerId", Name = "unq_Client_Information_Customer_ID", IsUnique = true)]
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

    [Column("Customer_ID")]
    public int? CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("ClientInformation")]
    public virtual ModelDbInit? Customer { get; set; }
}
