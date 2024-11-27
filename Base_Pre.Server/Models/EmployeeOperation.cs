using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Employee_Operations")]
[Index("EmployeeOperationsId", Name = "unq_Employee_Operations_Employee_Operations_ID", IsUnique = true)]
public partial class EmployeeOperation
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Employee_Operations_ID")]
    public int? EmployeeOperationsId { get; set; }
}
