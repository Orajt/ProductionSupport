namespace Domain
{
    public class ArticleProductionDepartment
    {
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public int ProductionDepartmentId { get; set; }
        public ProductionDepartment ProductionDepartment { get; set; }
        public int TimeToProduce { get; set; }
    }
}