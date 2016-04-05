using BloodhoundHelper.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodhoundHelper
{
    public class BloodhoundConfiguration
    {

        public IList<BloodhoundMap> Maps { get; set; }

        public BloodhoundConfiguration()
        {
            Maps = new List<BloodhoundMap>();
        }

    }
}
