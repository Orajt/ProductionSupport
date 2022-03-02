using Application.Calculations;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CalculationController : BaseApiController
    {
        [HttpGet("{orderId}/{articleTypeId}")]
        public async Task<IActionResult> GetArticleList(int orderId, int articleTypeId)
        {
            return HandleResult(await Mediator.Send(new CalculateArticlesByArticleType.Query{OrderId=orderId, ArticleTypeId=articleTypeId}));
        }
    }
}