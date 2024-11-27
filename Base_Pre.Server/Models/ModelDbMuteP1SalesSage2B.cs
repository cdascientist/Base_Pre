using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Model_DB_Mute_P1_Sales_Sage2_B")]
public partial class ModelDbMuteP1SalesSage2B
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("DB_Mute_P1_Customer_ID")]
    public int? DbMuteP1CustomerId { get; set; }

    public bool? Data { get; set; }

    [ForeignKey("DbMuteP1CustomerId")]
    [InverseProperty("ModelDbMuteP1SalesSage2Bs")]
    public virtual ModelDbMuteP1Sale? DbMuteP1Customer { get; set; }
}
