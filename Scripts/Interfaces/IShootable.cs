using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public interface IShootable
{
    void Shoot(Vector3 dir);
    void OnShoot(ShootContext context);
}
