namespace Application.Core
{
    public class ArticleTypeComponents
    {
        public ArticleTypeComponents(int articleTypeId, bool hasFamilly, bool HasStuff)
        {
            this.ArticleTypeId=articleTypeId;
            this.HasFamilly=hasFamilly;
            this.HasStuff=HasStuff;
        }
        public int ArticleTypeId { get; set; }
        public bool HasStuff { get; set; }
        public bool HasFamilly { get; set; }
    }
}