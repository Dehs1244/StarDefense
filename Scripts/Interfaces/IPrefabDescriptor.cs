using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public interface IResourceDescriptor<T>
    where T : Node
{
    T Create(string name);
}
