using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;

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
            private readonly IUnitOfWork _unitOfWork;
            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var fabric = await _unitOfWork.Articles.Find(request.Id);
                if (fabric.FullName.ToUpper() != request.FullName.ToUpper())
                {
                    if (await _unitOfWork.Articles.IsArticleNameUsed(request.FullName,6,request.StuffId))
                    {
                        return Result<Unit>.Failure("Fabric with that name exists in database");
                    }
                }
                var stuff = await _unitOfWork.Stuffs.Find(request.StuffId);
                if (stuff == null) return null;

                fabric.FullName = request.FullName;
                fabric.NameWithoutFamilly = request.FullName;
                fabric.StuffId = request.StuffId;
                fabric.EditDate = DateTime.Now.Date;

                var result = await _unitOfWork.SaveChangesAsync();

                if (!result) return Result<Unit>.Failure("Failed to edit fabric");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}