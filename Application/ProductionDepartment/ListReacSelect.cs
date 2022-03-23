using Application.Core;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ProductionDepartment
{
    public class ListReacSelect
    {
        public class Query : IRequest<Result<List<ReactSelectInt>>>
        {
            
        }

        public class Handler : IRequestHandler<Query, Result<List<ReactSelectInt>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<List<ReactSelectInt>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var productionDepartments = await _context.ProductionDepartments
                    .Select(p=>new{p.Id, p.Name})
                    .OrderBy(p=>p.Name)
                    .ToListAsync();
                var resultList=new List<ReactSelectInt>();
                foreach(var prodDep in productionDepartments)
                {
                    resultList.Add(new ReactSelectInt{Label=prodDep.Name, Value=prodDep.Id});
                }

                return Result<List<ReactSelectInt>>.Success(resultList);
            }
        }
    }
}