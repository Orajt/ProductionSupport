namespace Application.Orders
{
    public class OrderSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime EditDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime ProductionDate { get; set; }
        public string DeliveryPlaceName { get; set; }
        public List<OrderSummaryClientDto> Positions{get;set;}=new List<OrderSummaryClientDto>();

    }
}