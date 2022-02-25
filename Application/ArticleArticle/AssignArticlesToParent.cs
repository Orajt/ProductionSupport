using System.Diagnostics;
using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ArticleArticle
{
    public class AssignArticlesToParent
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int ParentId {get;set;}
            public List<AssignArticleToParentDto> ChildList { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ChildList).NotNull();
                RuleForEach(x => x.ChildList).SetValidator(new AssignChildValidator());
                RuleFor(x => x.ChildList).Must(list =>
                              ListHelpers.IsParametrUnique(list, "Position"))
                            .WithMessage("Postion should be unique");
                RuleFor(x => x.ChildList).Must(list =>
                             ListHelpers.IsParametrUnique(list, "ChildId"))
                           .WithMessage("Childs should be unique");

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
                //Select all child ids
                var childIds = request.ChildList.Select(p => p.ChildId).ToList();
                var parent = await _context.Articles.FirstOrDefaultAsync(p => p.Id == request.ParentId);

                var possibleTypes = Relations.ArticleTypeRelations.Where(p=>p.Parent==parent.ArticleTypeId).Select(p=>p.Child).ToList();
                possibleTypes.Add(parent.ArticleTypeId);

                if(childIds.Any(p=>p==parent.Id))
                    return Result<Unit>.Failure("You can't assign same articles");

                //Check if any element of ChildList is on higher level than parent
                var checkObject = new FindParentsDependsOnArticleType(childIds, _context, parent.ArticleTypeId);
                await checkObject.IsArticleOnHigherLevel(new List<int> { parent.Id });
                if (checkObject.ArticleOnHigherLevel == true)
                    return Result<Unit>.Failure("One of used articles is on upper level than parent you want to assign");

                //Get child elements from database
                var childs = await _context.Articles.Where(t => childIds.Contains(t.Id)).ToListAsync();

                //Check if childs have correct type
                if(!childs.Any(p=>possibleTypes.Contains(p.ArticleTypeId)))
                {
                    return Result<Unit>.Failure("One type of used articles is incompatible to parent type");
                }

                //Create new relations
                var newRelations = new List<Domain.ArticleArticle>();
                foreach (var child in request.ChildList)
                {
                    var childDB = childs.FirstOrDefault(p => p.Id == child.ChildId);
                    if (childDB == null) return null;

                    newRelations.Add(new Domain.ArticleArticle(parent, childDB, child.Quanity, child.Position));
                }
                try
                {
                    await _context.BulkInsertAsync(newRelations);
                }
                catch (Exception)
                {
                    return Result<Unit>.Failure("Failed to Assign articles");
                }
                return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}
