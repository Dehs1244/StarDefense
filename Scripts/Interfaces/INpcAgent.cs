using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public interface INpcAgent
{
    int Health { get; set; }

    bool IsActivatedAgent { get; set; }
    void Move(Vector3 direction);
    void LookAtAgent(INpcAgent agent);
    void LookAtPosition(Vector3 position);
    void InteractionWith(INpcAgent agent);
    void EndInteraction(INpcAgent agent);
    Vector3 GetPosition();
    void GetDamage(int damage);

    bool Dead();
}
