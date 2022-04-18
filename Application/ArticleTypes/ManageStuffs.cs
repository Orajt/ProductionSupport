using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ArticleTypes
{
    public class ManageStuffs
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
            public List<int> Stuffs { get; set; }
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
                var articleType=await _context.ArticleTypes.Include(p=>p.Stuffs).FirstOrDefaultAsync(p=>p.Id==request.Id);

                var stuffListToRemove = articleType.Stuffs.Where(p=>!request.Stuffs.Contains(p.StuffId)).ToList();

                if(stuffListToRemove.Count>0)
                    _context.ArticleTypesStuffs.RemoveRange(stuffListToRemove);
                

                var stuffsToAssign = await _context.Stuffs.ToListAsync();
                var stuffListToAdd = new List<Domain.ArticleTypeStuff>();

                foreach(var stuffId in request.Stuffs)
                {
                    var stuff=stuffsToAssign.FirstOrDefault(p=>p.Id==stuffId);
                    if(stuff==null) return null;
                    if(articleType.Stuffs.Any(p=>p.StuffId==stuffId)) continue;
                    stuffListToAdd.Add(new Domain.ArticleTypeStuff{
                        ArticleType=articleType,
                        ArticleTypeId=articleType.Id,
                        Stuff=stuff,
                        StuffId=stuff.Id
                    });
                }
                if(stuffListToAdd.Count>0)
                    _context.ArticleTypesStuffs.AddRange(stuffListToAdd);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to assign stuffs");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}