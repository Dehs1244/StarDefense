using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace StarDefense.Utils
{
    public class FPSCounter : Label
    {
        public override void _Process(float delta)
        {
            if (OS.IsDebugBuild()) Text = $"FPS: {Engine.GetFramesPerSecond()}";
            else Text = "";
            base._Process(delta);
        }
    }
}
