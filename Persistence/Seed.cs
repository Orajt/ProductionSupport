using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context,
            UserManager<AppUser> userManager)
        {
            if (!context.ProductionDepartments.Any())
            {

                // ---------Production Departments---------//   
                var productionDepartments = new List<ProductionDepartment>()
                {
                   new ProductionDepartment{Name="App administration Department"},
                };
                context.ProductionDepartments.AddRange(productionDepartments);

                // ---------Article Types---------//   
                var articleTypes = new List<ArticleType>()
                {
                   new ArticleType{Name="Finished furniture"},
                   new ArticleType{Name="Accessories"},
                   new ArticleType{Name="Frame set"},
                   new ArticleType{Name="Frame element"}
                };
                context.ArticleTypes.AddRange(articleTypes);

                // ---------Stuffs---------//
                var frameElement = articleTypes[3];
                var stuffs = new List<Stuff>()
                {
                    new Stuff{Name="Chipboard 15mm", StuffCode=201, ArticleType=frameElement, ArticleTypeId=frameElement.Id},
                    new Stuff{Name="Coniferous lumber", StuffCode=202, ArticleType=frameElement, ArticleTypeId=frameElement.Id},
                };
                await context.Stuffs.AddRangeAsync(stuffs);

                // ---------Families---------//
                var families = new List<Familly>()
                {
                    new Familly{Name="Ocean"},
                    new Familly{Name="Earth"},
                };
                await context.Famillies.AddRangeAsync(families);

                // ---------Articles---------//
                var finishedFurniture = articleTypes.FirstOrDefault(p => p.Name == "Finished furniture");
                var frameSet = articleTypes.FirstOrDefault(p => p.Name == "Finished furniture");
                var chipboard15mm = stuffs.FirstOrDefault(p => p.Name == "Chipboard 15mm");
                var coniferousLumber = stuffs.FirstOrDefault(p => p.Name == "Coniferous lumber");

                var articles = new List<Article>
                {
                    new Article
                    {
                        ArticleTypeId=finishedFurniture.Id,
                        ArticleType=finishedFurniture,
                        FullName="OCEAN 2ALR",
                        NameWithoutFamilly="2ALR",
                        CreateDate=DateTime.Now,
                        EditDate=DateTime.Now,
                        CreatedInCompany=true,
                        Familly=families.FirstOrDefault(p=>p.Name=="Ocean"),
                        FamillyId=families.FirstOrDefault(p=>p.Name=="Ocean").Id
                    },
                    new Article
                    {
                        ArticleTypeId=finishedFurniture.Id,
                        ArticleType=finishedFurniture,
                        FullName="OCEAN 2",
                        NameWithoutFamilly="2",
                        CreateDate=DateTime.Now,
                        EditDate=DateTime.Now,
                        CreatedInCompany=true,
                        Familly=families.FirstOrDefault(p=>p.Name=="Ocean"),
                        FamillyId=families.FirstOrDefault(p=>p.Name=="Ocean").Id
                    },
                    new Article
                    {
                        ArticleTypeId=finishedFurniture.Id,
                        ArticleType=finishedFurniture,
                        FullName="OCEAN ARM",
                        NameWithoutFamilly="ARM",
                        CreateDate=DateTime.Now,
                        EditDate=DateTime.Now,
                        CreatedInCompany=true,
                        Familly=families.FirstOrDefault(p=>p.Name=="Ocean"),
                        FamillyId=families.FirstOrDefault(p=>p.Name=="Ocean").Id
                    },
                    new Article
                    {
                        ArticleTypeId=frameSet.Id,
                        ArticleType=frameSet,
                        FullName="OCEAN 2 FRAME SET",
                        NameWithoutFamilly="OCEAN 2",
                        CreateDate=DateTime.Now,
                        EditDate=DateTime.Now,
                        CreatedInCompany=true,
                    },
                    new Article
                    {
                        ArticleTypeId=frameSet.Id,
                        ArticleType=frameSet,
                        FullName="OCEAN ARM FRAME SET",
                        NameWithoutFamilly="OCEAN ARM",
                        CreateDate=DateTime.Now,
                        EditDate=DateTime.Now,
                        CreatedInCompany=true,
                    },
                    new Article
                    {
                        ArticleTypeId=frameElement.Id,
                        ArticleType=frameElement,
                        FullName="650x450",
                        CreateDate=DateTime.Now,
                        EditDate=DateTime.Now,
                        CreatedInCompany=false,
                        StuffId=chipboard15mm.Id,
                        Stuff=chipboard15mm,
                        High=15,
                        Length=6500,
                        Width=4500,
                    },
                    new Article
                    {
                        ArticleTypeId=frameElement.Id,
                        ArticleType=frameElement,
                        FullName="200x200",
                        CreateDate=DateTime.Now,
                        EditDate=DateTime.Now,
                        CreatedInCompany=false,
                        StuffId=chipboard15mm.Id,
                        Stuff=chipboard15mm,
                        High=15,
                        Length=2000,
                        Width=2000,
                    },
                    new Article
                    {
                        ArticleTypeId=frameElement.Id,
                        ArticleType=frameElement,
                        FullName="300x300",
                        CreateDate=DateTime.Now,
                        EditDate=DateTime.Now,
                        CreatedInCompany=false,
                        StuffId=chipboard15mm.Id,
                        Stuff=chipboard15mm,
                        High=15,
                        Length=3000,
                        Width=3000,
                    },
                    new Article
                    {
                        ArticleTypeId=frameElement.Id,
                        ArticleType=frameElement,
                        FullName="300x300x35",
                        CreateDate=DateTime.Now,
                        EditDate=DateTime.Now,
                        CreatedInCompany=false,
                        StuffId=coniferousLumber.Id,
                        Stuff=coniferousLumber,
                        High=35,
                        Length=3000,
                        Width=3000,
                    },
                    new Article
                    {
                        ArticleTypeId=frameElement.Id,
                        ArticleType=frameElement,
                        FullName="400x300x35",
                        CreateDate=DateTime.Now,
                        EditDate=DateTime.Now,
                        CreatedInCompany=false,
                        StuffId=coniferousLumber.Id,
                        Stuff=coniferousLumber,
                        High=35,
                        Length=4000,
                        Width=3000,
                    },
                    new Article
                    {
                        ArticleTypeId=frameSet.Id,
                        ArticleType=frameSet,
                        FullName="Frame set inside frame sete",
                        CreateDate=DateTime.Now,
                        EditDate=DateTime.Now,
                        CreatedInCompany=true,
                        StuffId=chipboard15mm.Id,
                        Stuff=chipboard15mm,
                        High=15,
                        Length=2000,
                        Width=2000,
                    },
                };
                foreach (var article in articles)
                {
                    context.Articles.Add(article);
                }


                var articleRelationships = new List<ArticleArticle>
                {
                    new ArticleArticle(articles[0], articles[1],1,1),
                    new ArticleArticle(articles[0], articles[2],2,2),
                    new ArticleArticle(articles[1], articles[3],1,1),
                    new ArticleArticle(articles[2], articles[4],2,1),
                    new ArticleArticle(articles[3], articles[5],2,1),
                    new ArticleArticle(articles[3], articles[6],2,2),
                    new ArticleArticle(articles[3], articles[7],2,3),
                    new ArticleArticle(articles[3], articles[8],2,4),
                    new ArticleArticle(articles[3], articles[9],2,5),
                    new ArticleArticle(articles[4], articles[5],3,1),
                    new ArticleArticle(articles[4], articles[8],3,2),
                    new ArticleArticle(articles[4], articles[10],2,3),
                    new ArticleArticle(articles[10], articles[6],2,1),
                    new ArticleArticle(articles[10], articles[7],1,2),
                };
                context.ArticleArticle.AddRange(articleRelationships);
                context.SaveChanges();

                // ---------Users---------//
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        DisplayName = "Admin",
                        UserName = "admin",
                        Email = "admin@test.com",
                        ProductionDepartment=productionDepartments.FirstOrDefault(p=>p.Name=="App administration Department"),
                        ProductionDepartmentId=productionDepartments.FirstOrDefault(p=>p.Name=="App administration Department").Id
                    }
                };
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }
        }
    }
}
