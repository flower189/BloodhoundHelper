using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodhoundHelper
{
    /// <summary>
    /// Specifies that the property will be used as the value for a Bloodhound result.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class BloodhoundValueAttribute : BloodhoundFormatAttribute
    {

    }
}
