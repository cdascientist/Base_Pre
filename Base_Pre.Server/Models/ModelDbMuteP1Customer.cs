using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Model_DB_Mute_P1_Customer")]
[Index("DbMuteP1CustomerId", Name = "unq_Model_DB_Mute_P1_Customer_DB_Mute_P1_Customer_ID", IsUnique = true)]
public partial class ModelDbMuteP1Customer
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Model_DB_Init_ID")]
    public int? ModelDbInitId { get; set; }

    [Required]
    [Column("DB_Mute_P1_Customer_ID")]
    public int? DbMuteP1CustomerId { get; set; }

    [Column("Model_DB_Mute_P1_Cutomer_TimeStamp", TypeName = "smalldatetime")]
    public DateTime? ModelDbMuteP1CutomerTimeStamp { get; set; }

    public bool? Data { get; set; }

    [ForeignKey("ModelDbInitId")]
    [InverseProperty("ModelDbMuteP1Customers")]
    public virtual ModelDbInit? ModelDbInit { get; set; }

    [ForeignKey("ModelDbInitId")]
    [InverseProperty("ModelDbMuteP1Customers")]
    public virtual ModelDbMuteP1? ModelDbInitNavigation { get; set; }

    [InverseProperty("ModelMuteP1Cutstomer")]
    public virtual ICollection<ModelDbMuteP1CustomerSage2A> ModelDbMuteP1CustomerSage2As { get; set; } = new List<ModelDbMuteP1CustomerSage2A>();

    [InverseProperty("ModelMuteP1Cutstomer")]
    public virtual ICollection<ModelDbMuteP1CustomerSage2B> ModelDbMuteP1CustomerSage2Bs { get; set; } = new List<ModelDbMuteP1CustomerSage2B>();
}
