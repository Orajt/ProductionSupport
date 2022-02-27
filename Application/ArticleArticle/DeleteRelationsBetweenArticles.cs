using Application.Core;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.ArticleArticle
{
    public class DeleteRelationBetweenArticles
    {
          public class Command : IRequest<Result<Unit>>
        {
            public int ParentId {get;set;}
            public List<int> ChildList { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ChildList).NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    await _context.ArticleArticle.Where(p => p.ParentId==request.ParentId && request.ChildList.Contains(p.ChildId)).DeleteFromQueryAsync();
                }
                catch (Exception)
                {
                    return Result<Unit>.Failure("Failed to delete relations between articles");
                }
                return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}