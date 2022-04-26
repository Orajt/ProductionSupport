using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Files
{
    public partial class ManagePdfFiles
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int ArticleId { get; set; }
            public string ExsistingFile { get; set; }
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IWebHostEnvironment _env;
            public Handler(DataContext context, IWebHostEnvironment env)
            {
                _env = env;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var supportedFiles = new List<string>(){
                    "application/pdf",
                };
                string pdfFolderPath = _env.WebRootPath;
                bool result = false;
                DirectoryInfo pdfDirectoryInfo = new DirectoryInfo(pdfFolderPath);
                FileInfo[] PdfFiles = pdfDirectoryInfo.GetFiles("*.pdf");

                var article = await _context.Articles.Include(p => p.FilePaths).FirstOrDefaultAsync(p => p.Id == request.ArticleId);

                if (article == null)
                    return null;

                if (String.IsNullOrEmpty(request.ExsistingFile) && request.File == null)
                {

                    var pathToSeparate = article.FilePaths.FirstOrDefault(p => p.FileType == "pdf");
                    if (pathToSeparate != null)
                    {
                        if (!await _context.ArticlesFilesPaths.AnyAsync(p => p.FileName == pathToSeparate.FileName && p.ArticleId != article.Id))
                        {
                            File.Delete(Path.Combine(_env.WebRootPath,pathToSeparate.Path));
                        }
                        _context.Remove(pathToSeparate);
                    }
                    result = await _context.SaveChangesAsync() > 0;

                    if (!result) return Result<Unit>.Failure("Failed to manage files");

                    return Result<Unit>.Success(Unit.Value);
                }


                if (!String.IsNullOrEmpty(request.ExsistingFile) && request.File != null)
                    return Result<Unit>.Failure("You can assign only one pdf file to article");

                if (!String.IsNullOrEmpty(request.ExsistingFile))
                {
                    var extension = request.ExsistingFile.Substring(request.ExsistingFile.Length - 3);
                    if (extension != "pdf")
                    {
                        return Result<Unit>.Failure("Pdf filenames last 3 characters should be pdf");
                    }
                    var oldFile = article.FilePaths.FirstOrDefault(p => p.FileName == request.ExsistingFile);
                    if (oldFile == null)
                    {
                        var pathToSeparate = article.FilePaths.FirstOrDefault(p => p.FileType == "pdf");
                        if (pathToSeparate != null)
                        {
                            if (!await _context.ArticlesFilesPaths.AnyAsync(p => p.FileName == pathToSeparate.FileName && p.ArticleId != article.Id))
                            {
                                var fullPath = _env.WebRootPath+pathToSeparate.Path;
                                File.Delete(fullPath);
                            }
                            _context.Remove(pathToSeparate);
                        }
                        var exisitingFile = PdfFiles.FirstOrDefault(p => p.Name == request.ExsistingFile);

                        if (exisitingFile == null)
                            return null;

                        var fileToAssign = new Domain.ArticleFilePath()
                        {
                            FileType = "pdf",
                            FileName = request.ExsistingFile,
                            Article = article,
                            ArticleId = article.Id,
                            Path = exisitingFile.FullName.Substring(_env.WebRootPath.Length)
                        };

                        _context.ArticlesFilesPaths.Add(fileToAssign);

                        result = await _context.SaveChangesAsync() > 0;

                        if (!result) return Result<Unit>.Failure("Failed to manage files");

                        return Result<Unit>.Success(Unit.Value);
                    }
                    return Result<Unit>.Failure("File you want to attach has same name as previous");
                }

                var file = request.File;

                if (file.FileName.Length < 4)
                    return Result<Unit>.Failure($"{file.FileName} name is too short (should've more than 5 characters)");

                if (file.ContentType == "application/pdf")
                {
                    if (file.FileName.Substring(file.FileName.Length - 3) != "pdf")
                        return Result<Unit>.Failure("Pdf filenames last 3 characters should be pdf");

                    var filePath = Path.Combine(pdfFolderPath, file.FileName);

                    if (PdfFiles.Any(p=>p.Name==file.Name))
                        return Result<Unit>.Failure($"File named {file.FileName} exists in database");

                    var oldFile = article.FilePaths.FirstOrDefault(p => p.FileName == file.FileName);
                    if (oldFile == null)
                    {
                        var pathToSeparate = article.FilePaths.FirstOrDefault(p => p.FileType == "pdf");
                        if (pathToSeparate != null)
                        {
                            if (await _context.ArticlesFilesPaths.AnyAsync(p => p.FileName == pathToSeparate.FileName && p.ArticleId != article.Id))
                            {
                                var fullPath = Path.Combine(_env.WebRootPath,pathToSeparate.Path);
                                File.Delete(fullPath);
                            }
                            _context.Remove(pathToSeparate);
                        }
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var fileToAssign = new Domain.ArticleFilePath()
                        {
                            FileType = "pdf",
                            FileName = file.FileName,
                            Article = article,
                            ArticleId = article.Id,
                            Path = file.FileName
                        };
                        _context.ArticlesFilesPaths.Add(fileToAssign);
                    }

                    result = await _context.SaveChangesAsync() > 0;

                    if (!result) return Result<Unit>.Failure("Failed to manage files");

                    return Result<Unit>.Success(Unit.Value);
                }
                return Result<Unit>.Failure("File you want to attach has same name as previous");
            }
        }
    }
}