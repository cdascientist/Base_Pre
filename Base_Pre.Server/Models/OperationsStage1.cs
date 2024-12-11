using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Operations_Stage_1")]
public partial class OperationsStage1
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Order_ID")]
    public int? OrderId { get; set; }

    [Column("CSR_Opartational_ID")]
    public int? CsrOpartationalId { get; set; }

    [Column("Operational_ID")]
    public int? OperationalId { get; set; }

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

    [Unicode(false)]
    public string? Data { get; set; }

    [Column("Operations_Stage_One_Product_Vector")]
    [StringLength(100)]
    [Unicode(false)]
    public string? OperationsStageOneProductVector { get; set; }

    [Column("Operations_Stage_One_Service_Vector")]
    [StringLength(100)]
    [Unicode(false)]
    public string? OperationsStageOneServiceVector { get; set; }

    [ForeignKey("OperationsId")]
    [InverseProperty("OperationsStage1s")]
    public virtual ModelDbInitOperation? Operations { get; set; }

    [ForeignKey("SubProductA")]
    [InverseProperty("OperationsStage1s")]
    public virtual SubProductum? SubProductANavigation { get; set; }

    [ForeignKey("SubProductB")]
    [InverseProperty("OperationsStage1s")]
    public virtual SubProductB? SubProductBNavigation { get; set; }

    [ForeignKey("SubProductC")]
    [InverseProperty("OperationsStage1s")]
    public virtual SubProductC? SubProductCNavigation { get; set; }

    [ForeignKey("SubServiceA")]
    [InverseProperty("OperationsStage1s")]
    public virtual SubServiceA? SubServiceANavigation { get; set; }

    [ForeignKey("SubServiceB")]
    [InverseProperty("OperationsStage1s")]
    public virtual SubServiceB? SubServiceBNavigation { get; set; }

    [ForeignKey("SubServiceC")]
    [InverseProperty("OperationsStage1s")]
    public virtual SubServiceC? SubServiceCNavigation { get; set; }
}
