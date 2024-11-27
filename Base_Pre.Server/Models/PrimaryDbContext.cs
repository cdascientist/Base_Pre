using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Base_Pre.Server.Models;

public partial class PrimaryDbContext : DbContext
{
    public PrimaryDbContext()
    {
    }

    public PrimaryDbContext(DbContextOptions<PrimaryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carrier> Carriers { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientInformation> ClientInformations { get; set; }

    public virtual DbSet<ClientOrder> ClientOrders { get; set; }

    public virtual DbSet<Csr> Csrs { get; set; }

    public virtual DbSet<Csr1> Csr1s { get; set; }

    public virtual DbSet<Csr2> Csr2s { get; set; }

    public virtual DbSet<EmployeeOperation> EmployeeOperations { get; set; }

    public virtual DbSet<EmployeeQa> EmployeeQas { get; set; }

    public virtual DbSet<EmployeeSale> EmployeeSales { get; set; }

    public virtual DbSet<IterationCycle1> IterationCycle1s { get; set; }

    public virtual DbSet<IterationCycle2> IterationCycle2s { get; set; }

    public virtual DbSet<IterationCycle3> IterationCycle3s { get; set; }

    public virtual DbSet<IterationCycle4> IterationCycle4s { get; set; }

    public virtual DbSet<ModelDbInit> ModelDbInits { get; set; }

    public virtual DbSet<ModelDbMuteP1> ModelDbMuteP1s { get; set; }

    public virtual DbSet<ModelDbMuteP1Customer> ModelDbMuteP1Customers { get; set; }

    public virtual DbSet<ModelDbMuteP1CustomerSage2A> ModelDbMuteP1CustomerSage2As { get; set; }

    public virtual DbSet<ModelDbMuteP1CustomerSage2B> ModelDbMuteP1CustomerSage2Bs { get; set; }

    public virtual DbSet<ModelDbMuteP1Operation> ModelDbMuteP1Operations { get; set; }

    public virtual DbSet<ModelDbMuteP1OperationsStage2A> ModelDbMuteP1OperationsStage2As { get; set; }

    public virtual DbSet<ModelDbMuteP1OperationsStage2B> ModelDbMuteP1OperationsStage2Bs { get; set; }

    public virtual DbSet<ModelDbMuteP1Qa> ModelDbMuteP1Qas { get; set; }

    public virtual DbSet<ModelDbMuteP1QaStage2A> ModelDbMuteP1QaStage2As { get; set; }

    public virtual DbSet<ModelDbMuteP1QaStage2B> ModelDbMuteP1QaStage2Bs { get; set; }

    public virtual DbSet<ModelDbMuteP1Sale> ModelDbMuteP1Sales { get; set; }

    public virtual DbSet<ModelDbMuteP1SalesSage2A> ModelDbMuteP1SalesSage2As { get; set; }

    public virtual DbSet<ModelDbMuteP1SalesSage2B> ModelDbMuteP1SalesSage2Bs { get; set; }

    public virtual DbSet<ModelDbMuteP2> ModelDbMuteP2s { get; set; }

    public virtual DbSet<Operation> Operations { get; set; }

    public virtual DbSet<Operations1> Operations1s { get; set; }

    public virtual DbSet<Operations2> Operations2s { get; set; }

    public virtual DbSet<OperationsStage1> OperationsStage1s { get; set; }

    public virtual DbSet<OperationsStage2> OperationsStage2s { get; set; }

    public virtual DbSet<OperationsStage3> OperationsStage3s { get; set; }

    public virtual DbSet<OperationsStage4> OperationsStage4s { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Start> Starts { get; set; }

    public virtual DbSet<SubProductB> SubProductBs { get; set; }

    public virtual DbSet<SubProductC> SubProductCs { get; set; }

    public virtual DbSet<SubProductum> SubProductAs { get; set; }

    public virtual DbSet<SubServiceA> SubServiceAs { get; set; }

    public virtual DbSet<SubServiceB> SubServiceBs { get; set; }

    public virtual DbSet<SubServiceC> SubServiceCs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=cms2024.database.windows.net;Initial Catalog=Primary;User ID=cms;Password=Badpassword1!;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carrier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Carrier");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Client");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithOne(p => p.Client)
                .HasPrincipalKey<ClientOrder>(p => p.CustomerId)
                .HasForeignKey<Client>(d => d.CustomerId)
                .HasConstraintName("fk_client_client_order");

            entity.HasOne(d => d.CustomerNavigation).WithOne(p => p.Client)
                .HasPrincipalKey<ModelDbInit>(p => p.CustomerId)
                .HasForeignKey<Client>(d => d.CustomerId)
                .HasConstraintName("fk_client_model_db_init");
        });

        modelBuilder.Entity<ClientInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Client_Information");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Client).WithOne(p => p.ClientInformation)
                .HasPrincipalKey<Client>(p => p.ClientId)
                .HasForeignKey<ClientInformation>(d => d.ClientId)
                .HasConstraintName("fk_client_information_client");
        });

        modelBuilder.Entity<ClientOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_CLient_Order");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ClientNavigation).WithMany(p => p.ClientOrders)
                .HasPrincipalKey(p => p.ClientId)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("fk_client_order_client");

            entity.HasOne(d => d.OperationsStage1).WithMany(p => p.ClientOrders)
                .HasPrincipalKey(p => new { p.CustomerId, p.OrderId })
                .HasForeignKey(d => new { d.CustomerId, d.OrderId })
                .HasConstraintName("fk_client_order");
        });

        modelBuilder.Entity<Csr>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_CSR");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.CsrOpartational).WithOne(p => p.Csr)
                .HasPrincipalKey<Csr1>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr>(d => d.CsrOpartationalId)
                .HasConstraintName("fk_csr_csr_1");

            entity.HasOne(d => d.CsrOpartationalNavigation).WithOne(p => p.Csr)
                .HasPrincipalKey<Csr2>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr>(d => d.CsrOpartationalId)
                .HasConstraintName("fk_csr_csr_2");
        });

        modelBuilder.Entity<Csr1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_CSR_1");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.CsrOpartational).WithOne(p => p.Csr1)
                .HasPrincipalKey<IterationCycle1>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr1>(d => d.CsrOpartationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_csr_1_iteration_cycle_1");

            entity.HasOne(d => d.CsrOpartationalNavigation).WithOne(p => p.Csr1)
                .HasPrincipalKey<IterationCycle2>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr1>(d => d.CsrOpartationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_csr_1_iteration_cycle_2");

            entity.HasOne(d => d.CsrOpartational1).WithOne(p => p.Csr1)
                .HasPrincipalKey<IterationCycle3>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr1>(d => d.CsrOpartationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_csr_1_iteration_cycle_3");

            entity.HasOne(d => d.CsrOpartational2).WithOne(p => p.Csr1)
                .HasPrincipalKey<IterationCycle4>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr1>(d => d.CsrOpartationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_csr_1_iteration_cycle_4");

            entity.HasOne(d => d.CsrOpartational3).WithOne(p => p.Csr1)
                .HasPrincipalKey<OperationsStage1>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr1>(d => d.CsrOpartationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_csr_1_operations_stage_1");

            entity.HasOne(d => d.CsrOpartational4).WithOne(p => p.Csr1)
                .HasPrincipalKey<OperationsStage2>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr1>(d => d.CsrOpartationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_csr_1_operations_stage_2");

            entity.HasOne(d => d.CsrOpartational5).WithOne(p => p.Csr1)
                .HasPrincipalKey<OperationsStage3>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr1>(d => d.CsrOpartationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_csr_1_operations_stage_3");

            entity.HasOne(d => d.CsrOpartational6).WithOne(p => p.Csr1)
                .HasPrincipalKey<OperationsStage4>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr1>(d => d.CsrOpartationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_csr_1_operations_stage_4");
        });

        modelBuilder.Entity<Csr2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_CSR_2");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.CsrOpartational).WithOne(p => p.Csr2)
                .HasPrincipalKey<IterationCycle1>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr2>(d => d.CsrOpartationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_csr_2_iteration_cycle_1");

            entity.HasOne(d => d.CsrOpartationalNavigation).WithOne(p => p.Csr2)
                .HasPrincipalKey<IterationCycle2>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr2>(d => d.CsrOpartationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_csr_2_iteration_cycle_2");

            entity.HasOne(d => d.CsrOpartational1).WithOne(p => p.Csr2)
                .HasPrincipalKey<IterationCycle3>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr2>(d => d.CsrOpartationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_csr_2_iteration_cycle_3");

            entity.HasOne(d => d.CsrOpartational2).WithOne(p => p.Csr2)
                .HasPrincipalKey<IterationCycle4>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr2>(d => d.CsrOpartationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_csr_2_iteration_cycle_4");

            entity.HasOne(d => d.CsrOpartational3).WithOne(p => p.Csr2)
                .HasPrincipalKey<OperationsStage1>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr2>(d => d.CsrOpartationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_csr_2_operations_stage_1");

            entity.HasOne(d => d.CsrOpartational4).WithOne(p => p.Csr2)
                .HasPrincipalKey<OperationsStage2>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr2>(d => d.CsrOpartationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_csr_2_operations_2");

            entity.HasOne(d => d.CsrOpartational5).WithOne(p => p.Csr2)
                .HasPrincipalKey<OperationsStage3>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr2>(d => d.CsrOpartationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_csr_2_operations_stage_3");

            entity.HasOne(d => d.CsrOpartational6).WithOne(p => p.Csr2)
                .HasPrincipalKey<OperationsStage4>(p => p.CsrOpartationalId)
                .HasForeignKey<Csr2>(d => d.CsrOpartationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_csr_2_operations_stage_4");
        });

        modelBuilder.Entity<EmployeeOperation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Employee_Operations");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.EmployeeOperations).WithOne(p => p.EmployeeOperation)
                .HasPrincipalKey<Csr>(p => p.CsrOpartationalId)
                .HasForeignKey<EmployeeOperation>(d => d.EmployeeOperationsId)
                .HasConstraintName("fk_employee_operations_csr");

            entity.HasOne(d => d.EmployeeOperationsNavigation).WithOne(p => p.EmployeeOperation)
                .HasPrincipalKey<Operation>(p => p.OperationalId)
                .HasForeignKey<EmployeeOperation>(d => d.EmployeeOperationsId)
                .HasConstraintName("fk_employee_operations");
        });

        modelBuilder.Entity<EmployeeQa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Employee_QA");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.EmployeeQaNavigation).WithMany(p => p.EmployeeQas)
                .HasPrincipalKey(p => p.EmployeeQaId)
                .HasForeignKey(d => d.EmployeeQaId)
                .HasConstraintName("fk_employee_qa_csr");

            entity.HasOne(d => d.EmployeeQa1).WithMany(p => p.EmployeeQas)
                .HasPrincipalKey(p => p.EmployeeQaId)
                .HasForeignKey(d => d.EmployeeQaId)
                .HasConstraintName("fk_employee_qa_operations");
        });

        modelBuilder.Entity<EmployeeSale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Employee_Sales");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.EmployeeSales).WithMany(p => p.EmployeeSales)
                .HasPrincipalKey(p => p.EmployeeSalesId)
                .HasForeignKey(d => d.EmployeeSalesId)
                .HasConstraintName("fk_employee_sales_csr");

            entity.HasOne(d => d.EmployeeSalesNavigation).WithMany(p => p.EmployeeSales)
                .HasPrincipalKey(p => p.EmployeeSalesId)
                .HasForeignKey(d => d.EmployeeSalesId)
                .HasConstraintName("fk_employee_sales_operations");
        });

        modelBuilder.Entity<IterationCycle1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Iteration_Cycle_1");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithMany(p => p.IterationCycle1s)
                .HasPrincipalKey(p => p.CustomerId)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("fk_iteration_cycle_1_client");

            entity.HasOne(d => d.IterationCycle).WithOne(p => p.IterationCycle1)
                .HasPrincipalKey<IterationCycle2>(p => p.IterationCycleId)
                .HasForeignKey<IterationCycle1>(d => d.IterationCycleId)
                .HasConstraintName("fk_iteration_cycle_1");

            entity.HasOne(d => d.Order).WithOne(p => p.IterationCycle1)
                .HasPrincipalKey<OperationsStage3>(p => p.OrderId)
                .HasForeignKey<IterationCycle1>(d => d.OrderId)
                .HasConstraintName("fk_iteration_cycle_1_2");

            entity.HasOne(d => d.Product).WithMany(p => p.IterationCycle1s)
                .HasPrincipalKey(p => new { p.SubProductA, p.SubProductB, p.SubProductC })
                .HasForeignKey(d => new { d.SubProductA, d.SubProductB, d.SubProductC })
                .HasConstraintName("fk_iteration_cycle_1_product");

            entity.HasOne(d => d.Service).WithMany(p => p.IterationCycle1s)
                .HasPrincipalKey(p => new { p.SubServiceA, p.SubServiceB, p.SubServiceC })
                .HasForeignKey(d => new { d.SubServiceA, d.SubServiceB, d.SubServiceC })
                .HasConstraintName("fk_Iteration_Cycle_5_2");
        });

        modelBuilder.Entity<IterationCycle2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Iteration_Cycle_2");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithMany(p => p.IterationCycle2s)
                .HasPrincipalKey(p => p.CustomerId)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("fk_iteration_cycle_2_client");

            entity.HasOne(d => d.IterationCycle).WithOne(p => p.IterationCycle2)
                .HasPrincipalKey<IterationCycle3>(p => p.IterationCycleId)
                .HasForeignKey<IterationCycle2>(d => d.IterationCycleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_iteration_cycle_2");

            entity.HasOne(d => d.Product).WithMany(p => p.IterationCycle2s)
                .HasPrincipalKey(p => new { p.SubProductA, p.SubProductB, p.SubProductC })
                .HasForeignKey(d => new { d.SubProductA, d.SubProductB, d.SubProductC })
                .HasConstraintName("fk_iteration_cycle_2_product");

            entity.HasOne(d => d.Service).WithMany(p => p.IterationCycle2s)
                .HasPrincipalKey(p => new { p.SubServiceA, p.SubServiceB, p.SubServiceC })
                .HasForeignKey(d => new { d.SubServiceA, d.SubServiceB, d.SubServiceC })
                .HasConstraintName("fk_iteration_cycle_2_service_3");
        });

        modelBuilder.Entity<IterationCycle3>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Iteration_Cycle_3");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithMany(p => p.IterationCycle3s)
                .HasPrincipalKey(p => p.CustomerId)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("fk_iteration_cycle_3_client");

            entity.HasOne(d => d.IterationCycle).WithOne(p => p.IterationCycle3)
                .HasPrincipalKey<IterationCycle4>(p => p.IterationCycleId)
                .HasForeignKey<IterationCycle3>(d => d.IterationCycleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_iteration_cycle_3");

            entity.HasOne(d => d.Product).WithMany(p => p.IterationCycle3s)
                .HasPrincipalKey(p => new { p.SubProductA, p.SubProductB, p.SubProductC })
                .HasForeignKey(d => new { d.SubProductA, d.SubProductB, d.SubProductC })
                .HasConstraintName("fk_iteration_cycle_3_product");

            entity.HasOne(d => d.Service).WithMany(p => p.IterationCycle3s)
                .HasPrincipalKey(p => new { p.SubServiceA, p.SubServiceB, p.SubServiceC })
                .HasForeignKey(d => new { d.SubServiceA, d.SubServiceB, d.SubServiceC })
                .HasConstraintName("fk_Iteration_Cycle_5_3");
        });

        modelBuilder.Entity<IterationCycle4>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Iteration_Cycle_4");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithMany(p => p.IterationCycle4s)
                .HasPrincipalKey(p => p.CustomerId)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("fk_iteration_cycle_4_client");

            entity.HasOne(d => d.IterationCycle).WithOne(p => p.IterationCycle4)
                .HasPrincipalKey<IterationCycle1>(p => p.IterationCycleId)
                .HasForeignKey<IterationCycle4>(d => d.IterationCycleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_iteration_cycle_4");

            entity.HasOne(d => d.Order).WithMany(p => p.IterationCycle4s)
                .HasPrincipalKey(p => p.OrderId)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_iteration_cycle_4_Process");

            entity.HasOne(d => d.Product).WithMany(p => p.IterationCycle4s)
                .HasPrincipalKey(p => new { p.SubProductA, p.SubProductB, p.SubProductC })
                .HasForeignKey(d => new { d.SubProductA, d.SubProductB, d.SubProductC })
                .HasConstraintName("fk_iteration_cycle_4_product");

            entity.HasOne(d => d.Service).WithMany(p => p.IterationCycle4s)
                .HasPrincipalKey(p => new { p.SubServiceA, p.SubServiceB, p.SubServiceC })
                .HasForeignKey(d => new { d.SubServiceA, d.SubServiceB, d.SubServiceC })
                .HasConstraintName("fk_iteration_cycle_4_service");
        });

        modelBuilder.Entity<ModelDbInit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Init");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithOne(p => p.ModelDbInit)
                .HasPrincipalKey<ModelDbMuteP1>(p => p.CustomerId)
                .HasForeignKey<ModelDbInit>(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_model_db_init");
        });

        modelBuilder.Entity<ModelDbMuteP1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ModelDbInitNavigation).WithOne(p => p.ModelDbMuteP1).HasConstraintName("fk_model_db_mute_p1");

            entity.HasOne(d => d.ModelDbMuteP1Operations).WithMany(p => p.ModelDbMuteP1s)
                .HasPrincipalKey(p => p.OperationalId)
                .HasForeignKey(d => d.ModelDbMuteP1OperationsId)
                .HasConstraintName("fk_model_db_mute_p1_AI_Model_Iteration_1");

            entity.HasOne(d => d.ModelDbMuteP1OperationsNavigation).WithMany(p => p.ModelDbMuteP1s)
                .HasPrincipalKey(p => p.OperationalId)
                .HasForeignKey(d => d.ModelDbMuteP1OperationsId)
                .HasConstraintName("fk_Model_DB_Mute_P1_0AI_Model_Iteration_2");

            entity.HasOne(d => d.ModelDbMuteP1Operations1).WithMany(p => p.ModelDbMuteP1s)
                .HasPrincipalKey(p => p.OperationalId)
                .HasForeignKey(d => d.ModelDbMuteP1OperationsId)
                .HasConstraintName("fk_model_db_mute_p1AI_Model_Iteration_3");

            entity.HasOne(d => d.ModelDbMuteP1Operations2).WithMany(p => p.ModelDbMuteP1s)
                .HasPrincipalKey(p => p.OperationalId)
                .HasForeignKey(d => d.ModelDbMuteP1OperationsId)
                .HasConstraintName("fk_model_db_mute_p1AI_Model_Iteration_4");

            entity.HasOne(d => d.ModelDbMuteP1Operations3).WithMany(p => p.ModelDbMuteP1s)
                .HasPrincipalKey(p => p.OperationalId)
                .HasForeignKey(d => d.ModelDbMuteP1OperationsId)
                .HasConstraintName("fk_model_db_mute_p1_AI_Model");

            entity.HasOne(d => d.ModelDbMuteP1Operations4).WithMany(p => p.ModelDbMuteP1s)
                .HasPrincipalKey(p => p.OperationalId)
                .HasForeignKey(d => d.ModelDbMuteP1OperationsId)
                .HasConstraintName("fk_model_db_mute_p1_AI_Model_2");

            entity.HasOne(d => d.ModelDbMuteP1Operations5).WithMany(p => p.ModelDbMuteP1s)
                .HasPrincipalKey(p => p.OperationalId)
                .HasForeignKey(d => d.ModelDbMuteP1OperationsId)
                .HasConstraintName("fk_model_db_mute_p1AI_Model_4");

            entity.HasOne(d => d.ModelDbMuteP1Operations6).WithMany(p => p.ModelDbMuteP1s)
                .HasPrincipalKey(p => p.OperationsId)
                .HasForeignKey(d => d.ModelDbMuteP1OperationsId)
                .HasConstraintName("fk_model_db_mute_p1AI_Model_3");
        });

        modelBuilder.Entity<ModelDbMuteP1Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_Customer");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ModelDbInit).WithMany(p => p.ModelDbMuteP1Customers).HasConstraintName("fk_model_db_mute_p1_customer");

            entity.HasOne(d => d.ModelDbInitNavigation).WithMany(p => p.ModelDbMuteP1Customers)
                .HasPrincipalKey(p => p.ModelDbInitId)
                .HasForeignKey(d => d.ModelDbInitId)
                .HasConstraintName("fk_model_db_mute_p1_customer_2");
        });

        modelBuilder.Entity<ModelDbMuteP1CustomerSage2A>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_Customer_Sage2_A");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ModelMuteP1Cutstomer).WithMany(p => p.ModelDbMuteP1CustomerSage2As)
                .HasPrincipalKey(p => p.DbMuteP1CustomerId)
                .HasForeignKey(d => d.ModelMuteP1CutstomerId)
                .HasConstraintName("fk_model_db_mute_p1_customer_sage2_a");
        });

        modelBuilder.Entity<ModelDbMuteP1CustomerSage2B>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_Customer_Sage2_B");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ModelMuteP1Cutstomer).WithMany(p => p.ModelDbMuteP1CustomerSage2Bs)
                .HasPrincipalKey(p => p.DbMuteP1CustomerId)
                .HasForeignKey(d => d.ModelMuteP1CutstomerId)
                .HasConstraintName("fk_model_db_mute_p1_customer_sage2_b");
        });

        modelBuilder.Entity<ModelDbMuteP1Operation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_Operations");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.DbMuteP1Customer).WithOne(p => p.ModelDbMuteP1Operation)
                .HasPrincipalKey<ModelDbMuteP1>(p => p.ModelDbMuteP1CustomerId)
                .HasForeignKey<ModelDbMuteP1Operation>(d => d.DbMuteP1CustomerId)
                .HasConstraintName("fk_model_db_mute_p1_operations_2");

            entity.HasOne(d => d.EmployeeOperations).WithMany(p => p.ModelDbMuteP1Operations)
                .HasPrincipalKey(p => p.EmployeeOperationsId)
                .HasForeignKey(d => d.EmployeeOperationsId)
                .HasConstraintName("fk_model_db_mute_p1_operations_AI_Model");

            entity.HasOne(d => d.ModelDbInit).WithMany(p => p.ModelDbMuteP1Operations).HasConstraintName("fk_model_db_mute_p1_operations");
        });

        modelBuilder.Entity<ModelDbMuteP1OperationsStage2A>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_Operations_Stage2_A");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.DbMuteP1Customer).WithMany(p => p.ModelDbMuteP1OperationsStage2As)
                .HasPrincipalKey(p => p.DbMuteP1CustomerId)
                .HasForeignKey(d => d.DbMuteP1CustomerId)
                .HasConstraintName("fk_model_db_mute_p1_operations_stage2_a");

            entity.HasOne(d => d.EmployeeOperations).WithMany(p => p.ModelDbMuteP1OperationsStage2As)
                .HasPrincipalKey(p => p.EmployeeOperationsId)
                .HasForeignKey(d => d.EmployeeOperationsId)
                .HasConstraintName("fk_model_db_mute_p1_operations_stage2_a_AI_Model");
        });

        modelBuilder.Entity<ModelDbMuteP1OperationsStage2B>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_Operations_Stage2_B");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.DbMuteP1Customer).WithMany(p => p.ModelDbMuteP1OperationsStage2Bs)
                .HasPrincipalKey(p => p.DbMuteP1CustomerId)
                .HasForeignKey(d => d.DbMuteP1CustomerId)
                .HasConstraintName("fk_model_db_mute_p1_operations_stage2_b");

            entity.HasOne(d => d.EmployeeOperations).WithMany(p => p.ModelDbMuteP1OperationsStage2Bs)
                .HasPrincipalKey(p => p.EmployeeOperationsId)
                .HasForeignKey(d => d.EmployeeOperationsId)
                .HasConstraintName("fk_model_db_mute_p1_operations_stage2_b_AI_Model");
        });

        modelBuilder.Entity<ModelDbMuteP1Qa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_QA");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.DbMuteP1Customer).WithOne(p => p.ModelDbMuteP1Qa)
                .HasPrincipalKey<ModelDbMuteP1>(p => p.ModelDbMuteP1CustomerId)
                .HasForeignKey<ModelDbMuteP1Qa>(d => d.DbMuteP1CustomerId)
                .HasConstraintName("fk_model_db_mute_p1_qa_2");

            entity.HasOne(d => d.ModelDbInit).WithMany(p => p.ModelDbMuteP1Qas).HasConstraintName("fk_model_db_mute_p1_qa");
        });

        modelBuilder.Entity<ModelDbMuteP1QaStage2A>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_QA_Stage2_A");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.DbMuteP1Customer).WithMany(p => p.ModelDbMuteP1QaStage2As)
                .HasPrincipalKey(p => p.DbMuteP1CustomerId)
                .HasForeignKey(d => d.DbMuteP1CustomerId)
                .HasConstraintName("fk_model_db_mute_p1_qa_stage2_a");
        });

        modelBuilder.Entity<ModelDbMuteP1QaStage2B>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_QA_Stage2_B");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.DbMuteP1Customer).WithMany(p => p.ModelDbMuteP1QaStage2Bs)
                .HasPrincipalKey(p => p.DbMuteP1CustomerId)
                .HasForeignKey(d => d.DbMuteP1CustomerId)
                .HasConstraintName("fk_model_db_mute_p1_qa_stage2_b");
        });

        modelBuilder.Entity<ModelDbMuteP1Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Tbl");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.DbMuteP1Customer).WithOne(p => p.ModelDbMuteP1Sale)
                .HasPrincipalKey<ModelDbMuteP1>(p => p.ModelDbMuteP1CustomerId)
                .HasForeignKey<ModelDbMuteP1Sale>(d => d.DbMuteP1CustomerId)
                .HasConstraintName("fk_model_db_mute_p1_sales_2");

            entity.HasOne(d => d.ModelDbInit).WithMany(p => p.ModelDbMuteP1Sales).HasConstraintName("fk_model_db_mute_p1_sales");
        });

        modelBuilder.Entity<ModelDbMuteP1SalesSage2A>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_Sales_Sage2_A");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.DbMuteP1Customer).WithMany(p => p.ModelDbMuteP1SalesSage2As)
                .HasPrincipalKey(p => p.DbMuteP1CustomerId)
                .HasForeignKey(d => d.DbMuteP1CustomerId)
                .HasConstraintName("fk_model_db_mute_p1_sales_sage2_a");
        });

        modelBuilder.Entity<ModelDbMuteP1SalesSage2B>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_Sales_Sage2_B");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.DbMuteP1Customer).WithMany(p => p.ModelDbMuteP1SalesSage2Bs)
                .HasPrincipalKey(p => p.DbMuteP1CustomerId)
                .HasForeignKey(d => d.DbMuteP1CustomerId)
                .HasConstraintName("fk_model_db_mute_p1_sales_sage2_b");
        });

        modelBuilder.Entity<ModelDbMuteP2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mid");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ModelDbMuteP1).WithMany(p => p.ModelDbMuteP2s).HasConstraintName("fk_model_db_mid");
        });

        modelBuilder.Entity<Operation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Ops");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Operational).WithOne(p => p.Operation)
                .HasPrincipalKey<Operations1>(p => p.OperationalId)
                .HasForeignKey<Operation>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_operations_1");

            entity.HasOne(d => d.OperationalNavigation).WithOne(p => p.Operation)
                .HasPrincipalKey<Operations2>(p => p.OperationalId)
                .HasForeignKey<Operation>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_operations_2");
        });

        modelBuilder.Entity<Operations1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Ops_1");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Operational).WithOne(p => p.Operations1)
                .HasPrincipalKey<IterationCycle1>(p => p.OperationalId)
                .HasForeignKey<Operations1>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_1_3");

            entity.HasOne(d => d.OperationalNavigation).WithOne(p => p.Operations1)
                .HasPrincipalKey<IterationCycle2>(p => p.OperationalId)
                .HasForeignKey<Operations1>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_1_4");

            entity.HasOne(d => d.Operational1).WithOne(p => p.Operations1)
                .HasPrincipalKey<IterationCycle3>(p => p.OperationalId)
                .HasForeignKey<Operations1>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_1_5");

            entity.HasOne(d => d.Operational2).WithOne(p => p.Operations1)
                .HasPrincipalKey<IterationCycle4>(p => p.OperationalId)
                .HasForeignKey<Operations1>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_1_6");

            entity.HasOne(d => d.Operational3).WithOne(p => p.Operations1)
                .HasPrincipalKey<OperationsStage1>(p => p.OperationalId)
                .HasForeignKey<Operations1>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_1");

            entity.HasOne(d => d.Operational4).WithOne(p => p.Operations1)
                .HasPrincipalKey<OperationsStage2>(p => p.OperationalId)
                .HasForeignKey<Operations1>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_1_2");

            entity.HasOne(d => d.Operational5).WithOne(p => p.Operations1)
                .HasPrincipalKey<OperationsStage3>(p => p.OperationalId)
                .HasForeignKey<Operations1>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_1_7");

            entity.HasOne(d => d.Operational6).WithOne(p => p.Operations1)
                .HasPrincipalKey<OperationsStage4>(p => p.OperationalId)
                .HasForeignKey<Operations1>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_1_8");
        });

        modelBuilder.Entity<Operations2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Ops_2");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Operational).WithOne(p => p.Operations2)
                .HasPrincipalKey<IterationCycle1>(p => p.OperationalId)
                .HasForeignKey<Operations2>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_2_9");

            entity.HasOne(d => d.OperationalNavigation).WithOne(p => p.Operations2)
                .HasPrincipalKey<IterationCycle2>(p => p.OperationalId)
                .HasForeignKey<Operations2>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_2_10");

            entity.HasOne(d => d.Operational1).WithOne(p => p.Operations2)
                .HasPrincipalKey<IterationCycle3>(p => p.OperationalId)
                .HasForeignKey<Operations2>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_2_11");

            entity.HasOne(d => d.Operational2).WithOne(p => p.Operations2)
                .HasPrincipalKey<IterationCycle4>(p => p.OperationalId)
                .HasForeignKey<Operations2>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_2_12");

            entity.HasOne(d => d.Operational3).WithOne(p => p.Operations2)
                .HasPrincipalKey<OperationsStage1>(p => p.OperationalId)
                .HasForeignKey<Operations2>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_2");

            entity.HasOne(d => d.Operational4).WithOne(p => p.Operations2)
                .HasPrincipalKey<OperationsStage2>(p => p.OperationalId)
                .HasForeignKey<Operations2>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_2_2");

            entity.HasOne(d => d.Operational5).WithOne(p => p.Operations2)
                .HasPrincipalKey<OperationsStage3>(p => p.OperationalId)
                .HasForeignKey<Operations2>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_2_13");

            entity.HasOne(d => d.Operational6).WithOne(p => p.Operations2)
                .HasPrincipalKey<OperationsStage4>(p => p.OperationalId)
                .HasForeignKey<Operations2>(d => d.OperationalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_14");
        });

        modelBuilder.Entity<OperationsStage1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Operations_Stage_1");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithOne(p => p.OperationsStage1)
                .HasPrincipalKey<Client>(p => p.CustomerId)
                .HasForeignKey<OperationsStage1>(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_stage_1_client");

            entity.HasOne(d => d.Order).WithMany(p => p.OperationsStage1s)
                .HasPrincipalKey(p => p.OrderId)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_stage_1");

            entity.HasOne(d => d.Product).WithMany(p => p.OperationsStage1s)
                .HasPrincipalKey(p => new { p.SubProductA, p.SubProductB, p.SubProductC })
                .HasForeignKey(d => new { d.SubProductA, d.SubProductB, d.SubProductC })
                .HasConstraintName("fk_operations_stage_1_product");

            entity.HasOne(d => d.Service).WithMany(p => p.OperationsStage1s)
                .HasPrincipalKey(p => new { p.SubServiceA, p.SubServiceB, p.SubServiceC })
                .HasForeignKey(d => new { d.SubServiceA, d.SubServiceB, d.SubServiceC })
                .HasConstraintName("fk_operations_stage_1_service");
        });

        modelBuilder.Entity<OperationsStage2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Operations_Stage_2");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithMany(p => p.OperationsStage2s)
                .HasPrincipalKey(p => p.CustomerId)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("fk_operations_stage_2_client");

            entity.HasOne(d => d.Order).WithOne(p => p.OperationsStage2)
                .HasPrincipalKey<IterationCycle1>(p => p.OrderId)
                .HasForeignKey<OperationsStage2>(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_stage_2");

            entity.HasOne(d => d.Product).WithMany(p => p.OperationsStage2s)
                .HasPrincipalKey(p => new { p.SubProductA, p.SubProductB, p.SubProductC })
                .HasForeignKey(d => new { d.SubProductA, d.SubProductB, d.SubProductC })
                .HasConstraintName("fk_operations_stage_2_product");

            entity.HasOne(d => d.Service).WithMany(p => p.OperationsStage2s)
                .HasPrincipalKey(p => new { p.SubServiceA, p.SubServiceB, p.SubServiceC })
                .HasForeignKey(d => new { d.SubServiceA, d.SubServiceB, d.SubServiceC })
                .HasConstraintName("fk_operations_stage_2_service");
        });

        modelBuilder.Entity<OperationsStage3>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Operations_Stage_3");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithMany(p => p.OperationsStage3s)
                .HasPrincipalKey(p => p.CustomerId)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("fk_operations_stage_3_client");

            entity.HasOne(d => d.Order).WithOne(p => p.OperationsStage3)
                .HasPrincipalKey<OperationsStage4>(p => p.OrderId)
                .HasForeignKey<OperationsStage3>(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_operations_stage_3");

            entity.HasOne(d => d.Product).WithMany(p => p.OperationsStage3s)
                .HasPrincipalKey(p => new { p.SubProductA, p.SubProductB, p.SubProductC })
                .HasForeignKey(d => new { d.SubProductA, d.SubProductB, d.SubProductC })
                .HasConstraintName("fk_operations_stage_3_product");

            entity.HasOne(d => d.Service).WithMany(p => p.OperationsStage3s)
                .HasPrincipalKey(p => new { p.SubServiceA, p.SubServiceB, p.SubServiceC })
                .HasForeignKey(d => new { d.SubServiceA, d.SubServiceB, d.SubServiceC })
                .HasConstraintName("fk_Operations_Stage_4_4");
        });

        modelBuilder.Entity<OperationsStage4>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Operations_Stage_4");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithMany(p => p.OperationsStage4s)
                .HasPrincipalKey(p => p.CustomerId)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("fk_operations_stage_4_client");

            entity.HasOne(d => d.Product).WithMany(p => p.OperationsStage4s)
                .HasPrincipalKey(p => new { p.SubProductA, p.SubProductB, p.SubProductC })
                .HasForeignKey(d => new { d.SubProductA, d.SubProductB, d.SubServiceC })
                .HasConstraintName("fk_operations_stage_4_product");

            entity.HasOne(d => d.Service).WithMany(p => p.OperationsStage4s)
                .HasPrincipalKey(p => new { p.SubServiceA, p.SubServiceB, p.SubServiceC })
                .HasForeignKey(d => new { d.SubServiceA, d.SubServiceB, d.SubServiceC })
                .HasConstraintName("fk_operations_stage_4_service_5");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Product");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.SubProductANavigation).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_subproduct_a");

            entity.HasOne(d => d.SubProductBNavigation).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_subproduct_b");

            entity.HasOne(d => d.SubProductCNavigation).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_subproduct_c");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Service");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.SubServiceANavigation).WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_service_subservice_a");

            entity.HasOne(d => d.SubServiceBNavigation).WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_service_subservice_b");

            entity.HasOne(d => d.SubServiceCNavigation).WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_service_subservice_c");
        });

        modelBuilder.Entity<Start>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Start_Process");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.GenerateClient).WithMany(p => p.Starts)
                .HasPrincipalKey(p => p.ClientId)
                .HasForeignKey(d => d.GenerateClientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_start_client_information");
        });

        modelBuilder.Entity<SubProductB>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_SubProduct_A_0");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<SubProductC>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_SubProduct_A_1");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<SubProductum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_SubProduct_A");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<SubServiceA>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_SubService_A");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<SubServiceB>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_SubService_A_0");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<SubServiceC>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_SubService_A_1");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
