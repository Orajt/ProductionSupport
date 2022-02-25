namespace API.DTOs.Article
{
    public class ArticleListDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ArticleTypeName { get; set; }
        public string FamillyName{get;set;}
        public string EditDate { get; set; }
        public string CreateDate { get; set; }
        public string StuffName{get;set;}

    }
}