namespace Application.Core
{
    public class ArticleTypeComponents
    {
        public ArticleTypeComponents(int articleTypeId, bool hasFamilly, bool HasStuff, bool hasFVG)
        {
            this.ArticleTypeId = articleTypeId;
            this.HasFamilly = hasFamilly;
            this.HasStuff = HasStuff;
            this.HasFabicVariantGroup=hasFVG;
        }
        public ArticleTypeComponents()
        {
            
        }
        public int ArticleTypeId { get; set; }
        public bool HasStuff { get; set; }
        public bool HasFamilly { get; set; }
        public bool HasFabicVariantGroup { get; set; }
    }
}