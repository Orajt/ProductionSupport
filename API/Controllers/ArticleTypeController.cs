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
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            return HandleResult(await Mediator.Send(new Details.Query(){ArticleTypeId=id}));
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> AssignStuffs(int id, AssignStuffsToArticleType.Command command)
        {
            command.Id=id;
            return HandleResult(await Mediator.Send(command));
        }


    }
}