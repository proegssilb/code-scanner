using MicroResolver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proegssilb.CodeScanner.Lib.Interfaces
{
    public interface IPluginInstaller
    {
        bool RegisterPluginContents(ObjectResolver iocContainer);
    }
}
