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

    [Required]
    [Column("CSR_Opartational_ID")]
    public int? CsrOpartationalId { get; set; }

    [Required]
    [Column("Employee_Sales_ID")]
    public int? EmployeeSalesId { get; set; }

    [Column("Employee_Operations_ID")]
    public int? EmployeeOperationsId { get; set; }

    [Required]
    [Column("Employee_QA_ID")]
    public int? EmployeeQaId { get; set; }

    [ForeignKey("CsrOpartationalId")]
    [InverseProperty("Csr")]
    public virtual Csr1? CsrOpartational { get; set; }

    [ForeignKey("CsrOpartationalId")]
    [InverseProperty("Csr")]
    public virtual Csr2? CsrOpartationalNavigation { get; set; }

    [InverseProperty("EmployeeOperations")]
    public virtual EmployeeOperation? EmployeeOperation { get; set; }

    [InverseProperty("EmployeeQaNavigation")]
    public virtual ICollection<EmployeeQa> EmployeeQas { get; set; } = new List<EmployeeQa>();

    [InverseProperty("EmployeeSales")]
    public virtual ICollection<EmployeeSale> EmployeeSales { get; set; } = new List<EmployeeSale>();
}
