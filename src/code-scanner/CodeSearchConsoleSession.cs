using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Proegssilb.CodeScanner.Lib;

namespace Proegssilb.CodeScanner
{
    public class CodeSearchConsoleSession : ICodeSearchConsoleSession
    {
        private TextReader _stdin;
        private TextWriter _stdout;
        private ICodeSearcher _codeSearcher;

        public CodeSearchConsoleSession(TextReader stdin, TextWriter stdout, ICodeSearcher searcher)
        {
            _stdin = stdin;
            _stdout = stdout;
            _codeSearcher = searcher;
        }

        public void Run()
        {
            string line;
            while(!String.IsNullOrEmpty(line = _stdin.ReadLine()))
            {
                var results = _codeSearcher.Search(line);
                foreach(var result in results)
                {
                    _stdout.WriteLine(result);
                }
            }
        }
    }
}
