using Application.Core;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.TreeHelpers
{
    public class CalculateArticlesBasedOnArticleTypeId
    {
        public static async Task<List<CalculateArticlesBasedOnArticleTypeResult>> CalculateArticles(int articleId, int multiplier, int articleTypeId, 
            List<CalculateArticlesBasedOnArticleTypeResult> result, DataContext context, bool hasFamilly, bool hasStuff, bool firstStep, int orderId)
        {
            if(firstStep==true)
            {
                var orderPositionsQuery= context.OrderPositions.Where(p=>p.OrderId==orderId).Include(p=>p.Article).AsQueryable();
                if(hasFamilly && hasStuff)
                    orderPositionsQuery.Include(p=>p.Article).ThenInclude(p=>p.ChildRelations).ThenInclude(p=>p.ChildArticle).ThenInclude(p=>p.Stuff)
                                .Include(p=>p.Article).ThenInclude(p=>p.ChildRelations).ThenInclude(p=>p.ChildArticle).ThenInclude(p=>p.Familly);
                else if(hasFamilly)
                    orderPositionsQuery.Include(p=>p.Article).ThenInclude(p=>p.ChildRelations).ThenInclude(p=>p.ChildArticle).ThenInclude(p=>p.Familly);
                else if(hasStuff)
                    orderPositionsQuery.Include(p=>p.Article).ThenInclude(p=>p.ChildRelations).ThenInclude(p=>p.ChildArticle).ThenInclude(p=>p.Stuff);
                var orderPositions=await orderPositionsQuery.ToListAsync();
                if(orderPositions.Count==0) return null;
                foreach(var orderPosition in orderPositions)
                {
                    var article=orderPosition.Article;
                    var multiplierMultiplied = multiplier*orderPosition.Quanity;
                    if(article.ArticleTypeId==articleTypeId)
                    {
                        result.Add(new CalculateArticlesBasedOnArticleTypeResult
                        {
                            ArticleId=article.Id, 
                            ArticleName=article.FullName, 
                            ParentArticleId=null, 
                            ParentArticleName="",
                            StuffId=hasStuff ? article.StuffId : null,
                            StuffName=hasStuff ? article.Stuff.Name : "",
                            FamillyId=hasFamilly ? article.FamillyId : null,
                            FamillyName=hasFamilly ? article.Familly.Name : "",
                            Quanity=multiplierMultiplied
                        });

                        continue;
                    }
                   var gowno= await CalculateArticles(article.Id, multiplierMultiplied, articleTypeId, result, context, hasFamilly, hasStuff, false,0);
                }
            } 
            if(firstStep==false)
            {
                var article = await context.Articles.Include(p=>p.ChildRelations).ThenInclude(p=>p.ChildArticle).ThenInclude(p=>p.Stuff)
                                .Include(p=>p.ChildRelations).ThenInclude(p=>p.ChildArticle).ThenInclude(p=>p.Familly).FirstOrDefaultAsync(p=>p.Id==articleId);
                // if(hasFamilly && hasStuff)
                //     articleQuery.Include(p=>p.ChildRelations).ThenInclude(p=>p.ChildArticle).ThenInclude(p=>p.Stuff)
                //                 .Include(p=>p.ChildRelations).ThenInclude(p=>p.ChildArticle).ThenInclude(p=>p.Familly);
                // else if(hasFamilly)
                //     articleQuery.Include(p=>p.ChildRelations).ThenInclude(p=>p.ChildArticle).ThenInclude(p=>p.Familly);
                // else if(hasStuff)
                //     articleQuery.Include(p=>p.ChildRelations).ThenInclude(p=>p.ChildArticle).ThenInclude(p=>p.Stuff);

                // var article =await articleQuery.FirstOrDefaultAsync(p=>p.Id==articleId);

                foreach(var relation in article.ChildRelations)
                {
                    var multiplierMultiplied=multiplier*relation.Quanity;
                    if(relation.ChildArticle.ArticleTypeId==articleTypeId)
                    {
                        result.Add(new CalculateArticlesBasedOnArticleTypeResult
                        {
                            ArticleId=relation.ChildId, 
                            ArticleName=relation.ChildArticle.FullName, 
                            ParentArticleId=article.Id, 
                            ParentArticleName=article.FullName,
                            StuffId=hasStuff ? relation.ChildArticle.StuffId : null,
                            StuffName=hasStuff ? relation.ChildArticle.Stuff.Name : "",
                            FamillyId=hasFamilly ? relation.ChildArticle.FamillyId : null,
                            FamillyName=hasFamilly ? relation.ChildArticle.Familly.Name : "",
                            Quanity=multiplierMultiplied
                        });
                        continue;

                    }
                    await CalculateArticles(relation.ChildId, multiplierMultiplied, articleTypeId, result, context, hasFamilly, hasStuff, false,0);
                }
            }
            return result;
        }


    }
}