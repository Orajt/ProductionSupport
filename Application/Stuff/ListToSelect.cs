using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Stuff
{
    public class ListToSelect
    {
        public class Query : IRequest<Result<List<ListToSelectDto>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<ListToSelectDto>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<ListToSelectDto>>> Handle(Query request, CancellationToken cancellationToken)
            {

                var stuffs = await _context.Stuffs
                    .Include(p => p.ArticleTypes)
                    .OrderBy(p => p.Name)
                    .AsNoTracking()
                    .ToListAsync();

                var result = new List<ListToSelectDto>();
                foreach (var stuff in stuffs)
                {
                    var stuffToAdd = new ListToSelectDto();
                    stuffToAdd.Value = stuff.Id;
                    stuffToAdd.Label = stuff.Name;
                    if (stuff.ArticleTypes != null && stuff.ArticleTypes.Count > 0)
                    {
                        foreach (var articleType in stuff.ArticleTypes)
                        {
                            stuffToAdd.ArticleTypesIds.Add(articleType.ArticleTypeId);
                        }
                    }
                    result.Add(stuffToAdd);
                }


                return Result<List<ListToSelectDto>>.Success(result);
            }
        }
    }
}