using MicroResolver;
using Proegssilb.CodeScanner.Lib.Interfaces;
using System;
using System.Composition;

namespace Proegssilb.CodeScanner.CorePlugin
{
    [Export(typeof(IPluginInstaller))]
    class CorePluginInstaller : IPluginInstaller
    {
        public bool RegisterPluginContents(ObjectResolver iocContainer)
        {
            //TODO: Register classes
            throw new NotImplementedException();
        }
    }
}
