using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class BaseBlueNpc : BaseNpc, IBlueNpcAgent
{
    public override void InteractionWith(INpcAgent agent)
    {
        if (agent is IBlueNpcAgent npc) InteractWithFriendly(npc);
        if (agent is IRedNpcAgent enemy) InteractWithEnemy(enemy);
    }

    public abstract void InteractWithFriendly(IBlueNpcAgent npc);
    public abstract void InteractWithEnemy(IRedNpcAgent npc);
}
