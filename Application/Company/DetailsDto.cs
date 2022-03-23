namespace Application.Company
{
    public class DetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyIdentifier { get; set; } = "";
        public bool Supplier { get; set; }=false;
        public bool Merchant { get; set; }=false;
        public List<DeliveryPlace.ListDto> DeliveryPlaces=new List<DeliveryPlace.ListDto>();

    }
}