using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Files
{
    public class GetFile
    {
        public class Query : IRequest<Result<FileStreamResult>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<FileStreamResult>>
        {
            private readonly DataContext _context;
            private readonly IWebHostEnvironment _env;
            public Handler(DataContext context, IWebHostEnvironment env)
            {
                _env = env;
                _context = context;
            }

            public async Task<Result<FileStreamResult>> Handle(Query request, CancellationToken cancellationToken)
            {
                var file = await _context.ArticlesFilesPaths.FirstOrDefaultAsync(p => p.Id == request.Id);
                if(file==null)  return null;
                var stream = new FileStream(_env.WebRootPath+file.Path, FileMode.Open, FileAccess.Read);
                string contentType = "image/jpeg";
                if (file.FileType == "pdf")
                {
                    contentType="application/pdf";
                }
                var result = new FileStreamResult(stream, contentType);
                result.FileDownloadName=file.FileName;
                return Result<FileStreamResult>.Success(result);
            }
        }
    }
}