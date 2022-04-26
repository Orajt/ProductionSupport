# ProductionSupport
-------------------------------------------------------------------------------
Basics information: 

Architecture: MVC with MediatR pattern on API

Used tehcnologies: .NET 6.0, React, typescript, EF core, Axios, Identity, MobX, React semantic ui, JWT Token, Docker, Deployed to Azure.

Libraries .NET: Automapper, Linq Dynamic, Swashbuckle, ImageSharp, itext7, QRCoder

Libraries React: Formik, yup, react-select, react-router-dom, react-pdf, react-toastify, react-dropzone

Link to azure: http://mjasiukiewicz.germanywestcentral.azurecontainer.io/
-------------------------------------------------------------------------------
About the app:

ProdictionSupport is being created to support production of upholstered furniture.
App based on articles, and orders that used previously created articles. App provides many funcionalities, and i will discuss some of them on the text below.

Articles have tree structure. This is provided by many-to-many self-reference relations between
articles. Thats because every article could be attached to many parent articles with diffrent
quanity. Every article has article type, that defines required attributes, and relations between other articles. E.g article type: frame set, can't be asigned to accessories, but it can be assigned to finished furniture. App also stores images and pdf files with documentation in wwwroot folder. 

Nice feature is that artcile type: finished furniture has property Fabric Variant Group, which is then used in managing order to allow user to choose many fabrics on furniture, and then calculate fabrics lentgth which is needed to craete sofa. Aditional difficulty is that every fabric is cutting in diffrent ways, in result fabric length is diffrent for various types of fabrics.  

Orders are created based on articles. User can create sets of articles to client. For example if one client ordered furniture that is being made from 4 articles, the app will connect them by setid. It could be seen after creating order and clicking button Order summary.

App could calculate articles(depends on article type), which are needed to create every order, 
then create pdf file and send it to client. Created pdf could be used as an order to other company, for the articles that aren't made in company E.g fabrics or as production lists. If article has pdf file attached, then QRCode with link to documentation is being created and attach to position in created pdf.




