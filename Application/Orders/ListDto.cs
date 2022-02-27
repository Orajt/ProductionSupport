namespace Application.Orders
{
    public class ListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime EditDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime ProductionDate{get;set;}
        public bool Done { get; set; } = false;
        public int DeliveryPlaceId { get; set; }
        public string DeliveryPlaceName { get; set; }
        public bool FabricsCalculated { get; set; }

    }
}