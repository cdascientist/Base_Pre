using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Employee_QA")]
public partial class EmployeeQa
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Employee_QA_ID")]
    public int? EmployeeQaId { get; set; }

    [ForeignKey("EmployeeQaId")]
    [InverseProperty("EmployeeQas")]
    public virtual Operation? EmployeeQa1 { get; set; }

    [ForeignKey("EmployeeQaId")]
    [InverseProperty("EmployeeQas")]
    public virtual Csr? EmployeeQaNavigation { get; set; }
}
