using Application.Files;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FileController : BaseApiController
    {
        [HttpPost("pdf/{id}")]
        public async Task<IActionResult> ManageFiles(int id, [FromForm] ManagePdfFiles.Command command)
        {
            command.ArticleId=id;
            return HandleResult(await Mediator.Send(command));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPdf(int id)
        {
            var result = await Mediator.Send(new GetFile.Query(){Id=id});
            if(result == null) return NotFound();
            if(!result.IsSuccess)
                return BadRequest(result.Error);
            return result.Value;  
        }
        [HttpGet("list/reactSelect/{type}")]
        public async Task<IActionResult> ArticleList(string type)
        {
            return HandleResult(await Mediator.Send(new ListReactSelect.Query(){Type=type}));
        }
    }
}