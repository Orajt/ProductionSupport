namespace Application.Orders
{
    public class OrderSummaryClientDto
    {
        public string Client{get;set;}
        public List<OrderSummaryPositionDto> Positions{get;set;}= new List<OrderSummaryPositionDto>();
    }
}