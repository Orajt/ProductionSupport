namespace Application.ArticleFabricRealization
{
    public class QuanityPerGroup
    {
        public int GroupId { get; set; }
        public int StuffId { get; set; }
        public string CalculatedCode { get; set; }
        public bool QuanityChaanged {get;set;}
        public float Quanity { get; set; }
    }
    public class ArticleFRByStuff
    {
        public string StuffName { get; set; }
        public int StuffId { get; set; }
        public List<QuanityPerGroup> GroupsQuanities { get; set; } = new List<QuanityPerGroup>();
    }
    public class ArticleFRDetailsDto
    {
       public string ArticleName { get; set; }
       public string VariantGroup { get; set; }
       public List<ArticleFRByStuff> GroupByStuffList {get;set;} = new List<ArticleFRByStuff>();
    }
}