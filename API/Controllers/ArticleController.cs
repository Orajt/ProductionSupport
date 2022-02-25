using Application.Article;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ArticleController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetArticleList()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }
        [HttpPost]
        public async Task<IActionResult> CreateArticle(Create.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditArticle(int id, Edit.Command editCommand)
        {
            editCommand.Id=id;
            return HandleResult(await Mediator.Send(editCommand));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command{Id=id}));
        }
    }
}