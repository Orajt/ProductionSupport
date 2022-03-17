namespace Application.Companies
{
    public class ListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyIdentifier { get; set; } = "";
        public bool Seller { get; set; }=false;
        public bool Buyier{get;set;}=false;
    }
}