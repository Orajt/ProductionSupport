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
                .ForMember(d => d.EditDate, s => s.MapFrom(s => s.EditDate.ToString("dd-MM-YYYY")))
                .ForMember(d => d.CreateDate, s => s.MapFrom(s => s.CreateDate.ToString("dd-MM-YYYY")));
             CreateMap<Domain.OrderPosition, Orders.PositionDto>()
                .ForMember(d => d.ArticleName, s => s.MapFrom(s => s.Article.FullName));
             CreateMap<Domain.Order, Orders.DetailsDto>()
                .ForMember(d => d.DeliveryPlaceName, s => s.MapFrom(s => s.DeliveryPlace.DepotName))
                .ForMember(d=>d.OrderPostions, s=> s.MapFrom(s=>s.OrderPositions));
            CreateMap<Domain.Order, Orders.ListDto>()
                .ForMember(d => d.DeliveryPlaceName, s => s.MapFrom(s => s.DeliveryPlace.DepotName));
        }
    }
}