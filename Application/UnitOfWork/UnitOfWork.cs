using Application.Core;
using Application.Interfaces;
using Application.Repositories;
using Persistence;

namespace Application.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IRelations _relations;
        public UnitOfWork(DataContext context, IRelations relations)
        {
            _relations = relations;
            _context = context;
            Articles = new ArticleRepository(_context);
            ArticlesArticles = new ArticleArticleRepository(_context, _relations);
            ArticlesFabricRealizations = new ArticleFabicRealizationRepository(_context);
            Stuffs = new StuffRepository(_context);
            ArticlesFilePaths = new ArticleFilePathRepository(_context);
            ArticlesProductionDepartments = new ArticleProductionDepartmentRepository(_context);
            ArticleTypes = new ArticleTypeRepository(_context);
            ArticleTypesStuffs = new ArticleTypeStuffRepository(_context);
            Companies = new CompanyRepository(_context);
            CompaniesArticles = new CompanyArticleRepository(_context);
            DeliveryPlaces = new DeliveryPlaceRepository(_context);
            FabricVariants = new FabricVariantRepository(_context);
            FabricVariantsGroupVariants = new FabricVariantGroupVariantRepository(_context);
            FabricVariantsGroups = new FabricVariantGroupRepository(_context);
            Famillies = new FamillyRepository(_context);
            Orders = new OrderRepository(_context);
            OrderPositions = new OrderPositionRepository(_context);
            OrderPositionsRealizations = new OrderPositionRealizationRepozitory(_context);
            ProductionDepartments = new ProductionDepartmentRepository(_context);
            Sets = new SetRepository(_context);

        }
        public IArticleRepository Articles { get; private set; }
        public IStuffRepository Stuffs { get; private set; }
        public IArticleArticleRepository ArticlesArticles { get; private set; }
        public IArticleFabricRealizationRepository ArticlesFabricRealizations { get; private set; }
        public IArticleFilePathRepository ArticlesFilePaths { get; private set; }
        public IArticleProductionDepartmentRepository ArticlesProductionDepartments { get; private set; }
        public IArticleTypeRepository ArticleTypes { get; private set; }
        public IArticleTypeStuffRepository ArticleTypesStuffs { get; private set; }
        public ICompanyRepository Companies { get; private set; }
        public ICompanyArticleRepository CompaniesArticles { get; private set; }
        public IDeliveryPlaceRepository DeliveryPlaces { get; private set; }
        public IFabricVariantRepository FabricVariants { get; private set; }
        public IFabricVariantGroupVariantRepository FabricVariantsGroupVariants { get; private set; }
        public IFabricVariantGroupRepository FabricVariantsGroups { get; private set; }
        public IFamillyRepository Famillies { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IOrderPositionRepository OrderPositions { get; private set; }
        public IOrderPositionRealizationRepozitory OrderPositionsRealizations { get; private set; }
        public IProductionDepartmentRepository ProductionDepartments { get; private set; }
        public ISetRepository Sets { get; private set; }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}