using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Familly
{
    public class ManageFabricVariantGroup
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
            public List<int> FabricVariantGroups { get; set; }
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
                var familly=await _context.Famillies.Include(p=>p.FabricVariantGroups).FirstOrDefaultAsync(p=>p.Id==request.Id);

                var groupsListToRemove = familly.FabricVariantGroups.Where(p=>!request.FabricVariantGroups.Contains(p.FabricVariantGroupId)).ToList();

                if(groupsListToRemove.Count>0)
                    _context.FamilliesFabricVarianGroups.RemoveRange(groupsListToRemove);
                

                var groupsToAssign = await _context.FabricVariantGroups.ToListAsync();
                var groupsToAdd = new List<Domain.FamillyFabricVariantGroup>();

                foreach(var fabricVariantGroupId in request.FabricVariantGroups)
                {
                    var fVG=groupsToAssign.FirstOrDefault(p=>p.Id==fabricVariantGroupId);
                    if(fVG==null) return null;
                    if(familly.FabricVariantGroups.Any(p=>p.FabricVariantGroupId==fabricVariantGroupId)) continue;
                    groupsToAdd.Add(new Domain.FamillyFabricVariantGroup{
                        Familly=familly,
                        FamillyId=familly.Id,
                        FabricVariantGroup=fVG,
                        FabricVariantGroupId=fVG.Id
                    });
                }
                if(groupsToAdd.Count>0)
                    _context.FamilliesFabricVarianGroups.AddRange(groupsToAdd);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to manage fabric variants");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}