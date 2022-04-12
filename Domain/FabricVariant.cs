namespace Domain
{
    public class FabricVariant
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public ICollection<FabricVariantFabricGroupVariant> FabricVariantGroups { get; set; }=new List<FabricVariantFabricGroupVariant>();
    }
}