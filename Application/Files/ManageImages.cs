using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

namespace Application.Files
{
    public class ManageImages
{
        public class Command : IRequest<Result<Unit>>
        {
            public int ArticleId { get; set; }
            public List<string> ExsistingFiles { get; set; }
            public List<IFormFile> Files { get; set; }
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
                    "image/jpeg",
                };
                string imagesFolderPath = Path.Combine(_env.WebRootPath, "images");
                bool result = false;
                DirectoryInfo pdfDirectoryInfo = new DirectoryInfo(imagesFolderPath);
                FileInfo[] ImgFiles = pdfDirectoryInfo.GetFiles("*jpg");

                var article = await _context.Articles.Include(p => p.FilePaths).FirstOrDefaultAsync(p => p.Id == request.ArticleId);

                if (article == null)
                    return null;
                var filesToAssign = new List<Domain.ArticleFilePath>();
                if (request.ExsistingFiles == null && request.Files == null)
                {

                    var pathsToSeparate = article.FilePaths.Where(p => p.FileType == "jpg" || p.FileType == "thumb").ToList();
                    foreach (var path in pathsToSeparate)
                    {
                        if (!await _context.ArticlesFilesPaths.AnyAsync(p => p.FileName == path.FileName && p.ArticleId != article.Id))
                        {
                            var pathToDelete = _env.WebRootPath + path.Path;
                            File.Delete(_env.WebRootPath + path.Path);
                        }
                    }
                    _context.RemoveRange(pathsToSeparate);

                    result = await _context.SaveChangesAsync() > 0;

                    if (!result) return Result<Unit>.Failure("Failed to manage files");

                    return Result<Unit>.Success(Unit.Value);
                }

                if (request.ExsistingFiles != null && request.ExsistingFiles.Count > 0)
                {
                    if(request.ExsistingFiles.Count()!=article.FilePaths.Where(p=>p.FileType=="jpg").Count())
                    {
                        var pathsToSeparate=article.FilePaths.Where(p => (p.FileType == "jpg" || p.FileType == "thumb") && !request.ExsistingFiles.Contains(p.FileName)).ToList();
                        _context.RemoveRange(pathsToSeparate);
                    }
                    foreach (var fileName in request.ExsistingFiles)
                    {
                        var oldFile = article.FilePaths.FirstOrDefault(p => p.FileName == fileName);
                        if (oldFile == null)
                        {
                            var exisitingFile = ImgFiles.FirstOrDefault(p => p.Name == fileName);
                            var shortPath = exisitingFile.FullName.Substring(_env.WebRootPath.Length);
                            
                            var thumbShortPath = Path.Combine("images",$"th_{exisitingFile.Name}");
                            if (exisitingFile == null) return null;

                            filesToAssign.Add(new Domain.ArticleFilePath()
                            {
                                FileType = "jpg",
                                FileName = fileName,
                                Article = article,
                                ArticleId = article.Id,
                                Path = shortPath
                            });
                            filesToAssign.Add(new Domain.ArticleFilePath()
                            {
                                FileType = "thumb",
                                FileName = fileName,
                                Article = article,
                                ArticleId = article.Id,
                                Path = thumbShortPath
                            });
                        }
                    }
                }
                if (request.Files != null && request.Files.Count > 0)
                {
                    foreach (var file in request.Files)
                    {
                        if (file.FileName.Length < 4)
                            return Result<Unit>.Failure($"{file.FileName} name is too short (should've more than 5 characters)");

                        if (file.ContentType == "image/jpeg")
                        {
                            if (file.FileName.Substring(file.FileName.Length - 3) != "jpg")
                                return Result<Unit>.Failure("Image filenames last 3 characters should be jpg");

                            var filePath = Path.Combine(imagesFolderPath,$"{file.FileName}");
                            var thumbPath = Path.Combine(imagesFolderPath,$"th_{file.FileName}");
                            var oldFile = article.FilePaths.FirstOrDefault(p => p.FileName == file.FileName);

                            if (oldFile == null)
                            {
                                if (ImgFiles.Any(p => p.Name == file.Name))
                                    return Result<Unit>.Failure($"File named {file.FileName} exists in database");

                                if (file == null || file.Length == 0)
                                {
                                    return null;
                                }
                                using (var memoryStream = new MemoryStream())
                                {
                                    file.CopyTo(memoryStream);
                                    memoryStream.Position=0;
                                    using (var img = Image.Load(memoryStream))
                                    {
                                        bool imageSaved = false;
                                       var mutateScale = (int)Math.Floor((decimal)img.Width/1024);
                                       if(mutateScale>1)
                                       {
                                           img.Mutate(x=>x.Resize(img.Width/mutateScale, img.Height/mutateScale));
                                           img.Save(filePath);
                                           imageSaved=true;
                                       }
                                       if(!imageSaved)
                                            img.Save(filePath);
                                        img.Mutate(x=>x.Resize(120,120));
                                        img.Save(thumbPath);
                                    }
                                }


                                filesToAssign.Add(new Domain.ArticleFilePath()
                                {
                                    FileType = "jpg",
                                    FileName = file.FileName,
                                    Article = article,
                                    ArticleId = article.Id,
                                    Path = Path.Combine("images",$"{file.FileName}")
                                });
                                filesToAssign.Add(new Domain.ArticleFilePath()
                                {
                                    FileType = "thumb",
                                    FileName = file.FileName,
                                    Article = article,
                                    ArticleId = article.Id,
                                    Path = Path.Combine("images",$"th_{file.FileName}")
                                });
                            }
                        }
                    }
                }
                if (filesToAssign.Count > 0)
                {
                    _context.ArticlesFilesPaths.AddRange(filesToAssign);
                }
                result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to manage files");

                return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}