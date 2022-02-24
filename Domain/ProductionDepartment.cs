namespace Domain
{
    public class ProductionDepartment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<AppUser> AppUsers { get; set; } = new List<AppUser>();
        public ICollection<ArticleProductionDepartment> Articles { get; set; } = new List<ArticleProductionDepartment>();
    }
}