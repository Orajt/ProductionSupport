using Application.Company;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CompanyController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            return HandleResult(await Mediator.Send(new Details.Query(){Id=id}));
        }
        [HttpGet("reactSelect/{predicate}")]
        public async Task<IActionResult> ListReactSelect(string predicate)
        {
            return HandleResult(await Mediator.Send(new ListReactSelect.Query(){Predicate=predicate}));
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