using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public class ButtonLoadMenu : ButtonMenu
{
    [Export]
    public PackedScene LoadingMap { get; set; }
}