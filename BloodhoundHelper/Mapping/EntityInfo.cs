using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BloodhoundHelper.Mapping
{
    public class EntityInfo
    {

        public Type Type { get; }
        public bool IsConsideredPrimitive { get; }
        public bool IsEnumerable { get; set; }
        public IList<MapInfo> TokenPropertyInfos { get; set; }
        public IList<MapInfo> DataPropertyInfos { get; set; }
        public IList<MapInfo> ValuePropertyInfos { get; set; }


        public EntityInfo(Type type)
        {
            Type = type;
            IsConsideredPrimitive = GetIsConsideredPrimitive(Type);
            IsEnumerable = GetIsEnumerable(Type);

            TokenPropertyInfos = new List<MapInfo>();
            DataPropertyInfos = new List<MapInfo>();
            ValuePropertyInfos = new List<MapInfo>();
        }

        /// <summary>
        /// Determines if an object is of a type which is considered primitive.
        /// </summary>
        /// <param name="type">The type to test.</param>
        /// <returns>A <c>bool</c> indicating if the type is primitive or not.</returns>
        private bool GetIsConsideredPrimitive(Type type)
        {
            bool isPrimitive = type.IsValueType || type == typeof(String);
            return isPrimitive;
        }

        /// <summary>
        /// Determines if an object is of a type is enumerable.
        /// </summary>
        /// <param name="type">The type to test.</param>
        /// <returns>A <c>bool</c> indicating if the type is enumerable.</returns>
        private bool GetIsEnumerable(Type type)
        {
            if (type == typeof(String))
                return false;

            if (typeof(IEnumerable).IsAssignableFrom(type))
                return true;

            return false;

        }

    }
}
