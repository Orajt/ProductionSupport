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
        [HttpPost("{id}")]
        public async Task<IActionResult> ManageImages(int id, [FromForm] ManageImages.Command command)
        {
            command.ArticleId=id;
            return HandleResult(await Mediator.Send(command));
        }
        [HttpGet("{id}/{fileType}")]
        public async Task<IActionResult> GetFile(string id, string fileType)
        {
            var result = await Mediator.Send(new GetFile.Query(){FileIdentifier=id, FileType=fileType});
            if(result == null) return NotFound();
            if(!result.IsSuccess)
                return BadRequest(result.Error);
            Response.Headers.Add("Access-Control-Expose-Headers","Content-Disposition");
            return result.Value;  
        }
        [HttpGet("list/reactSelect/{type}")]
        public async Task<IActionResult> ArticleList(string type)
        {
            return HandleResult(await Mediator.Send(new ListReactSelect.Query(){Type=type}));
        }
    }
}