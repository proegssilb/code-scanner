using Microsoft.CodeAnalysis;
using Proegssilb.CodeScanner.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;

namespace Proegssilb.CodeScanner.CorePlugin.Scanners
{
    public class SolutionRescanner : IRescanner<Solution, Project>
    {
        public Expression<Func<Solution, bool>> GetFilter()
        {
            return (s) => true;
        }

        public IObservable<Project> Scan(IObservable<Solution> dataSource)
        {
            return dataSource.SelectMany(s => s.Projects);
        }
    }
}
