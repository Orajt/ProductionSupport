namespace Application.Core
{
    public static class HardcodedClientAppUrls
    {
        public static string ClientAppUrl = @"http://mjasiukiewicz.germanywestcentral.azurecontainer.io";
        public static string PdfViewUrl(int id)
        {
            return ClientAppUrl+$@"/files/pdf/{id}";
        }
    }
}