using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Operations_Stage_1")]
[Index("CsrOpartationalId", Name = "unq_Operations_Stage_1_CSR_Opartational_ID", IsUnique = true)]
[Index("CustomerId", Name = "unq_Operations_Stage_1_Customer_ID", IsUnique = true)]
[Index("CustomerId", "OrderId", Name = "unq_Operations_Stage_1_Customer_ID_0", IsUnique = true)]
[Index("OperationalId", Name = "unq_Operations_Stage_1_Operational_ID", IsUnique = true)]
public partial class OperationsStage1
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("Order_ID")]
    public int? OrderId { get; set; }

    [Required]
    [Column("CSR_Opartational_ID")]
    public int? CsrOpartationalId { get; set; }

    [Required]
    [Column("Operational_ID")]
    public int? OperationalId { get; set; }

    [Required]
    [Column("Customer_ID")]
    public int? CustomerId { get; set; }

    [Column("Sales_ID")]
    public int? SalesId { get; set; }

    [Column("Operations_ID")]
    public int? OperationsId { get; set; }

    [Column("SubService_A")]
    public int? SubServiceA { get; set; }

    [Column("SubService_B")]
    public int? SubServiceB { get; set; }

    [Column("SubService_C")]
    public int? SubServiceC { get; set; }

    [Column("SubProduct_A")]
    public int? SubProductA { get; set; }

    [Column("SubProduct_B")]
    public int? SubProductB { get; set; }

    [Column("SubProduct_C")]
    public int? SubProductC { get; set; }

    [InverseProperty("OperationsStage1")]
    public virtual ICollection<ClientOrder> ClientOrders { get; set; } = new List<ClientOrder>();

    [InverseProperty("CsrOpartational3")]
    public virtual Csr1? Csr1 { get; set; }

    [InverseProperty("CsrOpartational3")]
    public virtual Csr2? Csr2 { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("OperationsStage1")]
    public virtual Client Customer { get; set; } = null!;

    [InverseProperty("ModelDbMuteP1Operations3")]
    public virtual ICollection<ModelDbMuteP1> ModelDbMuteP1s { get; set; } = new List<ModelDbMuteP1>();

    [InverseProperty("Operational3")]
    public virtual Operations1? Operations1 { get; set; }

    [InverseProperty("Operational3")]
    public virtual Operations2? Operations2 { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OperationsStage1s")]
    public virtual OperationsStage2 Order { get; set; } = null!;

    [ForeignKey("SubProductA, SubProductB, SubProductC")]
    [InverseProperty("OperationsStage1s")]
    public virtual Product? Product { get; set; }

    [ForeignKey("SubServiceA, SubServiceB, SubServiceC")]
    [InverseProperty("OperationsStage1s")]
    public virtual Service? Service { get; set; }
}
