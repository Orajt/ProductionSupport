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
            public string FileIdentifier { get; set; }
            public string FileType { get; set; }
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
                int id = 0;
                Domain.ArticleFilePath file = null;
                if (int.TryParse(request.FileIdentifier, out id))
                {
                    file = await _context.ArticlesFilesPaths.FirstOrDefaultAsync(p => p.Id == id && p.FileType==request.FileType);
                }
                if (id == 0)
                {
                    file = await _context.ArticlesFilesPaths.FirstOrDefaultAsync(p => p.FileName == request.FileIdentifier && p.FileType==request.FileType);
                }

                if (file == null) return null;
                

                var path=Path.Combine(_env.WebRootPath,file.Path);
                
                var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                string contentType = "image/jpeg";
                if (file.FileType == "pdf")
                {
                    contentType = "application/pdf";
                }
                var result = new FileStreamResult(stream, contentType);
                result.FileDownloadName = file.FileName;
                return Result<FileStreamResult>.Success(result);
            }
        }
    }
}