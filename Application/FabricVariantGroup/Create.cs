using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.FabricVariantGroup
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public List<CreateDto> ListOfFabricVariants { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.ListOfFabricVariants.Count()).GreaterThan(0);
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
                var fabricVariantIds = request.ListOfFabricVariants.Select(p => p.FabricVariantId).ToList();

                if (fabricVariantIds.Distinct().Count() != fabricVariantIds.Count())
                    return Result<Unit>.Failure($"Duplicated fabric variant");

                var fabricVariants = await _context.FabricVariants.Where(p => fabricVariantIds.Contains(p.Id)).ToListAsync();

                if (fabricVariants.Count() != fabricVariantIds.Count())
                    return null;

                var orderedFabricVariantList = request.ListOfFabricVariants.OrderBy(p => p.PlaceInGroup).ToList();

                var newFabricVariantGroup = new Domain.FabricVariantGroup();

                for (int i = 0; i < orderedFabricVariantList.Count; i++)
                {
                    newFabricVariantGroup.Name += fabricVariants.FirstOrDefault(p => p.Id == orderedFabricVariantList[i].FabricVariantId).ShortName + "+";
                }
                if(!String.IsNullOrEmpty(newFabricVariantGroup.Name))
                    newFabricVariantGroup.Name=newFabricVariantGroup.Name.Remove(newFabricVariantGroup.Name.Length-1);
                
                if(await _context.FabricVariantGroups.AnyAsync(p=>p.Name==newFabricVariantGroup.Name))
                    return Result<Unit>.Failure("Group with choosen fabric variants in choosen order already exist in database");
                
                _context.FabricVariantGroups.Add(newFabricVariantGroup);

                var fabricVariantGroupMembers = new List<Domain.FabricVariantFabricGroupVariant>();
                int placeInGroup=1;
                foreach(var fabricVariant in orderedFabricVariantList)
                {
                    fabricVariantGroupMembers.Add(new Domain.FabricVariantFabricGroupVariant(){
                        FabricVariantId=fabricVariant.FabricVariantId,
                        FabricVariant=fabricVariants.FirstOrDefault(p=>p.Id==fabricVariant.FabricVariantId),
                        FabricVariantGroupId=newFabricVariantGroup.Id,
                        FabricVariantGroup=newFabricVariantGroup,
                        PlaceInGroup=placeInGroup
                    });
                    placeInGroup++;
                }
                _context.FabricVariantsGroupVariants.AddRange(fabricVariantGroupMembers);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create fabric group variant");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}