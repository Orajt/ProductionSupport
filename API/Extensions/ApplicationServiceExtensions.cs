using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;
using Infrastructure.Security;
using MediatR;
using Application.ProductionDepartment;
using Application.Core;
using Application.Repositories;
using Application.UnitOfWork;
using Application.Calculation;
using Application.Article;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                c.CustomSchemaIds(type=>type.ToString());
            });
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin();
                });
            });
            services.AddMediatR(typeof(ListReacSelect.Handler).Assembly);
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserAccessor, UserAccessor>();

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IArticleRepository, ArticleRepository>();
            services.AddTransient<IArticleArticleRepository, ArticleArticleRepository>();
            services.AddTransient<IArticleFabricRealizationRepository, ArticleFabicRealizationRepository>();
            services.AddTransient<IArticleFilePathRepository, ArticleFilePathRepository>();
            services.AddTransient<IArticleProductionDepartmentRepository, ArticleProductionDepartmentRepository>();
            services.AddTransient<IArticleTypeRepository, ArticleTypeRepository>();
            services.AddTransient<IArticleTypeStuffRepository, ArticleTypeStuffRepository>();
            services.AddTransient<ICompanyArticleRepository, CompanyArticleRepository>();
            services.AddTransient<IDeliveryPlaceRepository, DeliveryPlaceRepository>();
            services.AddTransient<IFabricVariantRepository, FabricVariantRepository>();
            services.AddTransient<IFabricVariantGroupVariantRepository, FabricVariantGroupVariantRepository>();
            services.AddTransient<IFabricVariantGroupRepository, FabricVariantGroupRepository>();  
            services.AddTransient<IFamillyRepository, FamillyRepository>();  
            services.AddTransient<IOrderRepository, OrderRepository>();  
            services.AddTransient<IOrderPositionRepository, OrderPositionRepository>();  
            services.AddTransient<IProductionDepartmentRepository, ProductionDepartmentRepository>();  
            services.AddTransient<ISetRepository, SetRepository>();  
            services.AddTransient<IStuffRepository, StuffRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IRelations, Relations>();
            services.AddTransient<IListHelpers, ListHelpers>();
            services.AddSingleton<ICalculateFabricsCreatePdf, CalculateFabricsCreatePdf>();
            services.AddTransient<IArticleHelpers, ArticleHelpers>();
            services.AddSignalR();

            return services;
        }
    }
}