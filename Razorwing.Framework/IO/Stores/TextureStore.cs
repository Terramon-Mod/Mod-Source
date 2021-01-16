using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Razorwing.Framework.IO.Stores;
using Terraria;

namespace Terramon.Razorwing.Framework.IO.Stores
{
    public class Texture2DStore : ResourceStore<Texture2D>
    {
        protected readonly Dictionary<string, Texture2D> TextureCache = new Dictionary<string, Texture2D>();

        public Texture2DStore(IResourceStore<byte[]> store = null)
        {
            Store = store;
        }

        protected IResourceStore<byte[]> Store { get; }


        /// <summary>
        /// Used in cases when we don't want block game thread and don't want texture just here at this moment
        /// We don't have this cases so ignore this method.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new Task<Texture2D> GetAsync(string name)
        {
            return Task.Run(
                () => Task.FromResult(Get(name)) 
            );
        }

        /// <summary>
        /// Retrieves a texture from the store and adds it to the Dictionary.
        /// </summary>
        /// <param name="name">The name of the texture.</param>
        /// <returns>The texture.</returns>
        public new virtual Texture2D Get(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;

            lock (TextureCache)//This locking needed only if we have multi thread access. But left it intact 
            {
                // refresh the texture if no longer available (may have been previously disposed).
                if (!TextureCache.TryGetValue(name, out Texture2D tex) || tex?.IsDisposed == true)
                    using (Stream str = Store.GetStream(name))
                    {
                        if (str == null)
                            return null;
                        TextureCache[name] = tex = Texture2D.FromStream(Main.graphics.GraphicsDevice, str);
                    }

                return tex;
            }
        }
    }
}