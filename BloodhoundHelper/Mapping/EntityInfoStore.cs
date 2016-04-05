using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BloodhoundHelper.Mapping
{
    public class EntityInfoStore : List<EntityInfo>
    {

        //private static EntityInfoStore _entityInfoStore;

        //private EntityInfoStore() { }

        //public static EntityInfoStore GetInstance()
        //{
        //    if (_entityInfoStore == null)
        //    {
        //        _entityInfoStore = new EntityInfoStore();
        //    }

        //    return _entityInfoStore;
        //}

        public IEnumerable<MapInfo> GetTokenAttributeInfos(Type type)
        {
            EntityInfo info = GetInfo(type);

            if (info == null)
                return new MapInfo[0];

            return info.TokenPropertyInfos;
        }

        public IEnumerable<MapInfo> GetDataMapInfos(Type type)
        {
            EntityInfo info = GetInfo(type);

            if (info == null)
                return new MapInfo[0];

            return info.DataPropertyInfos;
        }

        public MapInfo GetValueAttributeInfo(Type type)
        {
            EntityInfo info = GetInfo(type);

            return info?.ValuePropertyInfos?.FirstOrDefault();
        }

        public EntityInfo GetInfo(Type type)
        {
            EntityInfo info = this.SingleOrDefault(x => x.Type == type);
            if (info == null)
            {
                return MapType(type);
            }
            return info;
        }

        private void ThrowExceptionIfAppliedToCollection(string attributeName, PropertyInfo property, EntityInfo entityInfo)
        {
            if (entityInfo.IsEnumerable)
            {
                string messageFormat = "The {0} attribute does not support collections so cannot be applied to the property {1}.";
                string message = String.Format(messageFormat, attributeName, property.Name);
                throw new NotSupportedException(message);
            }
        }

        /// <summary>
        /// Gets the <c>BloodhoundTokenAttribute</c>'s which have been applied to the specified property.
        /// </summary>
        /// <param name="property">The property to get attributes for.</param>
        /// <returns>An array of <c>BloodhoundTokenAttribute</c>'s.</returns>
        private BloodhoundTokenAttribute[] GetBloodhoundTokenAttributesForProperty(PropertyInfo property)
        {
            var tokenAttributes = (BloodhoundTokenAttribute[])property.GetCustomAttributes(typeof(BloodhoundTokenAttribute));
            return tokenAttributes;
        }

        /// <summary>
        /// Gets the <c>BloodhoundValueAttribute</c>'s which have been applied to the specified property.
        /// </summary>
        /// <param name="property">The property to get attributes for.</param>
        /// <returns>An array of <c>BloodhoundValueAttribute</c>'s.</returns>
        private BloodhoundValueAttribute[] GetBloodhoundValueAttributesForProperty(PropertyInfo property)
        {
            var valueAttributes = (BloodhoundValueAttribute[])property.GetCustomAttributes(typeof(BloodhoundValueAttribute));
            return valueAttributes;
        }

        /// <summary>
        /// Gets the <c>BloodhoundDataAttribute</c>'s which have been applied to the specified property.
        /// </summary>
        /// <param name="property">The property to get attributes for.</param>
        /// <returns>An array of <c>BloodhoundDataAttribute</c>'s.</returns>
        private BloodhoundDataAttribute[] GetBloodhoundDataAttributesForProperty(PropertyInfo property)
        {
            var dataAttributes = (BloodhoundDataAttribute[])property.GetCustomAttributes(typeof(BloodhoundDataAttribute));
            return dataAttributes;
        }

        private EntityInfo MapType(Type type)
        {
            EntityInfo entityInfo = new EntityInfo(type);
            var properties = type.GetProperties();

            foreach (var propertyInfo in properties)
            {
                BloodhoundTokenAttribute[] tokenAttributes = GetBloodhoundTokenAttributesForProperty(propertyInfo);
                BloodhoundDataAttribute[] dataAttributes = GetBloodhoundDataAttributesForProperty(propertyInfo);
                BloodhoundValueAttribute[] valueAttributes = GetBloodhoundValueAttributesForProperty(propertyInfo);

                if (tokenAttributes.Any() || dataAttributes.Any() || valueAttributes.Any())
                {
                    foreach (BloodhoundTokenAttribute attribute in tokenAttributes)
                    {
                        ThrowExceptionIfAppliedToCollection("BloodhoundToken", propertyInfo, entityInfo);
                        entityInfo.TokenPropertyInfos.Add(new MapInfo(propertyInfo, attribute.Format));
                    }

                    foreach (BloodhoundDataAttribute attribute in dataAttributes)
                    {
                        ThrowExceptionIfAppliedToCollection("BloodhoundData", propertyInfo, entityInfo);
                        entityInfo.DataPropertyInfos.Add(new MapInfo(propertyInfo, attribute.Format, attribute.Name));
                    }

                    foreach (BloodhoundValueAttribute attribute in valueAttributes)
                    {
                        ThrowExceptionIfAppliedToCollection("BloodhoundValue", propertyInfo, entityInfo);
                        entityInfo.ValuePropertyInfos.Add(new MapInfo(propertyInfo, attribute.Format));
                    }

                }
            }

            if (entityInfo.DataPropertyInfos.Any() || entityInfo.TokenPropertyInfos.Any() || entityInfo.ValuePropertyInfos.Any())
            {
                this.Add(entityInfo);
                return entityInfo;
            }

            return entityInfo;
        }
    }
}
