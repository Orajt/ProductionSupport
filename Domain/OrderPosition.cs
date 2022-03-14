using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class OrderPosition
    {
        public long Id { get; set; }
        public int OrderId { get; set; }
        [Required]
        public Order Order { get; set; }
        public int ArticleId { get; set; }
        [Required]
        public Article Article { get; set; }
        public int Quanity { get; set; }
        public string Realization { get; set; }
        public string Client { get; set; }
        public int Lp { get; set; }
        public int? SetId { get; set; }
        public Set Set{get;set;}
        public decimal FabricPirce { get; set; } = 0;
        public string CalculatedRealization { get; set; }="";
        public ICollection<OrderPositionRealization> Realizations { get; set; }=new List<OrderPositionRealization>();

    }
}