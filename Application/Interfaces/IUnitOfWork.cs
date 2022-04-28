namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IArticleRepository Articles { get; }
        IArticleArticleRepository ArticlesArticles { get; }
        IArticleFabricRealizationRepository ArticlesFabricRealizations { get; }
        IArticleFilePathRepository ArticlesFilePaths { get; }
        IArticleProductionDepartmentRepository ArticlesProductionDepartments { get; }
        IArticleTypeRepository ArticleTypes { get; }
        IArticleTypeStuffRepository ArticleTypesStuffs { get; }
        ICompanyRepository Companies { get; }
        ICompanyArticleRepository CompaniesArticles { get; }
        IDeliveryPlaceRepository DeliveryPlaces { get; }
        IFabricVariantRepository FabricVariants { get; }
        IFabricVariantGroupVariantRepository FabricVariantsGroupVariants { get; }
        IFabricVariantGroupRepository FabricVariantsGroups { get; }
        IFamillyRepository Famillies { get; }
        IOrderRepository Orders { get; }
        IOrderPositionRepository OrderPositions { get; }
        IOrderPositionRealizationRepozitory OrderPositionsRealizations { get; }
        IProductionDepartmentRepository ProductionDepartments { get; }
        ISetRepository Sets { get; }
        IStuffRepository Stuffs { get; }
        Task<bool> SaveChangesAsync();
    }
}