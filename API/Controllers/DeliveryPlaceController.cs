using Application.DeliveryPlace;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DeliveryPlaceController : BaseApiController
    {
        [HttpGet("reactSelect")]
        public async Task<IActionResult> GetReactSelectInt([FromQuery]string predicate)
        {
            return HandleResult(await Mediator.Send(new ListReactSelect.Query(){Predicate=predicate}));
        }
    }
}