using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Article
{
    public class Details
    {
        public class Query : IRequest<Result<DetailsDto>>
        {
            public string ArticleIdentifier {get;set;}
        }

        public class Handler : IRequestHandler<Query, Result<DetailsDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<DetailsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var article = new DetailsDto();
                int articleId=0;
                if(int.TryParse(request.ArticleIdentifier, out articleId))
                {
                    article = await _context.Articles.ProjectTo<DetailsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(p=>p.Id==articleId);
                }
                if(articleId==0)
                    article = await _context.Articles.ProjectTo<DetailsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(p=>p.FamillyName.ToUpper()
                    ==request.ArticleIdentifier.ToUpper());

                if(article==null) return null;
                article.EditDate=DateHelpers.SetDateTimeToCurrent(article.EditDate);
                article.CreateDate=DateHelpers.SetDateTimeToCurrent(article.CreateDate);

                article.AbleToEditPrimaries=!(await _context.OrderPositions.AnyAsync(p=>p.ArticleId==articleId));
                return Result<DetailsDto>.Success(article);
            }
        }
    }
}