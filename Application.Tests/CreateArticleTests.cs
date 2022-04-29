using Application.Interfaces;
using Application.Article;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using System.Collections.Generic;
using System;

namespace Application.Tests;

[TestFixture]
public class CreateArticleTests
{
    private bool _articleWithSameNameAndStuffIdExistsInDb;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IRelations> _relations;
    private Mock<IArticleHelpers> _articleHelpers;
    private Create.Command _command;
    private ArticleTypeComponents _articleTypeComponents;
    private ArticleAdditionalProperties _articleAdditionalProperties;
    private Domain.ArticleType _articleType;

    [SetUp]
    public void Setup()
    {
        _articleAdditionalProperties = new ArticleAdditionalProperties() { Familly = new Domain.Familly(), Stuff = new Domain.Stuff(), FabricVariantGroup = new Domain.FabricVariantGroup() };
        _articleTypeComponents = new ArticleTypeComponents(1,false,false,false);
        _articleType = new Domain.ArticleType() { Id = 1 };
        var date = DateTime.Now;
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork.Setup(uow => uow.Articles.IsArticleNameUnique("a", 1, 1)).ReturnsAsync(false);
        _unitOfWork.Setup<Task<Domain.ArticleType>>(uow => uow.ArticleTypes.Find(1)).ReturnsAsync(_articleType);
        _unitOfWork.Setup<Task<ArticleAdditionalProperties>>(uow => uow.Articles.FindAdditionalProperties(1, 1, 1)).ReturnsAsync(_articleAdditionalProperties);
        _unitOfWork.Setup(uow=> uow.Articles.Add(It.IsAny<Domain.Article>()));
        _unitOfWork.Setup(uow=>uow.SaveChangesAsync()).ReturnsAsync(true);

        _relations = new Mock<IRelations>();
        _relations.Setup(rl => rl.ArticleProperties(1)).Returns(_articleTypeComponents);

        _articleHelpers = new Mock<IArticleHelpers>();
        _articleHelpers.Setup(ah => ah.ArticleProperitiesError(_articleTypeComponents, _articleAdditionalProperties))
        .Returns("");

        _command = new Create.Command()
        {
            FullName = "a",
            NameWithoutFamilly = "a",
            ArticleTypeId = 1,
            ChildArticles = new List<DetailsDtoChildArticles>()
            {
                new DetailsDtoChildArticles
                {
                    ChildId=1,
                    Quanity=1
                },
                new DetailsDtoChildArticles
                {
                    ChildId=2,
                    Quanity=1
                }
            }
        };
    }
    [Test]
    public async Task ArticleCreate_ArticleWithSameNameAndStuffExistInDataBase1_ReturnResultFailure()
    {
        _unitOfWork.Setup(uow => uow.Articles.IsArticleNameUnique("a", 1, 1)).ReturnsAsync(true);
        var handler = new Create.Handler(_unitOfWork.Object, _relations.Object, _articleHelpers.Object);
        var result = await handler.Handle(_command, CancellationToken.None);
        Assert.That(result.Error.Length, Is.GreaterThan(0));
    }
    [Test]
    public async Task ArticleCreate_ArticleTypeIdDoesntExistInDatabase_ReturnResultNull()
    {
        _unitOfWork.Setup(uow => uow.ArticleTypes.Find(1)).ReturnsAsync((Domain.ArticleType)null);
        var handler = new Create.Handler(_unitOfWork.Object, _relations.Object, _articleHelpers.Object);
        var result = await handler.Handle(_command, CancellationToken.None);
        Assert.That(result, Is.Null);
        _unitOfWork.Verify(uow => uow.Articles.FindAdditionalProperties(1, 1, 1), Times.Never);
    }
    [Test]
    public async Task ArticleCreate_ArticleDontHaveRequiredProperties_ReturnResultFailure()
    {
        _articleHelpers.Setup(ah => ah.ArticleProperitiesError(new ArticleTypeComponents(), new ArticleAdditionalProperties()))
        .Returns("Error");
        var handler = new Create.Handler(_unitOfWork.Object, _relations.Object, _articleHelpers.Object);
        var result = await handler.Handle(_command, CancellationToken.None);
        Assert.That(result.Error.Length, Is.GreaterThan(0));
    }
    [Test]
    public async Task ArticleCreate_ArticleHaveRequiredProperties_ArticleAddToContext()
    {
        var handler = new Create.Handler(_unitOfWork.Object, _relations.Object, _articleHelpers.Object);
        var result = await handler.Handle(_command, CancellationToken.None);
        _unitOfWork.Verify(uow => uow.Articles.Add(It.IsAny<Domain.Article>()));
    }
    [Test]
    public async Task ArticleCreate_ArticleCreatedAndChildArticlesAreNull_ReturnsSuccess()
    {
        _command.ChildArticles=null;
        var handler = new Create.Handler(_unitOfWork.Object, _relations.Object, _articleHelpers.Object);
        var result = await handler.Handle(_command, CancellationToken.None);
        Assert.That(result.IsSuccess, Is.True);
    }
    [Test]
    public async Task ArticleCreate_ArticleCreatedAndChildArticlesAreEmptyList_ReturnsSuccess()
    {
        _command.ChildArticles=new List<DetailsDtoChildArticles>();
        var handler = new Create.Handler(_unitOfWork.Object, _relations.Object, _articleHelpers.Object);
        var result = await handler.Handle(_command, CancellationToken.None);
        Assert.That(result.IsSuccess, Is.True);
    }
    [Test]
    public async Task ArticleCreate_ChildsExistsInRequstButNotFoundOnDatabase_ReturnsError()
    {
        _unitOfWork.Setup(uow=>uow.ArticlesArticles.GetComponentsToParentAricle(new List<DetailsDtoChildArticles>(), new Domain.Article())).ReturnsAsync(new List<Domain.ArticleArticle>());
        var handler = new Create.Handler(_unitOfWork.Object, _relations.Object, _articleHelpers.Object);
        var result = await handler.Handle(_command, CancellationToken.None);
        Assert.That(result.Error.Length, Is.GreaterThan(0));
    }

}