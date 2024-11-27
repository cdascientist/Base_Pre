using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

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
