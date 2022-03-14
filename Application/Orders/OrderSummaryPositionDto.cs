namespace Application.Orders
{
    public class OrderSummaryPositionDto
    {
        public string SetName{get;set;}
        public int SetId{get;set;}
        public List<OrderPosition.PositionDto> Positions{get;set;}= new List<OrderPosition.PositionDto>();
    }
}