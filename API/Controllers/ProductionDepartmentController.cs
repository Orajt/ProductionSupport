using Application.ProductionDepartment;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductionDepartmentController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetProductionDepartments()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }
        [HttpPost]
        public async Task<IActionResult> CreateProductionDepartment(Domain.ProductionDepartment productionDepartment)
        {
            return HandleResult(await Mediator.Send(new Create.Command {ProductionDepartment = productionDepartment}));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditProductionDepartment(int id, Domain.ProductionDepartment productionDepartment)
        {
            productionDepartment.Id=id;
            return HandleResult(await Mediator.Send(new Edit.Command {ProductionDepartment = productionDepartment}));
        }


    }
}