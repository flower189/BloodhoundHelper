using BloodhoundHelper.Mapping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BloodhoundHelper
{
    /// <summary>
    /// A helper class to provide support methods for transforming objects for use with Twitter's Typeahead and Bloodhound.
    /// </summary>
    public class Bloodhound : IBloodhound
    {

        private EntityInfoStore _entityInfoStore;

        public Bloodhound(BloodhoundConfiguration configuraton)
        {
            _entityInfoStore = new EntityInfoStore();
            if (configuraton.Maps.Any())
            {
                var entityInfos = configuraton.Maps.Select(x => x.BuildEntityInfo());
                _entityInfoStore.AddRange(entityInfos);
            }

        }

        public Bloodhound() : this(new BloodhoundConfiguration())
        {
        }

        /// <summary>
        /// Returns a dictionary array containing results ready to be serialized into a JSON response.
        /// </summary>
        /// <param name="obj">The object to create results for.</param>
        /// <returns>An <c>IDictionary&lt;string, object&gt;[]</c> containing results with specified key/values.</returns>
        public IDictionary<string, object>[] BuildResults(object obj)
        {
            var results = new List<IDictionary<string, object>>();

            Type type = obj.GetType();
            var info = _entityInfoStore.GetInfo(type);

            if (info.IsEnumerable)
            {
                foreach (var item in (IEnumerable)obj)
                {
                    IDictionary<string, object> result = BuildResult(item);
                    results.Add(result);
                }
            }
            else
            {
                IDictionary<string, object> result = BuildResult(obj);
                results.Add(result);
            }

            return results.ToArray();
        }

        /// <summary>
        /// Creates a dictionary representing a single result.
        /// </summary>
        /// <param name="obj">The object to create a result for.</param>
        /// <returns>An <c>IDictionary&lt;string, object&gt;</c> containing object properties as items.</returns>
        private IDictionary<string, object> BuildResult(object obj)
        {
            Type type = obj.GetType();
            var info = _entityInfoStore.GetInfo(type);

            string bloodhoundValue = info.IsConsideredPrimitive ? obj.ToString() : GetValue(obj);
            string[] bloodhoundTokens = info.IsConsideredPrimitive ? new string[] { obj.ToString() } : GetTokens(obj);
            IDictionary<string, object> bloodhoundData = GetData(obj);

            var result = new Dictionary<string, object>();
            result.Add("value", bloodhoundValue);
            result.Add("tokens", bloodhoundTokens);
            bloodhoundData.ToList().ForEach(x => result.Add(x.Key, x.Value));

            return result;
        }

        /// <summary>
        /// Gets the data for a specified object stored in a Dictionary.
        /// </summary>
        /// <param name="obj">The object to get the data for.</param>
        /// <returns>An <c>IDictionary&lt;string, object&gt;</c> containing the data for the specified object.</returns>
        public IDictionary<string, object> GetData(object obj)
        {
            var data = new Dictionary<string, object>();

            if (obj != null)
            {
                Type type = obj.GetType();

                var mapInfos = _entityInfoStore.GetDataMapInfos(type);

                foreach (var mapInfo in mapInfos)
                {
                    string propertyName = mapInfo.Name;
                    string propertyValue = SafelyGetFormattedPropertyValue(mapInfo.PropertyInfo, mapInfo.Format, obj);
                    data.Add(propertyName, propertyValue);
                }
            }

            return data;
        }

        /// <summary>
        /// Gets the value for an object.
        /// </summary>
        /// <param name="obj">The object to fetch the value for.</param>
        /// <returns>A string representing the value for the object.</returns>
        public string GetValue(object obj)
        {
            if (obj != null)
            {
                Type type = obj.GetType();
                var mapInfo = _entityInfoStore.GetValueAttributeInfo(type);

                if (mapInfo != null)
                {
                    return SafelyGetFormattedPropertyValue(mapInfo.PropertyInfo, mapInfo.Format, obj);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets an array of <c>String</c>'s representing the tokens of the object.
        /// </summary>
        /// <param name="obj">The object to fetch tokens for.</param>
        /// <returns>An array of <c>String</c>'s representing the tokens of the object.</returns>
        public string[] GetTokens(object obj)
        {
            List<String> tokens = new List<String>();

            if (obj != null)
            {
                Type type = obj.GetType();
                var mapInfos = _entityInfoStore.GetTokenAttributeInfos(type);

                foreach (var mapInfo in mapInfos)
                {
                    string value = SafelyGetFormattedPropertyValue(mapInfo.PropertyInfo, mapInfo.Format, obj);
                    if (!String.IsNullOrEmpty(value))
                    {
                        tokens.Add(value);
                    }
                }
            }

            return tokens.ToArray();
        }

        private string SafelyGetFormattedPropertyValue(PropertyInfo property, string format, object obj)
        {
            object propertyValue = property.GetValue(obj);
            if (propertyValue != null)
            {
                if (!String.IsNullOrEmpty(format))
                {
                    string formattedValue = String.Format(format, propertyValue);
                    return formattedValue;
                }
                return propertyValue.ToString();
            }
            return String.Empty;
        }

        /// <summary>
        /// Searches the provided collection for objects with property values containing the query as indicated by the BloodhoundToken attributes.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects to search.</typeparam>
        /// <param name="source">The object collection to search.</param>
        /// <param name="query">The string to be searched for.</param>
        /// <returns>A collection of objects which match the criteria.</returns>
        public IEnumerable<TSource> Search<TSource>(IEnumerable<TSource> source, string query)
        {
            string searchTerm = query.ToLower();
            Type type = typeof(TSource);
            var info = _entityInfoStore.GetInfo(type);

            if (info.IsConsideredPrimitive)
            {
                return source.Where(x => x != null && x.ToString().ToLower().Contains(searchTerm));
            }
            else
            {
                List<TSource> results = new List<TSource>();
                PropertyInfo[] properties = type.GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (info != null)
                    {
                        foreach (var item in source)
                        {
                            object propertyValue = property.GetValue(item);
                            if (propertyValue != null)
                            {
                                string propertyValueString = propertyValue.ToString().ToLower();
                                if (propertyValueString.Contains(searchTerm) && !results.Contains(item))
                                {
                                    results.Add(item);
                                }
                            }
                        }
                    }
                }
                return results;
            }
        }

    }
}
