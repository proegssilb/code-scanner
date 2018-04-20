using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proegssilb.CodeScanner.Lib.Data
{
    public class LibConfig
    {
        private readonly IReadOnlyCollection<Workspace> _scanLocations;
        public IReadOnlyCollection<Workspace> ScanLocations => _scanLocations;

        private readonly string _dbConnectionString;
        public string DbConnectionString => _dbConnectionString;

        public LibConfig(IReadOnlyCollection<Workspace> scanLocations, string dbConnectionString)
        {
            _scanLocations = scanLocations;
            _dbConnectionString = dbConnectionString;
        }

    }
}
