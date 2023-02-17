using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IBuilder<T>
    where T : IBuilding
{
    T CurrentBuilding { get; set; }
    void Place(T building);
    void PlaceGhost(T building);
}
