using System.Drawing;
using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Article
{
    public class ManageFiles
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int ArticleId { get; set; }
            public List<string> ExsistingFiles { get; set; }
            public List<IFormFile> Files { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {

            }
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
                    "image/jpeg"
                };
                string imageFolderPath = _env.WebRootPath + @"\images";
                string pdfFolderPath = _env.WebRootPath + @"\pdfs";


                DirectoryInfo imageDirectoryInfo = new DirectoryInfo(imageFolderPath);
                FileInfo[] ImageFiles = imageDirectoryInfo.GetFiles();

                DirectoryInfo pdfDirectoryInfo = new DirectoryInfo(pdfFolderPath);
                FileInfo[] PdfFiles = pdfDirectoryInfo.GetFiles();


                var article = await _context.Articles.Include(p => p.FilePaths).FirstOrDefaultAsync(p => p.Id == request.ArticleId);

                if (article == null)
                    return null;

                var pathsToSeparate = new List<Domain.ArticleFilePath>();
                var pathsToAssign = new List<Domain.ArticleFilePath>();
                

                ////Manage existing files///
                foreach(var path in article.FilePaths)
                {
                    if(request.ExsistingFiles.Any(p=>p==path.FileName))
                        continue;
                    pathsToSeparate.Add(path);
                }
                _context.RemoveRange(pathsToSeparate);

                ///check if file is no longer assing to any article and delete if istn///
                foreach(var path in pathsToSeparate)
                {
                    if(!_context.ArticlesFilesPaths.Any(p=>p.FileName==path.FileName))
                        File.Delete(path.Path);
                }
                ///assign existing file///
                foreach(var fileName in request.ExsistingFiles)
                {
                    var extension = fileName.Substring(fileName.Length - 3);
                    if(extension=="pdf"){
                        var path=PdfFiles.FirstOrDefault(p=>p.Name==fileName).FullName;
                        if(String.IsNullOrEmpty(path))
                            return null;
                    }
                }


                ////Save new files////
                if (request.Files.Select(p => p.FileName).GroupBy(p => p).Any(p => p.Count() > 1))
                {
                    return Result<Unit>.Failure("Two files has same name");
                }
                

                if (request.Files.Where(p => p.ContentType == "application/pdf").Count() > 1)
                    return Result<Unit>.Failure("You can attach only one pdf file");

                if (request.Files.Where(p => !supportedFiles.Contains(p.ContentType)).Count() > 0)
                    return Result<Unit>.Failure("Unsupported media type");

                
                foreach (var file in request.Files)
                {
                    if (file.FileName.Length < 4)
                        return Result<Unit>.Failure($"{file.FileName} name is too short (should've more than 5 characters)");

                    if (file.ContentType == "image/jpeg")
                    {
                        if (file.FileName.Substring(file.FileName.Length - 3) != "jpg")
                            return Result<Unit>.Failure("Image filenames last 3 characters should be jpg");

                        var filePath = imageFolderPath + @$"\{file.FileName}";

                        if (await _context.ArticlesFilesPaths.AnyAsync(p => p.Path == filePath))
                            return Result<Unit>.Failure($"File named {file.FileName} exists in database");

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await file.CopyToAsync(stream);
                        }

                        Image img = Image.FromFile(filePath);
                        Image thumb = img.GetThumbnailImage(120, 120, ()=>false, IntPtr.Zero);

                        var thumbPath = Path.ChangeExtension(filePath, "thumb");
                        thumb.Save(thumbPath);
                        pathsToAssign.Add(new Domain.ArticleFilePath{
                            Article=article,
                            ArticleId=article.Id,
                            Path=filePath,
                            FileType="img",
                            FileName=file.FileName
                        });
                        pathsToAssign.Add(new Domain.ArticleFilePath{
                            Article=article,
                            ArticleId=article.Id,
                            Path=thumbPath,
                            FileType="thumb",
                            FileName=file.FileName
                        });
                        continue;
                    }
                    
                    if (file.ContentType == "application/pdf")
                    {
                        if (file.FileName.Substring(file.FileName.Length - 3) != "pdf")
                            return Result<Unit>.Failure("Pdf filenames last 3 characters should be pdf");

                        var filePath = pdfFolderPath + @$"\{file.FileName}";

                        if (await _context.ArticlesFilesPaths.AnyAsync(p => p.Path == filePath))
                            return Result<Unit>.Failure($"File named {file.FileName} exists in database");

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await file.CopyToAsync(stream);
                        }

                        pathsToAssign.Add(new Domain.ArticleFilePath{
                            Article=article,
                            ArticleId=article.Id,
                            Path=filePath,
                            FileType="pdf"
                        });
                    }
                }
                _context.ArticlesFilesPaths.AddRange(pathsToAssign);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to manage files");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}