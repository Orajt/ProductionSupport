namespace Domain
{
    public class FamillyFabricVariantGroup
    {
        public int FamillyId { get; set; }
        public Familly Familly { get; set; }
        public int FabricVariantGroupId { get; set; }
        public FabricVariantGroup FabricVariantGroup { get; set; }
    }
}