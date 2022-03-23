using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class DeliveryPlace
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
        public Company Company { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        [NotMapped]
        public string NameWithCompany
        {
            get
            {
                return this.Name + $" ({this.Company.Name})";
            }
        }
        [NotMapped]
        public string ShortAddress
        {
            get
            {
                return $"{this.PostalCode} {this.City} {this.Street}";
            }
        }
        [NotMapped]
        public string FullAdress
        {
            get
            {
                return $"{this.PostalCode} {this.City} {this.Street} {this.NumberOfBuilding}/{this.NumberOfBuilding}";
            }
        }


    }
}