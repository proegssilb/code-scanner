using MicroResolver;
using Proegssilb.CodeScanner.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;

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
        private int compilingFlag;
        private const int COMPILING_NEW = 0;
        private const int COMPILING_PROCESSING = 1;
        private const int COMPILING_DONE = 2;

        /// <summary>
        /// Retrieves an instance of the IoC container, ready to resolve dependencies.
        /// 
        /// Compiles the container on first usage. Should be thread-safe.
        /// </summary>
        public ObjectResolver Container
        {
            get
            {
                // TODO: Make this more resilient to random problems.
                int wasCompiled = Interlocked.CompareExchange(ref compilingFlag, COMPILING_PROCESSING, COMPILING_NEW);
                if (wasCompiled == COMPILING_NEW && compilingFlag == COMPILING_PROCESSING)
                {
                    try
                    {
                        _container.Compile();
                        compilingFlag = COMPILING_DONE;
                    }
                    catch (Exception)
                    {
                        compilingFlag = COMPILING_NEW;
                    }
                }
                else
                {
                    // TODO: No, this will not deal with thundering herd.
                    while (compilingFlag < COMPILING_DONE) { }
                }
                return _container;
            }
        }

        [ImportMany]
        public IEnumerable<Lazy<IPluginInstaller>> PluginInstallers;

        public PluginManager(ObjectResolver iocContainer=null)
        {
            if (iocContainer == null)
            {
                iocContainer = ObjectResolver.Create();
            }
            _container = iocContainer;
        }

        public IEnumerable<IPluginInstaller> ListPlugins()
        {
            foreach(var pi in PluginInstallers)
            {
                yield return pi.Value;
            }
        }

        public void EnablePlugin(IPluginInstaller plugin)
        {
            // TODO: Still not safe, but it'll have to do for now.
            if (PluginInstallers.Any(ipi => Object.ReferenceEquals(plugin, ipi.Value)))
            {
                plugin.RegisterPluginContents(_container);
            }
        }

        public void DisablePlugin(string plugin)
        {
            throw new NotImplementedException();
        }
    }
}
