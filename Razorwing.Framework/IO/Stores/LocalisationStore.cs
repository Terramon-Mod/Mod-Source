using Razorwing.Framework.IO.Stores;
using System.Collections.Generic;
using System.IO;
using Terraria.Localization;

namespace Terramon.Razorwing.Framework.IO.Stores
{
    public class LocalisationStore : ResourceStore<string>
    {
        protected IResourceStore<byte[]> fileStore;
        protected GameCulture culture;
        protected Dictionary<string, string> dictionary = new Dictionary<string, string>();

        public LocalisationStore(IResourceStore<byte[]> source, GameCulture culture)
        {
            fileStore = source;
            this.culture = culture;
            loadDictionary();
        }


        public override string Get(string name)
        {
            return dictionary.ContainsKey(name) ? dictionary[name] : null;
        }

        protected void loadDictionary()
        {
            using (var stream = fileStore.GetStream($@"Resources/{culture.Name}.lang"))
            {
                if (stream == null)
                    return;

                using (var reader = new StreamReader(stream))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        int equalsIndex = line.IndexOf('=');

                        if (line.Length == 0 || line[0] == '#' || equalsIndex < 0) continue;

                        string key = line.Substring(0, equalsIndex).Trim();
                        string val = line.Remove(0, equalsIndex + 1).Trim();
                        val = val.Replace("\\n", "\n");
                        if (dictionary.ContainsKey(key))
                        {
#if DEBUG
                            throw new FileLoadException($@"Found duplicated entry: {key}");
#endif
                        }
                        else
                        {
                            dictionary.Add(key, val); 
                        }
                        //if (!Enum.TryParse(key, out TLookup lookup))
                        //    continue;

                        //if (ConfigStore.TryGetValue(lookup, out IBindable b))
                        //{
                        //    try
                        //    {
                        //        b.Parse(val);
                        //    }
                        //    catch (Exception e)
                        //    {
                        //    }
                        //}
                        //else if (AddMissingEntries)
                        //    Set(lookup, val);
                    }
                }
            }
        }
    }
}
