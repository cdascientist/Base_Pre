using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Model_DB_Mute_P1_QA_Stage2_A")]
public partial class ModelDbMuteP1QaStage2A
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("DB_Mute_P1_Customer_ID")]
    public int? DbMuteP1CustomerId { get; set; }

    public bool? Data { get; set; }
}
