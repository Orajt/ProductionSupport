using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context,
            UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {

                // ---------Production Departments---------//   
                var productionDepartments = new List<ProductionDepartment>()
                {
                   new ProductionDepartment{Name="App administration Department"},
                };
                await context.ProductionDepartments.AddRangeAsync(productionDepartments);

                // ---------Article Types---------//   
                var articleTypes = new List<ArticleType>()
                {
                   new ArticleType{Name="Finished furniture"},
                   new ArticleType{Name="Accessories"},
                   new ArticleType{Name="Frame set"},
                   new ArticleType{Name="Frame element"}
                };
                await context.ArticleTypes.AddRangeAsync(articleTypes);

                // ---------Stuffs---------//
                var frameElement = context.ArticleTypes.FirstOrDefault(p => p.Name == "Frame element");
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
                var chipboard15mm=stuffs.FirstOrDefault(p=>p.Name=="Chipboard 15mm");
                var coniferousLumber=stuffs.FirstOrDefault(p=>p.Name=="Coniferous lumber");

                var articles = new List<Article>
                {
                    new Article
                    {
                        ArticleTypeId=finishedFurniture.Id,
                        ArticleType=finishedFurniture,
                        FullName="OCEAN 2AL",
                        NameWithoutFamilly="2AL",
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
                        FullName="OCEAN AL",
                        NameWithoutFamilly="AL",
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
                };
                context.Articles.AddRange(articles);

                


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
