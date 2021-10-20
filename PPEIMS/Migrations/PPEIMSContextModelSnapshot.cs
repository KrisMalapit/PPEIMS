﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PPEIMS.Models;

namespace PPEIMS.Migrations
{
    [DbContext(typeof(PPEIMSContext))]
    partial class PPEIMSContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PPEIMS.Models.Company", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Status");

                    b.HasKey("ID");

                    b.ToTable("Companies");

                    b.HasData(
                        new { ID = 1, Code = "SLPGC", Name = "Southwest Luzon Power Gen Corporation", Status = "Active" },
                        new { ID = 2, Code = "SCPC", Name = "Sem-Calaca Power Corporation", Status = "Active" }
                    );
                });

            modelBuilder.Entity("PPEIMS.Models.Department", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<int>("CompanyId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Status");

                    b.HasKey("ID");

                    b.HasIndex("CompanyId");

                    b.ToTable("Departments");

                    b.HasData(
                        new { ID = 1, Code = "NA", CompanyId = 1, Name = "NOTSET", Status = "Deleted" }
                    );
                });

            modelBuilder.Entity("PPEIMS.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Description2");

                    b.Property<string>("No")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)");

                    b.Property<int?>("PPEId");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.HasIndex("PPEId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("PPEIMS.Models.ItemDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("ItemId");

                    b.Property<string>("LineNo");

                    b.Property<int>("Quantity");

                    b.Property<string>("Remarks");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("ItemDetails");
                });

            modelBuilder.Entity("PPEIMS.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Descriptions");

                    b.Property<string>("Status");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("PPEIMS.Models.NoSeries", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("LastNoUsed");

                    b.HasKey("Id");

                    b.ToTable("NoSeries");
                });

            modelBuilder.Entity("PPEIMS.Models.PPE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<int>("Field");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Office");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.ToTable("PPEs");
                });

            modelBuilder.Entity("PPEIMS.Models.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("DocumentStatus");

                    b.Property<string>("ReferenceNo");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("PPEIMS.Models.RequestDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("ItemId");

                    b.Property<int>("Quantity");

                    b.Property<string>("Remarks");

                    b.Property<int>("RequestId");

                    b.Property<string>("Status");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("RequestId");

                    b.ToTable("RequestDetails");
                });

            modelBuilder.Entity("PPEIMS.Models.RequestDetailUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("RequestDetailId");

                    b.Property<string>("Status");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RequestDetailId");

                    b.HasIndex("UserId");

                    b.ToTable("RequestDetailUsers");
                });

            modelBuilder.Entity("PPEIMS.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new { Id = 1, Name = "Admin", Status = "Active" },
                        new { Id = 2, Name = "User", Status = "Active" }
                    );
                });

            modelBuilder.Entity("PPEIMS.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category");

                    b.Property<string>("CompanyAccess");

                    b.Property<int>("DepartmentId");

                    b.Property<string>("Domain");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<int>("RoleId");

                    b.Property<string>("Status");

                    b.Property<string>("UserType");

                    b.Property<string>("Username")
                        .HasColumnType("VARCHAR(50)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("RoleId");

                    b.HasIndex("Username", "Status")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL AND [Status] IS NOT NULL");

                    b.ToTable("Users");

                    b.HasData(
                        new { Id = 1, CompanyAccess = "1", DepartmentId = 1, Domain = "SMCDACON", Email = "kcmalapit@semirarampc.com", FirstName = "Kristoffer", LastName = "Malapit", Name = "Kristoffer Malapit", Password = "", RoleId = 1, Status = "1", Username = "kcmalapit" }
                    );
                });

            modelBuilder.Entity("PPEIMS.Models.Department", b =>
                {
                    b.HasOne("PPEIMS.Models.Company", "Companies")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PPEIMS.Models.Item", b =>
                {
                    b.HasOne("PPEIMS.Models.PPE", "PPEs")
                        .WithMany()
                        .HasForeignKey("PPEId");
                });

            modelBuilder.Entity("PPEIMS.Models.ItemDetail", b =>
                {
                    b.HasOne("PPEIMS.Models.Item", "Items")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PPEIMS.Models.RequestDetail", b =>
                {
                    b.HasOne("PPEIMS.Models.Item", "Items")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PPEIMS.Models.Request", "Requests")
                        .WithMany()
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PPEIMS.Models.RequestDetailUser", b =>
                {
                    b.HasOne("PPEIMS.Models.RequestDetail", "RequestDetails")
                        .WithMany()
                        .HasForeignKey("RequestDetailId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PPEIMS.Models.User", "Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PPEIMS.Models.User", b =>
                {
                    b.HasOne("PPEIMS.Models.Department", "Departments")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PPEIMS.Models.Role", "Roles")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
