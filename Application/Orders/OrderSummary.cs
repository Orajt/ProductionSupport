using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Orders
{
    public class OrderSummary
    {
        public class Query : IRequest<Result<DetailsDto>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<DetailsDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<DetailsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var order = await _context.Orders
                    .ProjectTo<DetailsDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(p => p.Id == request.Id);
                order.OrderPositions = order.OrderPositions.OrderBy(p => p.Client).ThenBy(p => p.SetId).ThenBy(p => p.Lp).ToList();

                var orderToReturn = _mapper.Map<OrderSummaryDto>(order);
                var orderPositionsGroupedByClient = order.OrderPositions.GroupBy(p => p.Client).ToList();
                foreach (var groupClient in orderPositionsGroupedByClient)
                {
                    orderToReturn.Positions.Add(new OrderSummaryClientDto{
                        Client=groupClient.Key
                    });
                    var orderPositionsGroupedByClientAndSet=groupClient.GroupBy(p=>p?.SetId).ToList();
                    foreach(var groupSet in orderPositionsGroupedByClientAndSet)
                    {
                        string setName="";
                        for(int i=0;i<groupSet.Count();i++)
                        {
                            var iteratedMember = groupSet.ElementAt(i);
                            if(i==0)
                            {
                                setName+=iteratedMember.ArticleName;
                                continue;
                            }
                            var previousMember = groupSet.ElementAt(i-1);
                            if(iteratedMember.ArticleName.Contains(" ") && 
                            previousMember.ArticleTypeId==iteratedMember.ArticleTypeId)
                            {
                                setName+=$"-{iteratedMember.ArticleName.Split(' ')[1]}";
                                continue;
                            }
                            if(previousMember.ArticleTypeId!=iteratedMember.ArticleTypeId){
                                setName+=$"+{iteratedMember.ArticleName}";
                            }
                            
                        }
                        orderToReturn.Positions[orderToReturn.Positions.Count-1].Positions.Add(new OrderSummaryPositionDto{
                            SetName=setName,
                            SetId=groupSet.Key!=null ? (int)groupSet.Key : 0,
                            Positions=groupSet.Select(p=>p).ToList()
                        });
                    }
                }

                return Result<DetailsDto>.Success(order);
            }
        }
    }
}