using Razorwing.Framework.IO.Stores;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Terramon.Razorwing.Framework.IO.Stores
{
    internal class EmbeddedStore : IResourceStore<byte[]>
    {
        private Dictionary<string,byte[]> files= new Dictionary<string, byte[]>();
        private const string resourceFolder = "Terramon/";


        public void Dispose()
        {
            files = null;
        }

        public byte[] Get(string name)
        {
            if (files.ContainsKey(name))
                return files[name];
            var path = $"{resourceFolder}{name}";
            if (!ModContent.FileExists(path))
            {
                files.Add(name, null);
                return null;
            }
            var data = ModContent.GetFileBytes(path);
            files.Add(name,data);
            return files[name];
        }

        public Task<byte[]> GetAsync(string name)
        {
            return Task.FromResult(Get(name));
        }

        public IEnumerable<string> GetAvailableResources()
        {
            return null;
        }

        public Stream GetStream(string name)
        {
            var data = Get(name);
            if (data == null)
                return null;
            return new MemoryStream(data);
        }

        //protected string GetFilename(string name)
        //{
        //    if (name.StartsWith(resourceFolder))
        //        name = name.Substring(resourceFolder.Length + 1);
        //    return name.Replace('-', '_').Replace('.', '_')
        //            .Replace(Path.DirectorySeparatorChar, '_');
        //}

        //public static byte[] Base64Decode(string base64EncodedData)
        //{
        //    if (base64EncodedData == null) return null;
        //    return System.Convert.FromBase64String(base64EncodedData);
        //    //return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        //}
    }
}
