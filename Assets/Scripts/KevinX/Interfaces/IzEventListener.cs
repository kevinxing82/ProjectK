using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KevinX.Event;

namespace KevinX.Interfaces
{
    public interface IzEventListener
    {
        void ProcessEvent(GameEvent evt);
    }
}
