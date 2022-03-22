using Application.Stuff;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StuffController : BaseApiController
    {
        [HttpGet("list/reactSelect")]
        public async Task<IActionResult> ArticleList()
        {
            return HandleResult(await Mediator.Send(new ListRS.Query()));
        }
    }
}