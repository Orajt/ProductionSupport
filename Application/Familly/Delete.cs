using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Familly
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Id).NotNull();
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
                var familly = await _context.Famillies
                    .Include(p => p.FabricVariantGroups)
                    .Include(p=>p.Articles)
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                if (familly.Articles.Any())
                    return Result<Unit>.Failure("Familly is used to one or more articles");

                _context.Remove(familly);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete familly");

                return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}