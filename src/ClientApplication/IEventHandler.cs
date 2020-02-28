using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication
{
    public interface IEventHandler<T>
    {
        void Handle(T @event, IStateManager stateManager);
    }
}