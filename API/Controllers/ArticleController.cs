using Application.Article;
using Application.Core;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ArticleController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> ArticleList([FromQuery] PagingParams pagingParams, [FromQuery] List<FilterResult> filters)
        {
            return HandlePagedResult(await Mediator.Send(new List.Query(){PagingParams=pagingParams, Filters=filters}));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ArticleDetails(string id)
        {
            return HandleResult(await Mediator.Send(new Details.Query(){ArticleIdentifier=id}));
        }
        [HttpGet("{id}/{predicate}")]
        public async Task<IActionResult> ReactSelectInt(int id, string predicate)
        {
            return HandleResult(await Mediator.Send(new ListReactSelect.Query(){ArticleTypeId=id, Predicate=predicate}));
        }
        [HttpGet("check/name/{name}")]
        public async Task<IActionResult> CheckArticleName(string name, [FromQuery] string predicate, [FromQuery] int articleTypeId, [FromQuery] int? stuffId)
        {
            return HandleResult(await Mediator.Send(new CheckArticleNameOrGetId.Query(){ArticleName=name, Predicate=predicate, ArticleTypeId=articleTypeId, StuffId=stuffId}));
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