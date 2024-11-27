using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Client")]
[Index("ClientId", Name = "unq_Client_Client_ID", IsUnique = true)]
[Index("CustomerId", Name = "unq_Client_Customer_ID", IsUnique = true)]
public partial class Client
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("Client_ID")]
    public int? ClientId { get; set; }

    [Required]
    [Column("Customer_ID")]
    public int? CustomerId { get; set; }

    [InverseProperty("Client")]
    public virtual ClientInformation? ClientInformation { get; set; }

    [InverseProperty("ClientNavigation")]
    public virtual ICollection<ClientOrder> ClientOrders { get; set; } = new List<ClientOrder>();

    [ForeignKey("CustomerId")]
    [InverseProperty("Client")]
    public virtual ClientOrder? Customer { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Client")]
    public virtual ModelDbInit? CustomerNavigation { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<IterationCycle1> IterationCycle1s { get; set; } = new List<IterationCycle1>();

    [InverseProperty("Customer")]
    public virtual ICollection<IterationCycle2> IterationCycle2s { get; set; } = new List<IterationCycle2>();

    [InverseProperty("Customer")]
    public virtual ICollection<IterationCycle3> IterationCycle3s { get; set; } = new List<IterationCycle3>();

    [InverseProperty("Customer")]
    public virtual ICollection<IterationCycle4> IterationCycle4s { get; set; } = new List<IterationCycle4>();

    [InverseProperty("Customer")]
    public virtual OperationsStage1? OperationsStage1 { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<OperationsStage2> OperationsStage2s { get; set; } = new List<OperationsStage2>();

    [InverseProperty("Customer")]
    public virtual ICollection<OperationsStage3> OperationsStage3s { get; set; } = new List<OperationsStage3>();

    [InverseProperty("Customer")]
    public virtual ICollection<OperationsStage4> OperationsStage4s { get; set; } = new List<OperationsStage4>();
}
