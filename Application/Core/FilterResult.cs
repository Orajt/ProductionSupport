namespace Application.Core
{
    public class FilterResult
    {
        public string PropertyName{get;set;}="";
        public int? IntValue{get;set;}=null;
        public DateTime? DateValue{get;set;}=null;
        public string StringValue{get;set;}="";
        public string FilterOption{get;set;}="";
        public bool? BooleanValue{get;set;}=null;
    }
}