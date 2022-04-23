using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Article
{
    public class ChaangeDates
    {
        public class Query : IRequest<Result<DetailsDto>>
        {
            
        }

        public class Handler : IRequestHandler<Query, Result<DetailsDto>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<DetailsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var articles = await _context.Articles.ToListAsync();

                for(int i=0;i<articles.Count;i++)
                {
                    var newDate = articles[i].CreateDate.Date;
                    articles[i].CreateDate=newDate;
                    newDate=articles[i].EditDate.Date;
                    articles[i].EditDate=newDate;
                }

                var orders = await _context.Orders.ToListAsync();

                for(int i=0;i<orders.Count;i++)
                {
                    var newDate = orders[i].EditDate.Date;
                    orders[i].EditDate=newDate;

                    newDate=orders[i].ShipmentDate.Date;
                    orders[i].ShipmentDate=newDate;

                    newDate=orders[i].ProductionDate.Date;
                    orders[i].ProductionDate=newDate;
                }

                var result = await _context.SaveChangesAsync() > 0;

                return Result<DetailsDto>.Success(new DetailsDto());
            }
        }
    }
}