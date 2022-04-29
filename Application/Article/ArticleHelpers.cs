using Application.Core;

namespace Application.Article
{
    public interface IArticleHelpers
    {
        string ArticleProperitiesError(ArticleTypeComponents articleTypeProperties, ArticleAdditionalProperties articleProperties);
        Domain.Article ReplaceArticleProperitiesToNewer(Domain.Article oldArticle, ArticleAdditionalProperties newArticleProperties, Edit.Command request);
    }

    public class ArticleHelpers : IArticleHelpers
    {
        public string ArticleProperitiesError(ArticleTypeComponents articleTypeProperties, ArticleAdditionalProperties articleProperties)
        {
            if (articleTypeProperties.HasFamilly && articleProperties.Familly == null)
                return "Article needs familly";
            if (articleTypeProperties.HasStuff && articleProperties.Stuff == null)
                return "Article needs stuff";
            if (articleTypeProperties.HasFabicVariantGroup && articleProperties.FabricVariantGroup == null)
                return "Article needs fabric variant group";
            return "";
        }
        public Domain.Article ReplaceArticleProperitiesToNewer(Domain.Article oldArticle, ArticleAdditionalProperties newArticleProperties, Edit.Command request)
        {
            if (newArticleProperties.Familly != null)
            {
                oldArticle.Familly = newArticleProperties.Familly;
                oldArticle.FamillyId = newArticleProperties.Familly.Id;
            }
            if (newArticleProperties.Stuff != null)
            {
                oldArticle.Stuff = newArticleProperties.Stuff;
                oldArticle.StuffId = newArticleProperties.Stuff.Id;
            }
            if (newArticleProperties.FabricVariantGroup != null)
            {
                oldArticle.FabricVariant = newArticleProperties.FabricVariantGroup;
                oldArticle.FabricVariantGroupId = newArticleProperties.FabricVariantGroup.Id;
            }
            oldArticle.Width = request.Width;
            oldArticle.Length = request.Length;
            oldArticle.High = request.High;
            oldArticle.CreatedInCompany = request.CreatedInCompany;
            oldArticle.CalculateCapacity();
            oldArticle.FullName = request.FullName;
            oldArticle.NameWithoutFamilly = request.NameWithoutFamilly;
            oldArticle.EditDate = DateHelpers.SetDateTimeToCurrent(DateTime.Now).Date;
            return oldArticle;
        }

    }
}