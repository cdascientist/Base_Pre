using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Model_DB_Mute_P1_QA")]
[Index("DbMuteP1CustomerId", Name = "unq_Model_DB_Mute_P1_QA_DB_Mute_P1_Customer_ID", IsUnique = true)]
public partial class ModelDbMuteP1Qa
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Model_DB_Init_ID")]
    public int? ModelDbInitId { get; set; }

    [Required]
    [Column("DB_Mute_P1_Customer_ID")]
    public int? DbMuteP1CustomerId { get; set; }

    [Column("Model_DB_Mute_P1_QA_TimeStamp", TypeName = "smalldatetime")]
    public DateTime? ModelDbMuteP1QaTimeStamp { get; set; }

    public bool? Data { get; set; }

    [ForeignKey("DbMuteP1CustomerId")]
    [InverseProperty("ModelDbMuteP1Qa")]
    public virtual ModelDbMuteP1? DbMuteP1Customer { get; set; }

    [ForeignKey("ModelDbInitId")]
    [InverseProperty("ModelDbMuteP1Qas")]
    public virtual ModelDbInit? ModelDbInit { get; set; }

    [InverseProperty("DbMuteP1Customer")]
    public virtual ICollection<ModelDbMuteP1QaStage2A> ModelDbMuteP1QaStage2As { get; set; } = new List<ModelDbMuteP1QaStage2A>();

    [InverseProperty("DbMuteP1Customer")]
    public virtual ICollection<ModelDbMuteP1QaStage2B> ModelDbMuteP1QaStage2Bs { get; set; } = new List<ModelDbMuteP1QaStage2B>();
}
