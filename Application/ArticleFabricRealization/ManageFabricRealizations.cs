using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ArticleFabricRealization
{
    public class ManageFabricRealizations
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int ArticleId { get; set; }
            public List<QuanityPerGroup> QuanityGroups { get; set; } = new List<QuanityPerGroup>();
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {

            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                var article = await _unitOfWork.Articles.GetArticleWithFabricVarGroupWithDetailsAndRealizations(request.ArticleId);

                var orderedVariants = article.FabricVariant.FabricVariants.OrderBy(p => p.PlaceInGroup).ToList();

                var remainingGroups = request.QuanityGroups.Select(p => p.GroupId).ToList();

                var realizationsToDelete = article.Realizations.Where(p => !remainingGroups.Contains(p.Id));
                _unitOfWork.ArticlesFabricRealizations.RemoveRange(realizationsToDelete);

                var usedStuffs = request.QuanityGroups.Select(p => p.StuffId).Distinct().ToList();
                var stuffs =await  _unitOfWork.Stuffs.Where(p => usedStuffs.Contains(p.Id));

                var newArticleFabricRealziations = new List<Domain.ArticleFabricRealization>();

                foreach (var group in request.QuanityGroups)
                {
                    if (group.GroupId != 0 && group.QuanityChaanged)
                    {
                        var realizationToChaange = article.Realizations
                            .FirstOrDefault(p => p.Id == group.GroupId)
                            .FabricLength = group.Quanity;
                    }
                    if (group.GroupId == 0 && !article.Realizations.Any(p => p.StuffId == group.StuffId && p.CalculatedCode == group.CalculatedCode))
                    {
                        var stuff = stuffs.FirstOrDefault(p => p.Id == group.StuffId);

                        newArticleFabricRealziations.Add(new Domain.ArticleFabricRealization
                        {
                            ArticleId = article.Id,
                            Article = article,
                            CalculatedCode = group.CalculatedCode,
                            StuffId = group.StuffId,
                            Stuff = stuff,
                            FabricLength = (float)Math.Round(group.Quanity, 3)
                        });
                    }
                }
                _unitOfWork.ArticlesFabricRealizations.AddRange(newArticleFabricRealziations);

                var result = await _unitOfWork.SaveChangesAsync();

                if (!result) return Result<Unit>.Failure("Failed to manage fabric realizations");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}