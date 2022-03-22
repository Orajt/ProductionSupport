namespace Application.Core
{
    public class MappingProfiles : AutoMapper.Profile
    {
        public MappingProfiles()
        {
            CreateMap<Domain.Article, Article.ListDto>()
                .ForMember(d => d.ArticleTypeName, s => s.MapFrom(s => s.ArticleType.Name))
                .ForMember(d => d.FamillyName, s => s.MapFrom(s => s.Familly == null ? "None" : s.Familly.Name))
                .ForMember(d => d.StuffName, s => s.MapFrom(s => s.Stuff == null ? "None" : s.Stuff.Name))
                .ForMember(d => d.EditDate, s => s.MapFrom(s => s.EditDate))
                .ForMember(d => d.CreateDate, s => s.MapFrom(s => s.CreateDate));
             CreateMap<Domain.OrderPosition, OrderPosition.PositionDto>()
                .ForMember(d => d.ArticleName, s => s.MapFrom(s => s.Article.FullName))
                .ForMember(d => d.ArticleId, s => s.MapFrom(s => s.Article.Id))
                .ForMember(d => d.ArticleTypeId, s => s.MapFrom(s => s.Article.ArticleTypeId));
                
             CreateMap<Domain.Order, Orders.DetailsDto>()
                .ForMember(d => d.DeliveryPlaceName, s => s.MapFrom(s => s.DeliveryPlace.DepotName))
                .ForMember(d=>d.OrderPositions, s=> s.MapFrom(s=>s.OrderPositions));
            CreateMap<Domain.Order, Orders.ListDto>()
                .ForMember(d => d.DeliveryPlaceName, s => s.MapFrom(s => s.DeliveryPlace.DepotName));
            
            CreateMap<Orders.DetailsDto, Orders.OrderSummaryDto>();
            CreateMap<Domain.Company, Companies.ListDto>();

            CreateMap<Domain.Article, Article.DetailsDto>()
                .ForMember(d => d.ArticleTypeName, s => s.MapFrom(s => s.ArticleType.Name))
                .ForMember(d => d.FamillyName, s => s.MapFrom(s => s.Familly==null ? "None": s.Familly.Name))       
                .ForMember(d => d.StuffName, s => s.MapFrom(s => s.Stuff==null ? "None": s.Stuff.Name))       
                .ForMember(d => d.FabricVariantGroupName, s => s.MapFrom(s => s.FabricVariant==null ? "None": s.FabricVariant.Name))
                .ForMember(d => d.ChildArticles, s => s.MapFrom(s => s.ChildRelations));
            
            CreateMap<Domain.ArticleArticle, Article.DetailsDtoChildArticles>()
                .ForMember(d => d.ChildArticleName, s => s.MapFrom(s => s.ChildArticle.FullName))       
                .ForMember(d => d.ChildArticleType, s => s.MapFrom(s => s.ChildArticle.ArticleType.Name))       
                .ForMember(d => d.ChildArticleHasChild, s => s.MapFrom(s => s.ChildArticle.HasChild))
                .ForMember(d => d.ChildId, s => s.MapFrom(s => s.ChildId));
             CreateMap<Domain.Stuff, Stuff.ListDto>()
                .ForMember(d => d.ArticleTypeName, s => s.MapFrom(s => s.ArticleType.Name)); 
            
        }
    }
}