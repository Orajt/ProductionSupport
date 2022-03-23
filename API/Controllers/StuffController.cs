using Application.Stuff;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StuffController : BaseApiController
    {
        [HttpGet("list/reactSelect")]
        public async Task<IActionResult> ListReactSelect()
        {
            return HandleResult(await Mediator.Send(new ListToSelect.Query()));
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            return HandleResult(await Mediator.Send(new Details.Query() { Id = id }));
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command{Id=id}));
        }

    }
}