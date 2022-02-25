namespace Domain
{
    public class Article
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NameWithoutFamilly { get; set; }
        public ICollection<ArticleFilePath> FilePaths { get; set; }
        public int ArticleTypeId { get; set; }
        public ArticleType ArticleType { get; set; }
        public int? FamillyId { get; set; }
        public Familly Familly { get; set; } 
        public DateTime EditDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int? StuffId { get; set; }
        public Stuff Stuff { get; set; }
        public int? FabricVariantGroupId { get; set; }
        public FabricVariantGroup FabricVariant { get; set; }
        public ICollection<OrderPosition> OrderPosition { get; set; }
        public decimal Price { get; set; } = 0;
        public int Length { get; set; } = 0;
        public int Width { get; set; } = 0;
        public int High { get; set; } = 0;
        public float Area { get; set; } = 0;
        public float Capacity { get; set; } = 0;
        public bool CompletedInCompany { get; set; } = true;
        public bool CreatedInCompany { get; set; } = true;
        public bool HasInfluenceOnOrder { get; set; } = true;
        public ICollection<ArticleProductionDepartment> ProductionDepartments { get; set; }= new List<ArticleProductionDepartment>();
        public ICollection<CompanyArticle> Companies { get; set; }= new List<CompanyArticle>();
        public ICollection<ArticleFabricRealization> Realizations { get; set; } = new List<ArticleFabricRealization>();
        public virtual List <ArticleArticle> ChildRelations { get; set; }
        public virtual List <ArticleArticle> ParentRelations { get; set; }

        public void CalculateCapacity()
        {
            if(this.Width!=0 & this.Length!=0)
                this.Area=this.Width*this.Length;
            if(this.Area!=0 && this.High!=0)
                this.Capacity=this.Area*this.High;
            return;
            
        }
    }
}