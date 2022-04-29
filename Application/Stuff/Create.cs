using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Stuff
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Name { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Name).NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            public IUnitOfWork _unitOfWork { get; }
            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _unitOfWork.Stuffs.Any(p=>p.Name==request.Name))
                    return Result<Unit>.Failure($"Stuff {request.Name} exist in database");

                var newStuff = new Domain.Stuff
                {
                    Name = request.Name,
                };
                
                _unitOfWork.Stuffs.Add(newStuff);

                var result = await _unitOfWork.SaveChangesAsync();

                if (!result) return Result<Unit>.Failure("Failed to create stuff");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}