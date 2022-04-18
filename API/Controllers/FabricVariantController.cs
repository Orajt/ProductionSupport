using Application.FabricVariant;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FabricVariantController : BaseApiController
    {
        [HttpGet("list/reactSelect")]
        public async Task<IActionResult> FVListRS()
        {
            return HandleResult(await Mediator.Send(new ListReactSelect.Query()));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> FVListRS(string id)
        {
            return HandleResult(await Mediator.Send(new Details.Query(){Id=id}));
        }
        [HttpGet]
        public async Task<IActionResult> FVList()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
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