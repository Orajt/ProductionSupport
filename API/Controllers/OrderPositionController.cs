using Application.Core;
using Application.OrderPosition;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OrderPositionController : BaseApiController
    {
        [HttpPost("{id}")]
        public async Task<IActionResult> Delete(int id, Delete.Command command)
        {
            command.OrderId=id;
            return HandleResult(await Mediator.Send(command));
        }
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] PagingParams pagingParams, [FromQuery] List<FilterResult> filters)
        {
            return HandlePagedResult(await Mediator.Send(new List.Query(){PagingParams=pagingParams, Filters=filters}));
        }

    }
}