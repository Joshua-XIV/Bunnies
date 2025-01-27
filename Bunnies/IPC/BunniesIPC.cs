using ECommons.EzIpcManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPlugin.IPC;

internal class BunniesIPC
{
    internal BunniesIPC()
    {
        EzIPC.Init(this);
    }
}
