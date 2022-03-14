using Application.ArticleTypes;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ArticleTypeController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> ListRS()
        {
            return HandleResult(await Mediator.Send(new ListReactSelect.Query()));
        }
    }
}