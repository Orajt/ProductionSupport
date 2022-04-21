using Application.Core;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CalculationController : BaseApiController
    {
        [HttpGet("{orderId}/{articleTypeId}")]
        public async Task<IActionResult> GetArticleList(int orderId, int articleTypeId)
        {
            Result<Application.Core.MyFileResult> result = null;
            if(articleTypeId!=6)
            {
                result = await Mediator.Send(new Application.Orders.OrderPrintout.Query{OrderId=orderId, ArticleTypeId=articleTypeId});
            }
            if(articleTypeId==6)
            {
                 result = await Mediator.Send(new Application.Calculation.CalculateFabrics.Query{OrderId=orderId});
            }
            if(result == null) return NotFound();
            if(!result.IsSuccess)
                return BadRequest(result.Error);
            var fileStreamResult = new FileStreamResult(new MemoryStream(result.Value.File), "application/pdf");
            fileStreamResult.FileDownloadName=result.Value.FileName;
            Response.Headers.Add("Access-Control-Expose-Headers","Content-Disposition");
            return fileStreamResult;
        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> CalculateFabrics(int orderId)
        {
            var result = await Mediator.Send(new Application.Calculation.CalculateFabrics.Query{OrderId=orderId});
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