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
    }
}