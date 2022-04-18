using Microsoft.AspNetCore.Mvc;
using Application.FabricVariantGroup;

namespace API.Controllers
{
    public class FabricVariantGroupController : BaseApiController
    {
        [HttpGet("list/{id}")]
        public async Task<IActionResult> FVGListRS(int id)
        {
            return HandleResult(await Mediator.Send(new ListReactSelect.Query(){FamillyId=id}));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> FVGDetails(string id)
        {
            return HandleResult(await Mediator.Send(new Details.Query(){Id=id}));
        }
        [HttpGet("details/byArticle/{id}")]
        public async Task<IActionResult> FVGDetailsByArticleId(int id)
        {
            return HandleResult(await Mediator.Send(new DetailsByArticleId.Query(){Id=id}));
        }
        [HttpPost]
        public async Task<IActionResult> Create(Create.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }
    }
}