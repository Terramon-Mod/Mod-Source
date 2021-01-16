using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Razorwing.Framework.IO.Stores
{
    public interface IResourceStore<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// Retrieves an object from the store.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <returns>The object.</returns>
        T Get(string name);

        /// <summary>
        /// Retrieves an object from the store asynchronously.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <returns>The object.</returns>
        Task<T> GetAsync(string name);

        Stream GetStream(string name);

        /// <summary>
        /// Gets a collection of string representations of the resources available in this store.
        /// </summary>
        /// <returns>String representations of the resources available.</returns>
        IEnumerable<string> GetAvailableResources();
    }

}
