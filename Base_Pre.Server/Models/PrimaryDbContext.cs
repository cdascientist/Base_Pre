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

        modelBuilder.Entity<ClientInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Client_Information");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithOne(p => p.ClientInformation)
                .HasPrincipalKey<ModelDbInit>(p => p.CustomerId)
                .HasForeignKey<ClientInformation>(d => d.CustomerId)
                .HasConstraintName("fk_client_information");
        });

        modelBuilder.Entity<ClientOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_CLient_Order");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Csr>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_CSR");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Csr1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_CSR_1");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Csr2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_CSR_2");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<EmployeeOperation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Employee_Operations");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<EmployeeQa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Employee_QA");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<EmployeeSale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Employee_Sales");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<IterationCycle1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Iteration_Cycle_1");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<IterationCycle2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Iteration_Cycle_2");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<IterationCycle3>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Iteration_Cycle_3");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<IterationCycle4>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Iteration_Cycle_4");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ModelDbInit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Init");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ModelDbMuteP1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ModelDbMuteP1Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_Customer");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ModelDbMuteP1CustomerSage2A>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_Customer_Sage2_A");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ModelDbMuteP1CustomerSage2B>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_Customer_Sage2_B");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ModelDbMuteP1Operation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_Operations");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ModelDbMuteP1OperationsStage2A>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_Operations_Stage2_A");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ModelDbMuteP1OperationsStage2B>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_Operations_Stage2_B");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ModelDbMuteP1Qa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_QA");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ModelDbMuteP1QaStage2A>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_QA_Stage2_A");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ModelDbMuteP1QaStage2B>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_QA_Stage2_B");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ModelDbMuteP1Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Tbl");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ModelDbMuteP1SalesSage2A>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_Sales_Sage2_A");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ModelDbMuteP1SalesSage2B>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mute_P1_Sales_Sage2_B");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ModelDbMuteP2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Model_DB_Mid");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Operation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Ops");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Operations1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Ops_1");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Operations2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Ops_2");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<OperationsStage1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Operations_Stage_1");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<OperationsStage2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Operations_Stage_2");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<OperationsStage3>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Operations_Stage_3");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<OperationsStage4>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Operations_Stage_4");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Product");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Service");

            entity.Property(e => e.Id).ValueGeneratedNever();
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
