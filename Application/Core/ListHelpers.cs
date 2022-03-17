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
        public static IQueryable<T> IntegerFilter<T>(IQueryable<T> collection, string property, string option, int value)
        {
            if (typeof(T).HasProperty(property))
            {
                // if (option == "GT")
                //     // collection = collection.Where($"{property}==\"{value}\'");
                if (option == "EQ")
                    collection = collection.Where(p => (int)typeof(T).GetProperty(property).GetValue(p, null) == value);
                if (option == "LT")
                    collection = collection.Where(p => (int)typeof(T).GetProperty(property).GetValue(p, null) < value);
                if (option == "EQLT")
                    collection = collection.Where(p => (int)typeof(T).GetProperty(property).GetValue(p, null) <= value);
                if (option == "EQGT")
                    collection = collection.Where(p => (int)typeof(T).GetProperty(property).GetValue(p, null) <= value);
            }
            return collection;
        }
        public static IQueryable<T> DateFilter<T>(IQueryable<T> collection, string property, string option, DateTime value)
        {
            if (typeof(T).HasProperty(property))
            {
                if (option == "GT")
                    collection = collection.Where(p => (DateTime)typeof(T).GetProperty(property).GetValue(p, null) > value);
                if (option == "EQ")
                    collection = collection.Where(p => (DateTime)typeof(T).GetProperty(property).GetValue(p, null) == value);
                if (option == "LT")
                    collection = collection.Where(p => (DateTime)typeof(T).GetProperty(property).GetValue(p, null) < value);
                if (option == "EQLT")
                    collection = collection.Where(p => (DateTime)typeof(T).GetProperty(property).GetValue(p, null) <= value);
                if (option == "EQGT")
                    collection = collection.Where(p => (DateTime)typeof(T).GetProperty(property).GetValue(p, null) <= value);

            }

            return collection;
        }
        public static IQueryable<T> StringFilter<T>(IQueryable<T> collection, string property, string option, string value)
        {
            if (typeof(T).HasProperty(property))
            {
                if (option == "CT")
                    collection = collection.Where(p => typeof(T).GetProperty(property).GetValue(p, null).ToString().ToUpper().Contains(value.ToUpper()));
            }
            return collection;
        }
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
                        var year = dateValue.Year;
                        var month = dateValue.Month;
                        var day = dateValue.Day;
                        query+=$"{filter.PropertyName}{option}DateTime({year},{month},{day})";
                        propertyFound=true;
                    }
                    if(propertyFound==false && !String.IsNullOrEmpty(filter.StringValue))
                    {
                        var value=filter.StringValue.ToLower();
                        query+=$"{filter.PropertyName}.ToLower().Contains(\"{value}\")";
                        propertyFound=true;
                    }         
                }
                if(i<filters.Count-1)
                    query+=" && ";
            }
            return query;
        }
    }
}