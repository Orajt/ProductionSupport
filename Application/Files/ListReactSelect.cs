using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Persistence;

namespace Application.Files
{
    public class ListReactSelect
    {
        public class Query : IRequest<Result<List<ReactSelectInt>>>
        {
            public string Type { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ReactSelectInt>>>
        {
            private readonly DataContext _context;
            private readonly IWebHostEnvironment _env;
            public Handler(DataContext context, IWebHostEnvironment env)
            {
                _env = env;
                _context = context;
            }

            public async Task<Result<List<ReactSelectInt>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = new List<ReactSelectInt>();
                int i=0;
                if (request.Type == "pdf")
                {
                    string pdfFolderPath = Path.Combine(_env.WebRootPath);
                    DirectoryInfo pdfDirectoryInfo = new DirectoryInfo(pdfFolderPath);
                    FileInfo[] PdfFiles = pdfDirectoryInfo.GetFiles(searchPattern: "*.pdf");
                    foreach(var file in PdfFiles)
                    {
                        result.Add(new ReactSelectInt(){
                            Label=file.Name,
                            Value=i
                        });
                        i++;
                    }
                }
                if(request.Type=="jpg")
                {
                    string imageFolderPath = Path.Combine(_env.WebRootPath,"images");
                    DirectoryInfo imageDirectoryInfo = new DirectoryInfo(imageFolderPath);
                    FileInfo[] ImageFiles = imageDirectoryInfo.GetFiles(searchPattern: "*.jpg");
                    foreach(var file in ImageFiles)
                    {
                        var checkTh = file.Name.Substring(0,3);
                        if(checkTh=="th_")
                            continue;
                        result.Add(new ReactSelectInt(){
                            Label=file.Name,
                            Value=i
                        });
                        i++;
                    }
                }

                return Result<List<ReactSelectInt>>.Success(result);
            }
        }
    }
}