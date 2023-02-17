using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarDefense.Attrubutes
{
    public class ResourceAttribute : Attribute
    {
        public string Name { get; }

        public ResourceAttribute(string name)
        {
            Name = name;
        }
    }
}
