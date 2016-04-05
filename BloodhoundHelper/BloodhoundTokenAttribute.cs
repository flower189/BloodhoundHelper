using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodhoundHelper
{
    /// <summary>
    /// Specifies that the property will be used as a token in Bloodhound.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class BloodhoundTokenAttribute : BloodhoundFormatAttribute
    {

    }
}
