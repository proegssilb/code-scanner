using System;
using System.Collections.Generic;
using System.Text;

namespace Proegssilb.CodeScanner.Lib.Interfaces
{
    /// <summary>
    /// IScanner instances find brand new things to index from the outside world.
    /// 
    /// If you generate things to index from existing data, <see cref="IRescanner"/>.
    /// </summary>
    /// <typeparam name="TDiscovered">The type of thing the scanner discovers</typeparam>
    interface IScanner<TDiscovered>
    {
        /// <summary>
        /// Do whatever it is that needs to be done in order to find data to index.
        /// </summary>
        /// <param name="dataSink">A place to write data as it is discovered.</param>
        void Scan(IObserver<TDiscovered> dataSink);
    }
}
