using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodhoundHelper
{
    /// <summary>
    /// Specifies that the property will be provided as additional data in a Bloodhound result.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class BloodhoundDataAttribute : BloodhoundFormatAttribute
    {

        /// <summary>
        /// The name used for the JSON object property the data will be stored in.
        /// </summary>
        public string Name { get; set; }

    }
}
