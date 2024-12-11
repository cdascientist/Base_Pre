using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Model_DB_Init")]
[Index("CustomerId", Name = "unq_Model_DB_Init_Customer_ID", IsUnique = true)]
public partial class ModelDbInit
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Model_DB_Init_TimeStamp", TypeName = "smalldatetime")]
    public DateTime? ModelDbInitTimeStamp { get; set; }

    [Column("Model_DB_Init_Catagorical_ID")]
    public int? ModelDbInitCatagoricalId { get; set; }

    [Column("Model_DB_Init_Catagorical_Name")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ModelDbInitCatagoricalName { get; set; }

    [Column("Model_DB_Init_ModelData")]
    public bool? ModelDbInitModelData { get; set; }

    public byte[]? Data { get; set; }

    [Required]
    [Column("Customer_ID")]
    public int? CustomerId { get; set; }

    [Column("Model_DB_Init_Product_Vector")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ModelDbInitProductVector { get; set; }

    [Column("Model_DB_Init_Service_Vector")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ModelDbInitServiceVector { get; set; }

    [InverseProperty("Customer")]
    public virtual ClientInformation? ClientInformation { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<ClientOrder> ClientOrders { get; set; } = new List<ClientOrder>();
}
