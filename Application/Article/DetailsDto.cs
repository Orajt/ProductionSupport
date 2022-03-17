namespace Application.Article
{
    public class DetailsDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NameWithoutFamilly { get; set; }
        public int ArticleTypeId { get; set; }
        public string ArticleTypeName { get; set; }
        public int? FamillyId { get; set; }
        public string FamillyName { get; set; }
        public DateTime EditDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int? StuffId { get; set; }
        public string StuffName { get; set; }
        public int? FabricVariantGroupId { get; set; }
        public string FabricVariantGroupName  { get; set; }
        public decimal Price { get; set; } = 0;
        public int Length { get; set; } = 0;
        public int Width { get; set; } = 0;
        public int High { get; set; } = 0;
        public float Area { get; set; } = 0;
        public float Capacity { get; set; } = 0;
        public bool CreatedInCompany { get; set; } = true;
        public bool HasChild { get; set; } = false;
        public bool HasChildSameArticleType { get; set; } = false;
    }
}