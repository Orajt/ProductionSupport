namespace Domain
{
    public class DeliveryPlace
    {
        public int Id { get; set; }
        public string DepotName { get; set; }
        public string Adress { get; set; }
        public int CompanyID { get; set; }
        public Company Company { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}