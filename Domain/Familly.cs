namespace Domain
{
    public class Familly
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Article> Articles { get; set; }=new List<Article>();
        public ICollection<FamillyFabricVariantGroup> FabricVariantGroups { get; set; }= new List<FamillyFabricVariantGroup>();
    }
}