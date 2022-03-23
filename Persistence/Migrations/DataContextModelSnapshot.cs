﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.2");

            modelBuilder.Entity("Domain.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductionDepartmentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("ProductionDepartmentId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Domain.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("Area")
                        .HasColumnType("REAL");

                    b.Property<int>("ArticleTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Capacity")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("CreatedInCompany")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EditDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("FabricVariantGroupId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("FamillyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FullName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasChild")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasChildSameArticleType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("High")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Length")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NameWithoutFamilly")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(6,2)");

                    b.Property<int?>("StuffId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Width")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ArticleTypeId");

                    b.HasIndex("FabricVariantGroupId");

                    b.HasIndex("FamillyId");

                    b.HasIndex("StuffId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Domain.ArticleArticle", b =>
                {
                    b.Property<int>("ChildId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ParentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("AddCol")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quanity")
                        .HasColumnType("INTEGER");

                    b.HasKey("ChildId", "ParentId");

                    b.HasIndex("ParentId");

                    b.ToTable("ArticleArticle");
                });

            modelBuilder.Entity("Domain.ArticleFabricRealization", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CalculatedCode")
                        .HasColumnType("INTEGER");

                    b.Property<float>("FabricLength")
                        .HasColumnType("REAL");

                    b.Property<int>("FabricVariantId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GroupId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StuffId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("ArticleFabricRealizations");
                });

            modelBuilder.Entity("Domain.ArticleFilePath", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("ArticlesFilesPaths");
                });

            modelBuilder.Entity("Domain.ArticleProductionDepartment", b =>
                {
                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductionDepartmentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TimeToProduce")
                        .HasColumnType("INTEGER");

                    b.HasKey("ArticleId", "ProductionDepartmentId");

                    b.HasIndex("ProductionDepartmentId");

                    b.ToTable("ArticlesDepartments");
                });

            modelBuilder.Entity("Domain.ArticleType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ArticleTypes");
                });

            modelBuilder.Entity("Domain.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompanyIdentifier")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Merchant")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Supplier")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Domain.CompanyArticle", b =>
                {
                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClientName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("ForSale")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(6,2)");

                    b.HasKey("ArticleId", "CompanyId");

                    b.HasIndex("CompanyId");

                    b.ToTable("CompanyArticles");
                });

            modelBuilder.Entity("Domain.DeliveryPlace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Apartment")
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<int>("CompanyID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Country")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberOfBuilding")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PostalCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("Street")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CompanyID");

                    b.ToTable("DeliveryPlaces");
                });

            modelBuilder.Entity("Domain.FabricVariant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FullName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ShortName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FabricVariant");
                });

            modelBuilder.Entity("Domain.FabricVariantFabricGroupVariant", b =>
                {
                    b.Property<int>("FabricVariantGroupId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FabricVariantId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlaceInGroup")
                        .HasColumnType("INTEGER");

                    b.HasKey("FabricVariantGroupId", "FabricVariantId");

                    b.HasIndex("FabricVariantId");

                    b.ToTable("FabricVariantsGroupVariants");
                });

            modelBuilder.Entity("Domain.FabricVariantGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FabricVariantGroups");
                });

            modelBuilder.Entity("Domain.Familly", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Famillies");
                });

            modelBuilder.Entity("Domain.FamillyFabricVariantGroup", b =>
                {
                    b.Property<int>("FabricVariantGroupId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FamillyId")
                        .HasColumnType("INTEGER");

                    b.HasKey("FabricVariantGroupId", "FamillyId");

                    b.HasIndex("FamillyId");

                    b.ToTable("FamilliesFabricVarianGroups");
                });

            modelBuilder.Entity("Domain.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DeliveryPlaceId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Done")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EditDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("FabricsCalculated")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ProductionDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ShipmentDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryPlaceId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Domain.OrderPosition", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CalculatedRealization")
                        .HasColumnType("TEXT");

                    b.Property<string>("Client")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("FabricPirce")
                        .HasColumnType("decimal(6,2)");

                    b.Property<int>("Lp")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quanity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Realization")
                        .HasColumnType("TEXT");

                    b.Property<int?>("SetId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("OrderId");

                    b.HasIndex("SetId");

                    b.ToTable("OrderPositions");
                });

            modelBuilder.Entity("Domain.OrderPositionRealization", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FabricId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("OrderPositionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlaceInGroup")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VarriantId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FabricId");

                    b.HasIndex("OrderPositionId");

                    b.HasIndex("VarriantId");

                    b.ToTable("OrderPositionRealizations");
                });

            modelBuilder.Entity("Domain.ProductionDepartment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ProductionDepartments");
                });

            modelBuilder.Entity("Domain.Set", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("Domain.Stuff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArticleTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ArticleTypeId");

                    b.ToTable("Stuffs");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Domain.AppUser", b =>
                {
                    b.HasOne("Domain.ProductionDepartment", "ProductionDepartment")
                        .WithMany("AppUsers")
                        .HasForeignKey("ProductionDepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductionDepartment");
                });

            modelBuilder.Entity("Domain.Article", b =>
                {
                    b.HasOne("Domain.ArticleType", "ArticleType")
                        .WithMany("Articles")
                        .HasForeignKey("ArticleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.FabricVariantGroup", "FabricVariant")
                        .WithMany("Articles")
                        .HasForeignKey("FabricVariantGroupId");

                    b.HasOne("Domain.Familly", "Familly")
                        .WithMany("Articles")
                        .HasForeignKey("FamillyId");

                    b.HasOne("Domain.Stuff", "Stuff")
                        .WithMany("Articles")
                        .HasForeignKey("StuffId");

                    b.Navigation("ArticleType");

                    b.Navigation("FabricVariant");

                    b.Navigation("Familly");

                    b.Navigation("Stuff");
                });

            modelBuilder.Entity("Domain.ArticleArticle", b =>
                {
                    b.HasOne("Domain.Article", "ChildArticle")
                        .WithMany("ParentRelations")
                        .HasForeignKey("ChildId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Article", "ParentArticle")
                        .WithMany("ChildRelations")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ChildArticle");

                    b.Navigation("ParentArticle");
                });

            modelBuilder.Entity("Domain.ArticleFabricRealization", b =>
                {
                    b.HasOne("Domain.Article", "Article")
                        .WithMany("Realizations")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");
                });

            modelBuilder.Entity("Domain.ArticleFilePath", b =>
                {
                    b.HasOne("Domain.Article", "Article")
                        .WithMany("FilePaths")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");
                });

            modelBuilder.Entity("Domain.ArticleProductionDepartment", b =>
                {
                    b.HasOne("Domain.Article", "Article")
                        .WithMany("ProductionDepartments")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.ProductionDepartment", "ProductionDepartment")
                        .WithMany("Articles")
                        .HasForeignKey("ProductionDepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("ProductionDepartment");
                });

            modelBuilder.Entity("Domain.CompanyArticle", b =>
                {
                    b.HasOne("Domain.Article", "Article")
                        .WithMany("Companies")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Company", "Company")
                        .WithMany("Articles")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Domain.DeliveryPlace", b =>
                {
                    b.HasOne("Domain.Company", "Company")
                        .WithMany("DeliveryPlaces")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Domain.FabricVariantFabricGroupVariant", b =>
                {
                    b.HasOne("Domain.FabricVariantGroup", "FabricVariantGroup")
                        .WithMany("FabricVariants")
                        .HasForeignKey("FabricVariantGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.FabricVariant", "FabricVariant")
                        .WithMany("FabricVariantGroups")
                        .HasForeignKey("FabricVariantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FabricVariant");

                    b.Navigation("FabricVariantGroup");
                });

            modelBuilder.Entity("Domain.FamillyFabricVariantGroup", b =>
                {
                    b.HasOne("Domain.FabricVariantGroup", "FabricVariantGroup")
                        .WithMany("Famillies")
                        .HasForeignKey("FabricVariantGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Familly", "Familly")
                        .WithMany("FabricVariantGroups")
                        .HasForeignKey("FamillyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FabricVariantGroup");

                    b.Navigation("Familly");
                });

            modelBuilder.Entity("Domain.Order", b =>
                {
                    b.HasOne("Domain.DeliveryPlace", "DeliveryPlace")
                        .WithMany("Orders")
                        .HasForeignKey("DeliveryPlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeliveryPlace");
                });

            modelBuilder.Entity("Domain.OrderPosition", b =>
                {
                    b.HasOne("Domain.Article", "Article")
                        .WithMany("OrderPosition")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Order", "Order")
                        .WithMany("OrderPositions")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Set", "Set")
                        .WithMany("OrderPositions")
                        .HasForeignKey("SetId");

                    b.Navigation("Article");

                    b.Navigation("Order");

                    b.Navigation("Set");
                });

            modelBuilder.Entity("Domain.OrderPositionRealization", b =>
                {
                    b.HasOne("Domain.Article", "Fabric")
                        .WithMany()
                        .HasForeignKey("FabricId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.OrderPosition", "OrderPosition")
                        .WithMany("Realizations")
                        .HasForeignKey("OrderPositionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.FabricVariant", "Variant")
                        .WithMany()
                        .HasForeignKey("VarriantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Fabric");

                    b.Navigation("OrderPosition");

                    b.Navigation("Variant");
                });

            modelBuilder.Entity("Domain.Stuff", b =>
                {
                    b.HasOne("Domain.ArticleType", "ArticleType")
                        .WithMany("Stuffs")
                        .HasForeignKey("ArticleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ArticleType");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Domain.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Domain.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Domain.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Article", b =>
                {
                    b.Navigation("ChildRelations");

                    b.Navigation("Companies");

                    b.Navigation("FilePaths");

                    b.Navigation("OrderPosition");

                    b.Navigation("ParentRelations");

                    b.Navigation("ProductionDepartments");

                    b.Navigation("Realizations");
                });

            modelBuilder.Entity("Domain.ArticleType", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("Stuffs");
                });

            modelBuilder.Entity("Domain.Company", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("DeliveryPlaces");
                });

            modelBuilder.Entity("Domain.DeliveryPlace", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Domain.FabricVariant", b =>
                {
                    b.Navigation("FabricVariantGroups");
                });

            modelBuilder.Entity("Domain.FabricVariantGroup", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("FabricVariants");

                    b.Navigation("Famillies");
                });

            modelBuilder.Entity("Domain.Familly", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("FabricVariantGroups");
                });

            modelBuilder.Entity("Domain.Order", b =>
                {
                    b.Navigation("OrderPositions");
                });

            modelBuilder.Entity("Domain.OrderPosition", b =>
                {
                    b.Navigation("Realizations");
                });

            modelBuilder.Entity("Domain.ProductionDepartment", b =>
                {
                    b.Navigation("AppUsers");

                    b.Navigation("Articles");
                });

            modelBuilder.Entity("Domain.Set", b =>
                {
                    b.Navigation("OrderPositions");
                });

            modelBuilder.Entity("Domain.Stuff", b =>
                {
                    b.Navigation("Articles");
                });
#pragma warning restore 612, 618
        }
    }
}
