using System.Collections.Generic;

namespace BloodhoundHelper
{
    public interface IBloodhound
    {
        IDictionary<string, object>[] BuildResults(object obj);
        IDictionary<string, object> GetData(object obj);
        string[] GetTokens(object obj);
        string GetValue(object obj);
        IEnumerable<TSource> Search<TSource>(IEnumerable<TSource> source, string query);
    }
}