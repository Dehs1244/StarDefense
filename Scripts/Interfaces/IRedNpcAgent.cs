using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IRedNpcAgent : INpcAgent
{
    void InvokeOnCreateEnemy(IBlueNpcAgent blueAgent);
}
