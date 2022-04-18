using Application.Article;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FabricController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> CreateArticle(CreateFabric.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditArticle(int id, Edit.Command editCommand)
        {
            editCommand.Id=id;
            return HandleResult(await Mediator.Send(editCommand));
        }
    }
}