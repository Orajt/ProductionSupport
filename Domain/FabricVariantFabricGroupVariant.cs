namespace Domain
{
    public class FabricVariantFabricGroupVariant
    {
         public int FabricVariantId { get; set; }
        public FabricVariant FabricVariant { get; set; }
        public int FabricVariantGroupId { get; set; }
        public FabricVariantGroup FabricVariantGroup { get; set; }
        public int PlaceInGroup { get; set; }
    }
}