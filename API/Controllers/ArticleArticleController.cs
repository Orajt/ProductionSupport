using Application.ArticleArticle;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ArticleArticleController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> AssignArticle(AssignArticlesToParent.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }
    }
}