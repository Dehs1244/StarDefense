using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarDefense.Attrubutes
{
    public class BuildingAttribute : ResourceAttribute
    {
        public string Extension { get; set; }
        public string BuildingName { get; set; }
        public uint Cost { get; set; }
        public BuildingAttribute(string buildingResource, string buildingName, uint cost, string extension = "png") : base(buildingResource)
        {
            BuildingName = buildingName;
            Extension = extension;
            Cost = cost;
        }
    }
}
