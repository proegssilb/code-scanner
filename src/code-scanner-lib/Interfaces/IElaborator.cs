using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Proegssilb.CodeScanner.Lib.Interfaces
{
    /// <summary>
    /// Elaborators are used to add more properties to existing data.
    /// </summary>
    interface IElaborator<T>
    {
        //TODO: Make the return value of this actually work properly.
        /// <summary>
        /// Provides a preliminary filter to reduce amount of traffic to this IElaborator.
        /// </summary>
        /// <returns>An expression/lambda describing which objects this IElaborator is interested in.</returns>
        Expression<bool> GetFilter();

        /// <summary>
        /// Modify incoming data. Treat all data as immutable.
        /// </summary>
        /// <param name="dataSource">An object that can be subscribed to in order to receive discovered data.</param>
        /// <param name="dataSink">An object to publish modified data.</param>
        void Elaborate(IObservable<T> dataSource, IObserver<T> dataSink);
    }
}
