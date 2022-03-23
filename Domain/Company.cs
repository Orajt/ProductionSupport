

using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyIdentifier { get; set; } = "";
        [Required]
        public bool Supplier { get; set; }=false;
        public bool Merchant { get; set; }=false;
        public ICollection<DeliveryPlace> DeliveryPlaces { get; set; } = new List<DeliveryPlace>();
        public ICollection<CompanyArticle> Articles { get; set; } = new List<CompanyArticle>();

    }
}