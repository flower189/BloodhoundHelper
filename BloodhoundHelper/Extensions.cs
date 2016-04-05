using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodhoundHelper
{
    /// <summary>
    /// Extension methods for Bloodhoud Helper.
    /// </summary>
    public static class Extensions
    {

        /// <summary>
        /// Searches the provided collection for objects with property values containing the query as indicated by the BloodhoundToken attributes.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects to search.</typeparam>
        /// <param name="source">The object collection to search.</param>
        /// <param name="query">The string to be searched for.</param>
        /// <returns>A collection of objects which match the criteria.</returns>
        public static IEnumerable<TSource> BloodhoundSearch<TSource>(this IEnumerable<TSource> source, string query)
        {
            Bloodhound bloodhound = new Bloodhound();
            return bloodhound.Search<TSource>(source, query);
        }

    }
}
