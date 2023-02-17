using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public struct MouseClickContext
{
    public float Factor { get; set; }
    public bool IsPressed { get; set; }
    public ButtonList Button { get; set; }
    public bool IsOnUI => HoveredUI != null;
    public Node HoveredUI { get; set; }
    public bool IsMouseMotion { get; set; }
}
