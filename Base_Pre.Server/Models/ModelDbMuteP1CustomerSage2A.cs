using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Model_DB_Mute_P1_Customer_Sage2_A")]
public partial class ModelDbMuteP1CustomerSage2A
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Model_Mute_P1_Cutstomer_ID")]
    public int? ModelMuteP1CutstomerId { get; set; }

    public bool? Data { get; set; }
}
