using System.Linq.Dynamic;

namespace Application.Core
{
    public static class ListHelpers
    {
        public static bool IsParametrUnique<T>(List<T> list, string prop1, string prop2 = "", string prop3 = "")
        {

            if (String.IsNullOrEmpty(prop2))
            {
                var query = list.GroupBy(x => typeof(T).GetProperty(prop1).GetValue(x, null))
                    .Where(g => g.Count() > 1)
                    .Select(y => y.Key)
                    .ToList();

                if (query.Count > 0) return false;
            }

            else if (String.IsNullOrEmpty(prop3))
            {
                var query = list.GroupBy(x => new { prop1 = typeof(T).GetProperty(prop1).GetValue(x, null), prop2 = typeof(T).GetProperty(prop2).GetValue(x, null) })
                    .Where(g => g.Count() > 1)
                    .Select(y => y.Key)
                    .ToList();

                if (query.Count > 0) return false;
            }
            else
            {
                var query = list.GroupBy(x => new
                {
                    prop1 = typeof(T).GetProperty(prop1).GetValue(x, null),
                    prop2 = typeof(T).GetProperty(prop2).GetValue(x, null),
                    prop3 = typeof(T).GetProperty(prop3).GetValue(x, null)
                })
                    .Where(g => g.Count() > 1)
                    .Select(y => y.Key)
                    .ToList();

                if (query.Count > 0) return false;
            }
            return true;

        }
        public static Dictionary<string,string> FilterOptionTranslation = new Dictionary<string, string>
        {
            {"GT",">"},
            {"EQ","=="},
            {"LT","<="},
            {"CT","CT"}
        };
        public static string CreateQueryString(List<FilterResult> filters)
        {
            var query="";
            for(int i=0;i<filters.Count;i++)
            {
                var filter=filters[i];
                bool propertyFound=false;
                if(FilterOptionTranslation.ContainsKey(filter.FilterOption))
                {
                    string option =FilterOptionTranslation.GetValueOrDefault(filter.FilterOption); 
                    if(filter.IntValue!=null && filter.IntValue!=0)
                    {
                        query+=$"{filter.PropertyName}{option}{filter.IntValue}";
                        propertyFound=true;
                    }
                    if(propertyFound==false && filter.DateValue!=null)
                    {
                        var dateValue=filter.DateValue.Value;
                        dateValue=DateHelpers.SetDateTimeToCurrent(dateValue);
                        var year = dateValue.Year;
                        var month = dateValue.Month;
                        var day = dateValue.Day;
                        int hour=0;
                        int minute=0;
                        int sec=0;
                        if(option==">")
                        {
                            hour = 23;
                            minute=59;
                            sec=59;
                        }
                        query+=$"{filter.PropertyName}{option}DateTime({year},{month},{day},{hour},{minute},{sec})";
                        
                        propertyFound=true;
                    }
                    if(propertyFound==false && !String.IsNullOrEmpty(filter.StringValue))
                    {
                        var value=filter.StringValue.ToLower();
                        query+=$"{filter.PropertyName}.ToLower().Contains(\"{value}\")";
                        propertyFound=true;
                    }
                    if(propertyFound==false && filter.BooleanValue!=null){
                        string opt=filter.BooleanValue==true ? "true" : "false";
                        query+=$"{filter.PropertyName}=={opt}";
                    }  
                }
                if(i<filters.Count-1)
                    query+=" && ";
            }
            return query;
        }
    }
}