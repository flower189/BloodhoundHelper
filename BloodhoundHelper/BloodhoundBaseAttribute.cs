using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodhoundHelper
{

    /// <summary>
    /// A base attribute class for the project.
    /// </summary>
    public abstract class BloodhoundBaseAttribute : Attribute
    {

        /// <summary>
        /// The TypeId.
        /// </summary>
        public override object TypeId
        {
            get
            {
                return this;
            }
        }

    }
}
