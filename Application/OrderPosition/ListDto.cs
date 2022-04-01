namespace Application.OrderPosition
{
    public class ListDto
    {
        public long Id { get; set; }
        public int ArticleId { get; set; }
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public int SetId { get; set; }
        public string Realization { get; set; }
        public string Client { get; set; }
        public int Quanity { get; set; }
        public string ArticleFullName { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime ProductionDate { get; set; }
        public string ArticleTypeName { get; set; }
        public string FamillyName { get; set; }
        public string StuffName { get; set; }

    }
}