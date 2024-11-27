using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Model_DB_Mute_P1_Operations_Stage2_B")]
public partial class ModelDbMuteP1OperationsStage2B
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("DB_Mute_P1_Customer_ID")]
    public int? DbMuteP1CustomerId { get; set; }

    public byte[]? Data { get; set; }

    [Column("Operational_ID")]
    public int? OperationalId { get; set; }

    [Column("Employee_Operations_ID")]
    public int? EmployeeOperationsId { get; set; }
}
