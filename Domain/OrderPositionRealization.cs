using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class OrderPositionRealization
    {
        public long Id { get; set; }
        public long OrderPositionId { get; set; }
        public OrderPosition OrderPosition { get; set; }
        [ForeignKey("Variant")]
        public int VarriantId { get; set; }
        public FabricVariant Variant { get; set; }
        public int FabricId { get; set; }
        public Article Fabric { get; set; }
        public int PlaceInGroup { get; set; }
    }
}