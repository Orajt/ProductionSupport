using Application.Core;
using Application.Interfaces;
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
            private readonly IUnitOfWork _unitOfWork;
            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var articleType = await _unitOfWork.ArticleTypes.GetArticleTypeWithStuffs(request.Id);

                var relationsBetweenArticleTypeAndStuffsToRemove = articleType.Stuffs.Where(p => !request.Stuffs.Contains(p.StuffId)).ToList();

                if (relationsBetweenArticleTypeAndStuffsToRemove.Count > 0)
                    _unitOfWork.ArticleTypesStuffs.RemoveRange(relationsBetweenArticleTypeAndStuffsToRemove);


                var stuffsToAssign = await _unitOfWork.Stuffs.GetAll();
                var stuffListToAdd = new List<Domain.ArticleTypeStuff>();

                foreach (var stuffId in request.Stuffs)
                {
                    var stuff = stuffsToAssign.FirstOrDefault(p => p.Id == stuffId);
                    if (stuff == null) return null;

                    if (articleType.Stuffs.Any(p => p.StuffId == stuffId)) continue;

                    stuffListToAdd.Add(new Domain.ArticleTypeStuff
                    {
                        ArticleType = articleType,
                        ArticleTypeId = articleType.Id,
                        Stuff = stuff,
                        StuffId = stuff.Id
                    });
                }
                if (stuffListToAdd.Count > 0)
                    _unitOfWork.ArticleTypesStuffs.AddRange(stuffListToAdd);

                var result = await _unitOfWork.SaveChangesAsync();

                if (!result) return Result<Unit>.Failure("Failed to manage stuffs");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}