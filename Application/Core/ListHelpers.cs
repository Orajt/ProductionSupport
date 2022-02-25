namespace Application.Core
{
    public static class ListHelpers
    {
        public static bool IsParametrUnique<T>(List<T> list, string prop1, string prop2="", string prop3="")
        {
            
            if(String.IsNullOrEmpty(prop2))
            {
                var query=list.GroupBy(x => typeof(T).GetProperty(prop1).GetValue(x,null)) 
                    .Where(g => g.Count() > 1)
                    .Select(y => y.Key)
                    .ToList();

                    if(query.Count>0) return false;
            }
                
            else if(String.IsNullOrEmpty(prop3))
            {
                var query=list.GroupBy(x => new {prop1=typeof(T).GetProperty(prop1).GetValue(x,null), prop2=typeof(T).GetProperty(prop2).GetValue(x,null)}) 
                    .Where(g => g.Count() > 1)
                    .Select(y => y.Key)
                    .ToList();

                    if(query.Count>0) return false;
            }
            else
            {
                var query=list.GroupBy(x => new {prop1=typeof(T).GetProperty(prop1).GetValue(x,null),
                                                prop2=typeof(T).GetProperty(prop2).GetValue(x,null),
                                                prop3=typeof(T).GetProperty(prop3).GetValue(x,null)
                }) 
                    .Where(g => g.Count() > 1)
                    .Select(y => y.Key)
                    .ToList();

                    if(query.Count>0) return false;
            }
            return true;

        }
    }
}