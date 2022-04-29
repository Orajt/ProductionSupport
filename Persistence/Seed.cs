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
                   new ArticleType{Name="Frame element"},
                   new ArticleType{Name="Finished furniture set"},
                   new ArticleType{Name="Fabrics"},

                };
                context.ArticleTypes.AddRange(articleTypes);

                // ---------Stuffs---------//
                var frameElement = articleTypes[3];
                var stuffs = new List<Stuff>()
                {
                    new Stuff{Name="Chipboard 15mm"},
                    new Stuff{Name="Coniferous lumber"},
                    new Stuff{Name="SKLEJKA 15MM"},
                    new Stuff{Name="KARTON TECHNICZNY"},
                    new Stuff{Name="TARCICA LIŚCIASTA (BUK)"},
                    new Stuff{Name="TARCICA IGLASTA (SOSNA)"},
                    new Stuff{Name="F-STANDARD-H:138"},
                    new Stuff{Name="F-VELVET-H:138"},

                };
                await context.Stuffs.AddRangeAsync(stuffs);
                 // ---------ArticleTypesStuffs---------//
                 var ArticleTypesStuffs = new List<ArticleTypeStuff>(){
                     new ArticleTypeStuff{ArticleType=frameElement, ArticleTypeId=frameElement.Id, Stuff=stuffs[0], StuffId=stuffs[0].Id},
                     new ArticleTypeStuff{ArticleType=frameElement, ArticleTypeId=frameElement.Id, Stuff=stuffs[1], StuffId=stuffs[1].Id}
                 };
                 await context.ArticleTypesStuffs.AddRangeAsync(ArticleTypesStuffs);

                var fabricVariants = new List<FabricVariant>(){
                    new FabricVariant{FullName="KORPUS", ShortName="K"},
                    new FabricVariant{FullName="PODUSZKA OPARCIOWA", ShortName="POP"},
                    new FabricVariant{FullName="PODUSZKA SIEDZENIOWA", ShortName="PS"},
                    new FabricVariant{FullName="ZAGLOWEK", ShortName="Z"},
                };
                await context.FabricVariants.AddRangeAsync(fabricVariants);
                var fabricVariantGroups=new List<FabricVariantGroup>()
                {
                    new FabricVariantGroup{Name="K+PO+PS+Z"},
                    new FabricVariantGroup{Name="K+PO+PS"},

                };
                await context.FabricVariantGroups.AddRangeAsync(fabricVariantGroups);
                var fabricVariantsGroupVariants = new List<FabricVariantFabricGroupVariant>()
                {
                    new FabricVariantFabricGroupVariant
                    {
                        FabricVariantId= fabricVariants[0].Id,
                        FabricVariant= fabricVariants[0],
                        FabricVariantGroupId= fabricVariantGroups[0].Id,
                        FabricVariantGroup= fabricVariantGroups[0],
                        PlaceInGroup=1
                    },
                    new FabricVariantFabricGroupVariant
                    {
                        FabricVariantId= fabricVariants[1].Id,
                        FabricVariant= fabricVariants[1],
                        FabricVariantGroupId= fabricVariantGroups[0].Id,
                        FabricVariantGroup= fabricVariantGroups[0],
                        PlaceInGroup=2
                    },
                    new FabricVariantFabricGroupVariant
                    {
                        FabricVariantId= fabricVariants[2].Id,
                        FabricVariant= fabricVariants[2],
                        FabricVariantGroupId= fabricVariantGroups[0].Id,
                        FabricVariantGroup= fabricVariantGroups[0],
                        PlaceInGroup=3
                    },
                     new FabricVariantFabricGroupVariant
                    {
                        FabricVariantId= fabricVariants[3].Id,
                        FabricVariant= fabricVariants[3],
                        FabricVariantGroupId= fabricVariantGroups[0].Id,
                        FabricVariantGroup= fabricVariantGroups[0],
                        PlaceInGroup=4
                    },
                    //----------------//
                    new FabricVariantFabricGroupVariant
                    {
                        FabricVariantId= fabricVariants[0].Id,
                        FabricVariant= fabricVariants[0],
                        FabricVariantGroupId= fabricVariantGroups[1].Id,
                        FabricVariantGroup= fabricVariantGroups[1],
                        PlaceInGroup=1
                    },
                    new FabricVariantFabricGroupVariant
                    {
                        FabricVariantId= fabricVariants[1].Id,
                        FabricVariant= fabricVariants[1],
                        FabricVariantGroupId= fabricVariantGroups[1].Id,
                        FabricVariantGroup= fabricVariantGroups[1],
                        PlaceInGroup=2
                    },
                    new FabricVariantFabricGroupVariant
                    {
                        FabricVariantId= fabricVariants[2].Id,
                        FabricVariant= fabricVariants[2],
                        FabricVariantGroupId= fabricVariantGroups[1].Id,
                        FabricVariantGroup= fabricVariantGroups[1],
                        PlaceInGroup=3
                    }
                };
                await context.FabricVariantsGroupVariants.AddRangeAsync(fabricVariantsGroupVariants);

                // ---------Families---------//
                var families = new List<Familly>()
                {
                    new Familly{Name="Ocean"},
                    new Familly{Name="Earth"},
                    new Familly{Name="Sissy"},
                    new Familly{Name="Kaya"},
                };
                await context.Famillies.AddRangeAsync(families);

                var companies = new List<Company>()
                {
                    new Company{
                        Name="Main dealer",
                        CompanyIdentifier="123456789",
                        Merchant=true,
                        Supplier=true
                    }
                };
                context.Companies.AddRange(companies);

                var deliveryPlaces = new List<DeliveryPlace>(){
                    new DeliveryPlace{
                        Name="Main dealer's main depot",
                        Country="Random",
                        City="Random city",
                        Street="Random street",
                        PostalCode="11-040",
                        NumberOfBuilding=8,
                        CompanyID=companies[0].Id,
                        Company=companies[0]
                    },
                    new DeliveryPlace{
                        Name="Main dealer's second depot",
                        Country="Random",
                        City="Random city",
                        Street="Random street",
                        PostalCode="11-040",
                        NumberOfBuilding=8,
                        CompanyID=companies[0].Id,
                        Company=companies[0]
                    }

                };
                context.DeliveryPlaces.AddRange(deliveryPlaces);

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
                context.SaveChanges();
            }
        }
    }
}