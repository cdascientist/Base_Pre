using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

[Table("Employee_Sales")]
public partial class EmployeeSale
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Employee_Sales_ID")]
    public int? EmployeeSalesId { get; set; }

    [ForeignKey("EmployeeSalesId")]
    [InverseProperty("EmployeeSales")]
    public virtual Csr? EmployeeSales { get; set; }

    [ForeignKey("EmployeeSalesId")]
    [InverseProperty("EmployeeSales")]
    public virtual Operation? EmployeeSalesNavigation { get; set; }
}
