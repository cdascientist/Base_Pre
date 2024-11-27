using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("CSR")]
public partial class Csr
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("CSR_Opartational_ID")]
    public int? CsrOpartationalId { get; set; }

    [Column("Employee_Sales_ID")]
    public int? EmployeeSalesId { get; set; }

    [Column("Employee_Operations_ID")]
    public int? EmployeeOperationsId { get; set; }

    [Column("Employee_QA_ID")]
    public int? EmployeeQaId { get; set; }
}
