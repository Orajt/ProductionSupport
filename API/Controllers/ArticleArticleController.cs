using System.Diagnostics;
using Application.ArticleArticle;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ArticleArticleController : BaseApiController
    {
        [HttpPost("{id}")]
        public async Task<IActionResult> AssignArticles(int id, AssignArticlesToParent.Command command)
        {
            command.ParentId=id;
            return HandleResult(await Mediator.Send(command));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRelations(int id, DeleteRelationBetweenArticles.Command command)
        {
            command.ParentId=id;
            return HandleResult(await Mediator.Send(command));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLowerLevel(int id)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var test = await Mediator.Send(new GetLowerLevelAricles.Query{Id=id});
            watch.Stop();
            Debug.WriteLine($"Execution time:{watch.ElapsedMilliseconds} milliseconds");
            return HandleResult(test);
        }
    }
}