using System;
using System.Collections.Generic;
using Razorwing.Framework.IO.Stores;
using System.IO;
using System.Threading.Tasks;

namespace Terramon.Razorwing.Framework.IO.Stores
{
    /// <summary>
    /// This is a middleware resource store what can handle web url and first try to fetch file from drive
    /// If file don't exist it try to load file from provided url and cache it on drive
    /// </summary>
    public class StorageCachableStore : ResourceStore<byte[]>
    {
        /// <summary>
        /// Lead to ModLoader/Mods/Cache/Terramon folder
        /// </summary>
        private readonly Storage storage;

        /// <summary>
        /// If we can't load file both from drive and from web just ignore any calls to this file.
        /// And always return null as result.
        /// Automatically fills from own config file.
        /// This file should be deleted once new mod version releases.
        /// </summary>
        private readonly List<string> NotAvailable = new List<string>();

        /// <summary>
        /// Config file name.
        /// </summary>
        private const string AVAILABLE_FILE_NAME = "NotAvailable";


        public StorageCachableStore(Storage storage, IResourceStore<byte[]> store = null)
        {
            this.storage = storage;
            if (store != null)
                AddStore(store);

            if (storage.Exists(AVAILABLE_FILE_NAME))
            {
                using (var stream = storage.GetStream(AVAILABLE_FILE_NAME))
                {
                    if (stream == null)
                        return;

                    using (var reader = new StreamReader(stream))
                    {
                        string line;
                        //Load each line as single entry
                        while ((line = reader.ReadLine()) != null)
                        {
                            NotAvailable.Add(line);
                        }
                    }
                }
            }
            else
            {
                using (Stream _ = storage.GetStream(AVAILABLE_FILE_NAME, FileAccess.Write, FileMode.CreateNew))
                {
                    //Just make one
                }
            }

        }

        public override byte[] Get(string name)
        {
            //If we don't get failed when fetching resource from web
            if (NotAvailable.Contains(name))
                return null;

            //Get usable path from url
            var path = getStorageString(name);
            //If file present on drive
            if (storage.Exists(path))
            {
                //Just load it
                using (Stream stream = storage.GetStream(path))
                {
                    if (stream == null) return null;

                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
            }

            //Else try get it from other sources (including Web)
            var res = base.Get(name);
            //If resource get loaded
            if (res != null)
            {
                using (Stream stream = storage.Exists(path)
                    ? storage.GetStream(path, FileAccess.Write, FileMode.Truncate)
                    : storage.GetStream(path, FileAccess.Write))
                {
                    //Store a copy on drive
                    stream.Write(res, 0, res.Length);
                }
            }
            else
            {
                //Else add it to NotAvailable config
                NotAvailable.Add(name);
                using (Stream stream = storage.GetStream(AVAILABLE_FILE_NAME, FileAccess.Write, FileMode.Append))
                {
                    if (stream == null)
                        return null;

                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.WriteLine(name);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Async version of <see cref="Get"/> method.
        /// In most cases this method never be needed for us 
        /// </summary>
        /// <param name="name">URL to resource</param>
        /// <returns>Return bytes array</returns>
        public override async Task<byte[]> GetAsync(string name)
        {
            if (NotAvailable.Contains(name))
                return null;

            var path = getStorageString(name);
            if (storage.Exists(path))
            {
                using (Stream stream = storage.GetStream(path))
                {
                    if (stream == null) return null;

                    byte[] buffer = new byte[stream.Length];
                    await stream.ReadAsync(buffer, 0, buffer.Length);
                    return buffer;
                }
            }

            var res = await base.GetAsync(name);
            if (res != null)
            {
                using (Stream stream = storage.Exists(path)
                    ? storage.GetStream(path, FileAccess.Write, FileMode.Truncate)
                    : storage.GetStream(path, FileAccess.Write))
                {
                    await stream.WriteAsync(res, 0, res.Length);
                }
            }
            else
            {
                NotAvailable.Add(name);
                using (Stream stream = storage.GetStream(AVAILABLE_FILE_NAME, FileAccess.Write, FileMode.Append))
                {
                    if (stream == null)
                        return null;

                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        await writer.WriteLineAsync(name);
                    }
                }
            }

            return res;
        }

        public override Stream GetStream(string name)
        {
            if (NotAvailable.Contains(name))
                return null;

            var path = getStorageString(name);

            if (storage.Exists(path))
            {
                return storage.GetStream(path);
            }

            var res = base.GetStream(name);
            if (res != null)
            {
                using (Stream stream = storage.Exists(path)
                    ? storage.GetStream(path, FileAccess.Write, FileMode.Truncate)
                    : storage.GetStream(path, FileAccess.Write))
                {
                    byte[] buffer = new byte[stream.Length];
                    //stream.Read(buffer, 0, buffer.Length);
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
            else
            {
                NotAvailable.Add(name);
                using (Stream stream = storage.GetStream(AVAILABLE_FILE_NAME, FileAccess.Write, FileMode.Append))
                {
                    if (stream == null)
                        return null;

                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.WriteLine(name);
                    }
                }
            }

            return storage.GetStream(path);
        }

        // ReSharper disable once StringIndexOfIsCultureSpecific.1
        private string getStorageString(string url) => url.Substring(url.IndexOf("//") + 2);
    }
}
