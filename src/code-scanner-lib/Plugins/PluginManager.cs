using MicroResolver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proegssilb.CodeScanner.Lib.Plugins
{
    /// <summary>
    /// The PluginManager is responsible for locating and loading plugins, including making sure plugin classes
    /// are resolvable from the application's IoC container.
    /// </summary>
    public class PluginManager
    {
        //TODO: Figure out how to test this
        //TODO: MEF code...
        //TODO: Load MEF Components into AppDomain/unregistering. May require switching containers.
        private readonly ObjectResolver _container;
        private bool compiled;

        public ObjectResolver Container
        {
            get
            {
                _container.Compile();
                return _container;
            }
        }

        public PluginManager(ObjectResolver iocContainer=null)
        {
            if (iocContainer == null)
            {
                iocContainer = ObjectResolver.Create();
            }
            _container = iocContainer;
        }

        public IEnumerable<string> ListPlugins()
        {
            throw new NotImplementedException();
        }

        public void EnablePlugin(string plugin)
        {
            throw new NotImplementedException();
        }

        public void DisablePlugin(string plugin)
        {
            throw new NotImplementedException();
        }
    }
}
