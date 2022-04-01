namespace Application.ArticleTypes
{
    public class DetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ReactSelectInt> Stuffs{get;set;}=new List<ReactSelectInt>();

    }
}