using Application.DeliveryPlace;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DeliveryPlaceController : BaseApiController
    {
        [HttpGet("list/reactSelect")]
        public async Task<IActionResult> GetReactSelectInt([FromQuery]string predicate)
        {
            return HandleResult(await Mediator.Send(new ListReactSelect.Query(){Predicate=predicate}));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            return HandleResult(await Mediator.Send(new Details.Query(){Id=id}));
        }
        [HttpGet]
        public async Task<IActionResult> List(int id)
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }
        [HttpPost]
        public async Task<IActionResult> Create(Create.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Create(int id, Edit.Command command)
        {
            command.Id=id;
            return HandleResult(await Mediator.Send(command));
        }

    }
}