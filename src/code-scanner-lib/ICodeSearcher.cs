using System;
using System.Collections.Generic;
using System.Text;

namespace Proegssilb.CodeScanner.Lib
{
    public interface ICodeSearcher
    {
        IEnumerable<string> Search(string query);
    }
}
