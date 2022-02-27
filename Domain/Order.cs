using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime EditDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime ProductionDate{get;set;}
        public bool Done { get; set; } = false;
        public int DeliveryPlaceId { get; set; }
        [Required]
        public DeliveryPlace DeliveryPlace { get; set; }
        public ICollection<OrderPosition> OrderPositions { get; set; }= new List<OrderPosition>();
        public bool FabricsCalculated { get; set; } = false;
    }
}