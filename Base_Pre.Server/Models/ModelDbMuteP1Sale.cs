using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Model_DB_Mute_P1_Sales")]
[Index("DbMuteP1CustomerId", Name = "unq_Model_DB_Mute_P1_Sales_DB_Mute_P1_Customer_ID", IsUnique = true)]
public partial class ModelDbMuteP1Sale
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Model_DB_Init_ID")]
    public int? ModelDbInitId { get; set; }

    [Required]
    [Column("DB_Mute_P1_Customer_ID")]
    public int? DbMuteP1CustomerId { get; set; }

    [Column("Model_DB_Mute_P1_Sales_TimeStamp", TypeName = "smalldatetime")]
    public DateTime? ModelDbMuteP1SalesTimeStamp { get; set; }

    public bool? Data { get; set; }

    [ForeignKey("DbMuteP1CustomerId")]
    [InverseProperty("ModelDbMuteP1Sale")]
    public virtual ModelDbMuteP1? DbMuteP1Customer { get; set; }

    [ForeignKey("ModelDbInitId")]
    [InverseProperty("ModelDbMuteP1Sales")]
    public virtual ModelDbInit? ModelDbInit { get; set; }

    [InverseProperty("DbMuteP1Customer")]
    public virtual ICollection<ModelDbMuteP1SalesSage2A> ModelDbMuteP1SalesSage2As { get; set; } = new List<ModelDbMuteP1SalesSage2A>();

    [InverseProperty("DbMuteP1Customer")]
    public virtual ICollection<ModelDbMuteP1SalesSage2B> ModelDbMuteP1SalesSage2Bs { get; set; } = new List<ModelDbMuteP1SalesSage2B>();
}
