namespace Application.Core
{
    public static class HardcodedClientAppUrls
    {
        public static string ClientAppUrl = @"http://localhost:3000/";
        public static string PdfViewUrl(int id)
        {
            return ClientAppUrl+$@"/files/pdf/{id}";
        }
    }
}