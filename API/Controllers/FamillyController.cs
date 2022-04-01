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
        [HttpPost]
        public async Task<IActionResult> Create(Create.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, Edit.Command command)
        {
            command.Id=id;
            return HandleResult(await Mediator.Send(command));
        }

    }
}