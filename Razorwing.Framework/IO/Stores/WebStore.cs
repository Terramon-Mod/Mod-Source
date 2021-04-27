using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Razorwing.Framework.IO.Stores
{
    /// <summary>
    ///     Temp solution what uses <see cref="WebClient" /> instead of <see cref="HttpClient" />
    ///     It way slower since we can only download one file per time instead of default 3 streams in
    ///     <see cref="HttpClient" />
    /// </summary>
    public class WebStore : IResourceStore<byte[]>
    {
        private readonly string[] accept = new[] { "image/png" };
        private readonly WebClient web = new WebClient();


        public WebStore(string[] accept = null)
        {
            if (accept != null) //image/png
            {
                this.accept = accept;
                foreach (var it in accept)
                {

                }
            }
        }

        public byte[] Get(string name)
        {
            return ((MemoryStream)GetStream(name))?.GetBuffer(); //Just reuse code
        }

        public Task<byte[]> GetAsync(string name)
        {
            return Task.FromResult(Get(name)); //Just reuse code
        }

        public Stream GetStream(string name)
        {
            lock (web)
            {
                try
                {
                    using (Stream str = web.OpenRead(name))
                    {
                        MemoryStream ms = new MemoryStream();
                        //if (web.ResponseHeaders.Get("content-type") != accept) return null;

                        str?.CopyTo(ms);

                        return ms;
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public IEnumerable<string> GetAvailableResources()
        {
            return null;
        }

        #region IDisposable Support

        private bool isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed) isDisposed = true;
        }

        ~WebStore() { Dispose(false); }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}