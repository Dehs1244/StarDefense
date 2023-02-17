using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPlayer
{
    int Health { get; set; }
    int Energy { get; set; }
    int Credits { get; set; }

    void GetDamage(int damage);
    void Heal(int heals);
}
