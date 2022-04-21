using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ArticleFabricRealization
{
    public class ListFabricRealizationById
    {
        public class Query : IRequest<Result<ArticleFRDetails>>
        {
            public int ArticleId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ArticleFRDetails>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<ArticleFRDetails>> Handle(Query request, CancellationToken cancellationToken)
            {
                var article = await _context.Articles
                    .Include(p=>p.Realizations)
                        .ThenInclude(p=>p.Stuff)
                    .Include(p=>p.FabricVariant)
                    .FirstOrDefaultAsync(p=>p.Id==request.ArticleId);
                
                var gowno = article.ArticleTypeId;

                var fabricRealizationsGrouped = article.Realizations
                    .OrderBy(p=>p.StuffId)
                    .GroupBy(p=>p.StuffId)
                    .ToList();


                var result = new ArticleFRDetails();
                result.ArticleName=article.FullName;
                result.VariantGroup=article.FabricVariant.Name;

                foreach(var stuffGroup in fabricRealizationsGrouped)
                {
                    var newGroup = new ArticleFRByStuff()
                    {
                        StuffName=stuffGroup.First().Stuff.Name,
                        StuffId=stuffGroup.Key
                    };

                    foreach(var group in stuffGroup)
                    {
                        newGroup.GroupsQuanities.Add(new QuanityPerGroup
                        {
                            GroupId=group.Id,
                            CalculatedCode=group.CalculatedCode,
                            StuffId=stuffGroup.Key,
                            QuanityChaanged=false,
                            Quanity=group.FabricLength
                        });
                    }
                    newGroup.GroupsQuanities.OrderBy(p=>p.CalculatedCode).ToList();
                    result.GroupByStuffList.Add(newGroup);
                }
                return Result<ArticleFRDetails>.Success(result);
            }
        }
    }
}