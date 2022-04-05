namespace Application.Stuff
{
    public class ListToSelectDto
    {
        public string Label{get;set;}
        public int Value{get;set;}
        public List<int> ArticleTypesIds{get;set;}=new List<int>();
    }
}