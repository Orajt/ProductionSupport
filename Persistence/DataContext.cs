using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;
public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext() : base()
    {
    }
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {

    }
    public DbSet<Article> Articles { get; set; }
    public DbSet<ArticleArticle> ArticleArticle { get; set; }
    public DbSet<ArticleFilePath> ArticlesFilesPaths { get; set; }
    public DbSet<ArticleFabricRealization> ArticleFabricRealizations { get; set; }
    public DbSet<ArticleProductionDepartment> ArticlesDepartments { get; set; }
    public DbSet<ArticleType> ArticleTypes { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyArticle> CompanyArticles { get; set; }
    public DbSet<DeliveryPlace> DeliveryPlaces { get; set; }
    public DbSet<FabricVariant> FabricVariant { get; set; }
    public DbSet<FabricVariantGroup> FabricVariantGroups { get; set; }
    public DbSet<FabricVariantFabricGroupVariant> FabricVariantsGroupVariants { get; set; }
    public DbSet<Familly> Famillies { get; set; }
    public DbSet<FamillyFabricVariantGroup> FamilliesFabricVarianGroups { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderPosition> OrderPositions { get; set; }
    public DbSet<OrderPositionRealization> OrderPositionRealizations { get; set; }
    public DbSet<ProductionDepartment> ProductionDepartments { get; set; }
    public DbSet<Stuff> Stuffs { get; set; }
    public DbSet<Set> Sets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ArticleArticle>(x => x.HasKey(mb => new { mb.ChildId, mb.ParentId }));
        modelBuilder.Entity<ArticleArticle>()
            .HasOne(pt => pt.ChildArticle)
            .WithMany(p => p.ParentRelations)
            .HasForeignKey(pt => pt.ChildId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ArticleArticle>()
            .HasOne(pt => pt.ParentArticle)
            .WithMany(p => p.ChildRelations)
            .HasForeignKey(pt => pt.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Article>()
            .Property(p => p.Price)
            .HasColumnType("decimal(6,2)");

        modelBuilder.Entity<ArticleFilePath>()
            .HasOne(p => p.Article)
            .WithMany(p => p.FilePaths);

        modelBuilder.Entity<ArticleProductionDepartment>(x => x.HasKey(op => new { op.ArticleId, op.ProductionDepartmentId }));
        modelBuilder.Entity<CompanyArticle>(x => x.HasKey(op => new { op.ArticleId, op.CompanyId }));
        modelBuilder.Entity<CompanyArticle>()
           .Property(p => p.Price)
           .HasColumnType("decimal(6,2)");
        modelBuilder.Entity<FabricVariantFabricGroupVariant>(x => x.HasKey(fb => new { fb.FabricVariantGroupId, fb.FabricVariantId }));
        modelBuilder.Entity<FamillyFabricVariantGroup>(x => x.HasKey(fb => new { fb.FabricVariantGroupId, fb.FamillyId }));
        modelBuilder.Entity<OrderPosition>()
            .Property(p => p.FabricPirce)
            .HasColumnType("decimal(6,2)");

        modelBuilder.Entity<OrderPosition>()
            .HasMany(pt => pt.Realizations)
            .WithOne(pt => pt.OrderPosition)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
