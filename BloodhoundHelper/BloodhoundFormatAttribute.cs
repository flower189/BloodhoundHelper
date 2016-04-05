using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodhoundHelper
{

    /// <summary>
    /// A base class for providing a format ability to other attributes.
    /// </summary>
    public abstract class BloodhoundFormatAttribute : BloodhoundBaseAttribute
    {

        /// <summary>
        /// The string format to use for storing the data.
        /// </summary>
        public string Format { get; set; }

    }
}
