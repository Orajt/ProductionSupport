using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Core
{
    public static class TreeHelpers
    {
        public static async void CalculateArticlesBasedOnArticleType(List<Application.Orders.OrderPrintoutDto> result, int articleId, int multiplier, string parentName, int countInParent, int articleTypeToCalculate, DataContext context, List<int> articleTypesToGoThrough)
        {
            var articleInList = result.FirstOrDefault(p => p.ArticleId == articleId);

            var parentInfo = $"{parentName}({countInParent})";

            if (articleInList != null)
            {
                articleInList.Quanity += multiplier;
                if (!articleInList.ParentAndCount.Any(p => p == parentInfo))
                {
                    articleInList.ParentAndCount.Add(parentInfo);
                }
                return;
            }

            var article = await context.Articles
                .Include(p => p.ChildRelations)
                    .ThenInclude(p => p.ChildArticle)
                .Include(p => p.Stuff)
                .Include(p=>p.FilePaths)
                .FirstOrDefaultAsync(p => p.Id == articleId);

            if (article.ArticleTypeId == articleTypeToCalculate)
            {
                var newItem = new Orders.OrderPrintoutDto
                {
                    ArticleId = articleId,
                    ArticleName = article.FullName,
                    StuffId = article.StuffId != null ? (int)article.StuffId : 0,
                    Stuff = article.Stuff != null ? article.Stuff.Name : "none",
                    Quanity = multiplier,
                    Length=article.Length,
                    Width=article.Width,
                    Height=article.High
                };
                var pdfFilePath = article.FilePaths.FirstOrDefault(p => p.FileType == "pdf");
                if (pdfFilePath != null)
                    newItem.PdfId = pdfFilePath.Id;

                newItem.ParentAndCount.Add(parentInfo);

                result.Add(newItem);

                return;
            }
            foreach (var child in article.ChildRelations)
            {
                if (child.ChildArticle.ArticleTypeId == articleTypeToCalculate || (articleTypesToGoThrough.Contains(child.ChildArticle.ArticleTypeId) && child.ChildArticle.HasChild))
                {
                    CalculateArticlesBasedOnArticleType(result, child.ChildId, multiplier * child.Quanity,
                        article.FullName, child.Quanity, articleTypeToCalculate, context, articleTypesToGoThrough);
                }
                continue;
            }
        }
    }
}