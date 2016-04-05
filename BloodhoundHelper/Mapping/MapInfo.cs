using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BloodhoundHelper.Mapping
{
    public class MapInfo
    {

        public PropertyInfo PropertyInfo { get; set; }
        public string Format { get; set; }
        public string Name { get; set; }

        public MapInfo(PropertyInfo propertyInfo, string format = null, string name = null)
        {
            PropertyInfo = propertyInfo;
            Format = format;
            Name = String.IsNullOrEmpty(name) ? propertyInfo.Name : name;
        }

    }
}
