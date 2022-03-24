namespace Application.DeliveryPlace
{
    public class DetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public int NumberOfBuilding { get; set; }
        public int Apartment { get; set; } = 0;
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
    }
}