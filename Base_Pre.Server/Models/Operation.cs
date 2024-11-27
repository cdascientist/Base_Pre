using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Index("EmployeeQaId", Name = "unq_Operations_Employee_QA_ID", IsUnique = true)]
[Index("EmployeeSalesId", Name = "unq_Operations_Employee_Sales_ID", IsUnique = true)]
[Index("OperationalId", Name = "unq_Operations_Operational_ID", IsUnique = true)]
public partial class Operation
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Operational_ID")]
    public int? OperationalId { get; set; }

    [Column("Employee_Sales_ID")]
    public int? EmployeeSalesId { get; set; }

    [Column("Employee_Operations_ID")]
    public int? EmployeeOperationsId { get; set; }

    [Column("Employee_QA_ID")]
    public int? EmployeeQaId { get; set; }
}
