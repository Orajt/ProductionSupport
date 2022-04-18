using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CalculationController : BaseApiController
    {
        [HttpGet("{orderId}/{articleTypeId}")]
        public async Task<IActionResult> GetArticleList(int orderId, int articleTypeId)
        {
            var result = await Mediator.Send(new Application.Orders.OrderPrintout.Query{OrderId=orderId, ArticleTypeId=articleTypeId});
            if(result == null) return NotFound();
            if(!result.IsSuccess)
                return BadRequest(result.Error);
            var fileStreamResult = new FileStreamResult(new MemoryStream(result.Value.File), "application/pdf");
            fileStreamResult.FileDownloadName=result.Value.FileName;
            Response.Headers.Add("Access-Control-Expose-Headers","Content-Disposition");
            return fileStreamResult;
        }
    }
}