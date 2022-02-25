using FluentValidation;

namespace Application.ArticleArticle
{
    public class AssignChildValidator : AbstractValidator<AssignArticleToParentDto>
    {
        public AssignChildValidator()
        {
            RuleFor(x => x.ChildId).GreaterThan(0);
            RuleFor(x => x.Quanity).GreaterThan(0);
            RuleFor(x => x.Position).GreaterThan(0);
        }
    }
}