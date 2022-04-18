using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Article
{
    public class EditFabric
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public int StuffId { get; set; }

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.FullName.Length).GreaterThan(0);
                RuleFor(x => x.StuffId).NotNull().GreaterThan(0);
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
                var fabric = await _context.Articles.FirstOrDefaultAsync(p => p.Id == request.Id);
                if (fabric.FullName.ToUpper() != request.FullName.ToUpper())
                {
                    if (await _context.Articles.AnyAsync(p => p.FullName.ToUpper() == request.FullName.ToUpper()
                             && p.ArticleTypeId == 6))
                    {
                        return Result<Unit>.Failure("Fabric with that name exists in database");
                    }
                }
                var stuff = await _context.Stuffs.FirstOrDefaultAsync(p => p.Id == request.StuffId);
                if (stuff == null) return null;

                fabric.FullName=request.FullName;
                fabric.NameWithoutFamilly=request.FullName;
                fabric.StuffId=request.StuffId;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to edit fabric");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}