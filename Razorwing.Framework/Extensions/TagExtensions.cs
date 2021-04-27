using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terramon.Pokemon;
using Terraria.ModLoader.IO;

namespace Razorwing.Framework.Extensions
{
    public static class TagExtensions
    {
        public static IEnumerable<T> LoadFromTag<T>(this TagCompound tag) where T : ITagLoadable
        {
            var list = new List<T>();
            var j = tag.GetInt("size");
            for (int i = 0; i < j; i++)
            {
                try
                {
                    T ac = (T)Activator.CreateInstance(typeof(T));
                    ac.Load(tag.Get<TagCompound>($"{i}"));
                    list.Add(ac);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }

            return list;
        }

        public static TagCompound SaveToTag<T>(this IEnumerable<T> list) where T : ITagLoadable
        {
            var tag = new TagCompound();
            tag.Add("size", list.Count());
            int i = 0;
            foreach (var it in list)
            {
                tag.Add($"{i}", it.GetCompound());
                i++;
            }
            return tag;
        }
    }

    public interface ITagLoadable
    {
        void Load(TagCompound tag);
        TagCompound GetCompound();
    }
}
