using Dalamud.Interface.Windowing;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPlugin.Ui.MainWindow
{
    internal class About
    {
        public static void Draw() 
        {
            ImGui.Text("This is the About Tab");
        }
    }
}
