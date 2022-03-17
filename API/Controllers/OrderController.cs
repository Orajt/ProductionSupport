using Application.Core;
using Application.Orders;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OrderController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] PagingParams pagingParams, [FromQuery] List<FilterResult> filters)
        {
            return HandlePagedResult(await Mediator.Send(new List.Query(){PagingParams=pagingParams, Filters=filters}));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            return HandleResult(await Mediator.Send(new Details.Query{Id=id}));
        }
        [HttpGet("{name}/{predicate}")]
        public async Task<IActionResult> CheckName(string name, string predicate)
        {
            return HandleResult(await Mediator.Send(new CheckNameOrGetId.Query{Name=name, Predicate=predicate}));
        }
        [HttpGet("summary/order/{predicate}")]
        public async Task<IActionResult> GetOrderSummary(string predicate)
        {
            return HandleResult(await Mediator.Send(new OrderSummary.Query{Predicate=predicate}));
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