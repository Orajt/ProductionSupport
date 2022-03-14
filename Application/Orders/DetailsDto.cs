using Application.OrderPosition;

namespace Application.Orders
{
    public class DetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime EditDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime ProductionDate { get; set; }
        public bool Done { get; set; } = false;
        public ReactSelectInt DeliveryPlace
        {
            get => new ReactSelectInt{Label=this.DeliveryPlaceName, Value=this.DeliveryPlaceId};
        }
        public int DeliveryPlaceId { get; set; }
        public string DeliveryPlaceName { get; set; }
        public bool FabricsCalculated { get; set; }
        public List<PositionDto> OrderPositions { get; set; } = new List<PositionDto>();
    }

}