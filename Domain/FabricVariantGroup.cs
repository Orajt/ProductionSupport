namespace Domain
{
    public class FabricVariantGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<FabricVariantFabricGroupVariant> FabricVariants { get; set; } = new List<FabricVariantFabricGroupVariant>();
        public ICollection<Article> Articles { get; set; }=new List<Article>();
        public ICollection<FamillyFabricVariantGroup> Famillies { get; set; }=new List<FamillyFabricVariantGroup>();
    }
}