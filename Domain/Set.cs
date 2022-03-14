namespace Domain
{
    public class Set
    {
        public int Id{get;set;}
        public ICollection<OrderPosition> OrderPositions {get;set;} = new List<OrderPosition>();
    }
}