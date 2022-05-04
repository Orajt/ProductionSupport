using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Article
{
    public class CreateFabric
    {
        public class Command : IRequest<Result<Unit>>
        {
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
            private readonly IUnitOfWork _unitOfWork;
            public Handler(DataContext context, IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _unitOfWork.Articles.IsArticleNameUsed(request.FullName, 6, request.StuffId))
                {
                    return Result<Unit>.Failure("Fabrics with that name exists in database");
                }
                var stuff = await _unitOfWork.Stuffs.Find(request.StuffId);
                if (stuff == null) return null;

                var date = DateTime.Now.Date;
                var article = new Domain.Article
                {
                    FullName = request.FullName,
                    NameWithoutFamilly = request.FullName,
                    ArticleTypeId = 6,
                    EditDate = date,
                    CreateDate = date,
                    CreatedInCompany = false,
                    HasChild = false,
                    Stuff = stuff,
                    StuffId = stuff.Id
                };

                _unitOfWork.Articles.Add(article);

                var result = await _unitOfWork.SaveChangesAsync();

                if (!result) return Result<Unit>.Failure("Failed to create Fabric");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}