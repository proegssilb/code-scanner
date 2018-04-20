using Proegssilb.CodeScanner.Lib.Data;
using Proegssilb.CodeScanner.Lib.Interfaces;
using Proegssilb.CodeScanner.Lib.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Proegssilb.CodeScanner.Lib
{
    public class ScanFlow
    {
        private readonly LibConfig _config;
        private readonly PluginManager _pluginManager;

        public ScanFlow(LibConfig config, PluginManager pluginManager)
        {
            _config = config;
            _pluginManager = pluginManager;
            _foundRescanners = new List<IRescanner>();
        }

        public IReadOnlyCollection<IRescanner> Rescanners => _foundRescanners;
        private List<IRescanner> _foundRescanners;

        public void GenerateFlow()
        {
            var container = _pluginManager.Container;
            //TODO: Initialize storage for observables

            // Build a list of rescanners by input type
            // TODO: Replace the reflection with something more reasonable.
            var rescanners = container.Resolve<IEnumerable<IRescanner>>();
            var lookupDict = rescanners.ToDictionary(r =>
            {
                Type rescannerType = r.GetType();
                var iface = rescannerType.FindInterfaces((t, fc) => t.IsConstructedGenericType && t.GetInterfaces().Contains(typeof(IRescanner)), null).First();
                return iface.GenericTypeArguments[0];
            }, r => 
            {
                Type rescannerType = r.GetType();
                var iface = rescannerType.FindInterfaces((t, fc) => t.IsConstructedGenericType && t.GetInterfaces().Contains(typeof(IRescanner)), null).First();
                return new {Rescanner=r, ReturnType=iface.GenericTypeArguments[1] };
            });
            Type nextType = typeof(Solution);
            while (true)
            {
                if (!lookupDict.ContainsKey(nextType))
                {
                    break;
                }
                var rescanner = lookupDict[nextType];
                _foundRescanners.Add(rescanner.Rescanner);
                nextType = rescanner.ReturnType;
            }
        }
    }
}
