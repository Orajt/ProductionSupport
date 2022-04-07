namespace Application.Article
{
    public class DetailsDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NameWithoutFamilly { get; set; } = "";
        public int ArticleTypeId { get; set; }
        public string ArticleTypeName { get; set; }
        public int? FamillyId { get; set; }
        public string FamillyName { get; set; }
        public DateTime EditDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int StuffId { get; set; }
        public string StuffName { get; set; }
        public int FabricVariantGroupId { get; set; }
        public string FabricVariantGroupName { get; set; }
        public decimal Price { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int High { get; set; }
        public float Area { get; set; }
        public float Capacity { get; set; }
        public bool CreatedInCompany { get; set; }
        public bool AbleToEditPrimaries { get; set; }
        public List<DetailsDtoChildArticles> ChildArticles { get; set; } = new List<DetailsDtoChildArticles>();
        public DetailFileDto PdfFile { get; set; }
        public List<DetailFileDto> Images { get; set; } = new List<DetailFileDto>();
    }
}