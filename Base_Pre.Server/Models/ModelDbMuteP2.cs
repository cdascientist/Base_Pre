using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Model_DB_Mute_P2")]
public partial class ModelDbMuteP2
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Model_DB_Mute_P1_ID")]
    public int? ModelDbMuteP1Id { get; set; }

    [ForeignKey("ModelDbMuteP1Id")]
    [InverseProperty("ModelDbMuteP2s")]
    public virtual ModelDbMuteP1? ModelDbMuteP1 { get; set; }
}
