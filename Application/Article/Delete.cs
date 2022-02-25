using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Article
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id {get;set;}
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
               
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
                var article= await _context.Articles
                .Include(p=>p.ChildRelations)
                .Include(p=>p.ParentRelations)
                .FirstOrDefaultAsync(p=>p.Id==request.Id);

                if(article==null) return null;
                
                _context.ArticleArticle.RemoveRange(article.ChildRelations.Concat(article.ParentRelations));
                _context.Articles.Remove(article);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete Article");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}