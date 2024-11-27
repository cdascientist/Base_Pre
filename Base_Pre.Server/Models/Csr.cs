using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("CSR")]
[Index("CsrOpartationalId", Name = "unq_CSR_CSR_Opartational_ID", IsUnique = true)]
[Index("EmployeeQaId", Name = "unq_CSR_Employee_QA_ID", IsUnique = true)]
[Index("EmployeeSalesId", Name = "unq_CSR_Employee_Sales_ID", IsUnique = true)]
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
