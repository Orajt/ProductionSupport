# ProductionSupport
-------------------------------------------------------------------------------
Basics information: 

Architecture: MVC with MediatR pattern on API

Used tehcnologies: .NET 6.0, EF core, NUnit, React, typescript, Axios, Identity, MobX, React semantic ui, JWT Token, Docker, Deployed to Azure.

Libraries .NET: Automapper, Linq Dynamic, Swashbuckle, ImageSharp, itext7, QRCoder, 

Libraries React: Formik, yup, react-select, react-router-dom, react-pdf, react-toastify, react-dropzone

Link to azure: http://mjasiukiewicz.germanywestcentral.azurecontainer.io/t

login: admin@test.com
pw: Pa$$w0rd

Securityheader raport:
![image](https://user-images.githubusercontent.com/35630427/165917211-dd7d1dc8-e81f-4306-a041-40eb3afe2334.png)
-------------------------------------------------------------------------------
About the app:

ProdictionSupport is being created to support production of upholstered furniture. The app is divided to 6 projects: API, Application, Persistance, Domain, Application.Tests, and client app. 

Database schema: Arrow is on many side :)
![image](https://user-images.githubusercontent.com/35630427/166073761-8317c2ca-fbe4-4093-9f66-db9ee674a081.png)


App based on articles, and orders that used previously created articles. App provides many funcionalities, and i will discuss some of them on the text below.

Articles have tree structure. This is provided by many-to-many self-reference relations between
articles. I used this kind of relationship because every article could be attached to many parent articles with diffrent
quanity. Every article has article type, that defines required attributes, and relations between other articles. E.g article type: frame set, can't be asigned to accessories, but it can be assigned to finished furniture. Those relations are stored in Application/Core/Relations.cs. App stores images and pdf files with documentation in wwwroot folder. Every article could have one pdf file and many images. 

Article finished furniute has additional property FabricVarianGroup, which is used to calculate fabrics based on previously defined variants. E.g To furniture like the one below, user could choose different fabrics to every part of furniture that is signed by letter. So this article will have fabric variant group created from 4 variants: K+OP+S+Z. Every fabric could be cutted in diffrent way, so fabric length that is necessary to create furniture is also depends on fabric type.  
![image](https://user-images.githubusercontent.com/35630427/166059824-0ad64736-e8ce-4c3b-b81d-3da2a830f861.png)

To handle that situation in order form /orders/form, we can choose fabrics desired by client, by type different integers in inputs that is marked by variant names: 

![image](https://user-images.githubusercontent.com/35630427/166062415-b10d50e1-56de-4a22-ac17-c55846932904.png)

Then, when user complete form and click button save chaanges, he could calculate fabrics (or differen type of articles) to previously created order.
![image](https://user-images.githubusercontent.com/35630427/166063210-5a15b05c-92cd-4568-ad14-79b1ffd0dbec.png)

Then application creates PDF file. On the first page, users gain informations about lacks of fabric realizations to choosen articles, that he has to add if he want calculate fabrics properly. The user can add those informations by finding article on article list, then click details button and then click manage realizations.

![image](https://user-images.githubusercontent.com/35630427/166063565-513104a6-78b3-4043-8a9f-84d590dbd6be.png)

Manage realizations form that realized second row in the table:

![image](https://user-images.githubusercontent.com/35630427/166064323-ac486d3f-5051-4ef1-a8f6-a3e07481ea91.png)

On the second page, calculated fabrics are listed in table with company names that sells those fabrics. On the last pages there are position that needs fabrics to be created. 

Application is much more complex, i could talk about that in a phone call or something. 





