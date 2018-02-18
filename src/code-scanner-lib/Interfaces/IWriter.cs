using System;
using System.Collections.Generic;
using System.Text;

namespace Proegssilb.CodeScanner.Lib.Interfaces
{
    /// <summary>
    /// Writers write data in a persistent/indexed form. For example, into a database.
    /// </summary>
    interface IWriter<T>
    {
        /// <summary>
        /// As data arrives, store it.
        /// </summary>
        /// <param name="dataToWrite">An object that can be subscribed to receive data to be written.</param>
        void WriteAll(IObservable<T> dataToWrite);
    }
}
