using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IBuilding
{
    int Health { get; set; }
    uint MaxHealth { get; }
    uint Cost { get; }
    bool CanPlace { get; set; }
    bool IsBuildingNow { get; set; }

    int Damage { get; }
    void GetDamage(int damage);
    void Heal(int heal);

    void EnableGhostMode();
    void DisableGhostMode();

    void EnableErrorMode();
    void DisableErrorMode();

    void OnSuccessBuild();

    bool Dead();
}
