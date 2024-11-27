using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Model_DB_Mute_P1_Operations")]
[Index("DbMuteP1CustomerId", Name = "unq_Model_DB_Mute_P1_Operations_DB_Mute_P1_Customer_ID", IsUnique = true)]
public partial class ModelDbMuteP1Operation
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Model_DB_Init_ID")]
    public int? ModelDbInitId { get; set; }

    [Column("DB_Mute_P1_Customer_ID")]
    public int? DbMuteP1CustomerId { get; set; }

    [Column("Model_DB_Mute_P1_Operations_TimeStamp", TypeName = "smalldatetime")]
    public DateTime? ModelDbMuteP1OperationsTimeStamp { get; set; }

    public bool? Data { get; set; }

    [Column("Operational_ID")]
    public int? OperationalId { get; set; }

    [Column("Employee_Operations_ID")]
    public int? EmployeeOperationsId { get; set; }
}
