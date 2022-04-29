using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Stuff
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
               RuleFor(p=>p.Id).NotNull().GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public Handler(IUnitOfWork context)
            {
                _unitOfWork = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                if(await _unitOfWork.Articles.Any(p=>p.StuffId==request.Id))
                {
                    return Result<Unit>.Failure("Stuff is being used in some articles");
                }
                var stuff = await _unitOfWork.Stuffs.Find(request.Id);
                
                if(stuff==null)
                    return null;

                _unitOfWork.Stuffs.Remove(stuff);

                var result = await _unitOfWork.SaveChangesAsync();

                if (!result) return Result<Unit>.Failure("Failed to create stuff");
                
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}