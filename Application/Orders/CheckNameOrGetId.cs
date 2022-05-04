using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Orders
{
    public class CheckNameOrGetId
    {
        public class Query : IRequest<Result<int>>
        {
            public string Name { get; set; }
            public string Predicate{get;set;}
        }

        public class Handler : IRequestHandler<Query, Result<int>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<int>> Handle(Query request, CancellationToken cancellationToken)
            {
                if(request.Predicate=="CHECK"){
                    var isAny = await _context.Orders.AnyAsync(p=>p.Name.ToUpper()==request.Name.ToUpper());
                    return Result<int>.Success(isAny? 1:0);
                }     
                
                var order = await _context.Orders.Select(p=>new {p.Id, p.Name})
                    .FirstOrDefaultAsync(p=>p.Name.ToUpper()==request.Name.ToUpper());
                if(order == null) return null;

                return Result<int>.Success(order.Id);
            }
        }
    }
}