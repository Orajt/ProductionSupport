namespace Application.Orders
{
    public class DetailsDto
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
        public List<PositionDto> OrderPostions {get;set;}
    }
    public class PositionDto
    {
        public long Id { get; set; }
        public int Lp { get; set; }
        public string ArticleName { get; set; }
        public int Quanity { get; set; }
        public string Realization { get; set; }
        public string Client { get; set; }
        public int SetId { get; set; }

    }

}