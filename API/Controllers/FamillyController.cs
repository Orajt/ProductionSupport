using Application.Familly;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FamillyController: BaseApiController
    {
        [HttpGet("list/reactSelect")]
        public async Task<IActionResult> ArticleList()
        {
            return HandleResult(await Mediator.Send(new ListRs.Query()));
        }
    }
}