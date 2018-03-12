using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Proegssilb.CodeScanner.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proegssilb.CodeScanner.CorePlugin.Scanners
{
    public class ProjectRescanner : IRescanner<Project, ClassDeclarationSyntax>
    {
        public Expression<Func<Project, bool>> GetFilter()
        {
            throw new NotImplementedException();
        }

        public IObservable<ClassDeclarationSyntax> Scan(IObservable<Project> dataSource)
        {
            return dataSource.SelectMany(proj => proj.Documents)
                .SelectMany(d => Observable.FromAsync(() => d.GetSyntaxRootAsync()))
                .SelectMany(sr => sr.DescendantNodes().OfType<ClassDeclarationSyntax>());
        }
    }
}
