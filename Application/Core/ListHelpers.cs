using System.Linq.Dynamic;

namespace Application.Core
{
    public interface IListHelpers
    {
        string CreateQueryString(List<FilterResult> filters);
    }

    public class ListHelpers : IListHelpers
    {
        private static Dictionary<string, string> FilterOptionTranslation = new Dictionary<string, string>
        {
            {"GT",">"},
            {"EQ","=="},
            {"LT","<"},
            {"CT","CT"}
        };
        public string CreateQueryString(List<FilterResult> filters)
        {
            var query = "";
            for (int i = 0; i < filters.Count; i++)
            {
                var filter = filters[i];
                bool propertyFound = false;
                if (FilterOptionTranslation.ContainsKey(filter.FilterOption))
                {
                    string option = FilterOptionTranslation.GetValueOrDefault(filter.FilterOption);
                    if (filter.IntValue != null && filter.IntValue != 0)
                    {
                        query += $"{filter.PropertyName}{option}{filter.IntValue}";
                        propertyFound = true;
                    }
                    if (propertyFound == false && filter.DateValue != null)
                    {
                        var dateValue = filter.DateValue.Value;
                        dateValue = DateHelpers.SetDateTimeToCurrent(dateValue);
                        query += $"{filter.PropertyName}{option}DateTime({dateValue.Year},{dateValue.Month},{dateValue.Day})";
                        propertyFound = true;
                    }
                    if (propertyFound == false && !String.IsNullOrEmpty(filter.StringValue))
                    {
                        var value = filter.StringValue.ToLower();
                        query += $"{filter.PropertyName}.ToLower().Contains(\"{value}\")";
                        propertyFound = true;
                    }
                    if (propertyFound == false && filter.BooleanValue != null)
                    {
                        string opt = filter.BooleanValue == true ? "true" : "false";
                        query += $"{filter.PropertyName}=={opt}";
                    }
                }
                if (i < filters.Count - 1)
                    query += " && ";
            }
            return query;
        }
    }
}