using Application.ArticleFabricRealization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ArticleFabricRealizationController : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> ListByArticleId(int id)
        {
            return HandleResult(await Mediator.Send(new ListFabricRealizationById.Query(){ArticleId=id}));
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> List(int id, ManageFabricRealizations.Command command)
        {
            command.ArticleId=id;
            return HandleResult(await Mediator.Send(command));
        }

    }
}