using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Proegssilb.CodeScanner.Lib.Interfaces
{
    /// <summary>
    /// A mechanism to identify additional information to index based on existing data.
    /// 
    /// If you need to work with the outside world (such as a file system), <see cref="IScanner"/>
    /// </summary>
    /// <typeparam name="TTrigger">The type of data that needs to be scanned.</typeparam>
    /// <typeparam name="TDiscovered">The type of data being discovered as a result of scanning.</typeparam>
    interface IRescanner<TTrigger, TDiscovered>
    {
        //TODO: Make the return value of this actually work properly.
        /// <summary>
        /// Provides a preliminary filter to reduce amount of traffic to this IRescanner.
        /// </summary>
        /// <returns>An expression/lambda describing which objects this IRescanner is interested in.</returns>
        Expression<bool> GetFilter();

        /// <summary>
        /// Generate the next data for indexing.
        /// </summary>
        /// <param name="dataSource">An object that can be subscribed to in order to receive discovered data.</param>
        /// <param name="dataSink">An object to publish newly scanned data.</param>
        void Scan(IObservable<TTrigger> dataSource, IObserver<TDiscovered> dataSink);
    }
}
