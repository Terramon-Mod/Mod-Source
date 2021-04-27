using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Razorwing.Framework.IO.Stores;
using Terraria.ModLoader;

namespace Terramon.Razorwing.Framework.IO.Stores
{
    public class ModStorage : DesktopStorage
    {
        private string root;

        public ModStorage(string baseName = "") : base(baseName) { }

        protected override string LocateBasePath()
        {
            if (root == null)
            {
                //string[] arr = ModLoader.ModPath.Split('\\');

                root = Path.Combine(ModLoader.ModPath, "Cache");
            }

            return root;
        }

        public override void OpenInNativeExplorer()
        {
            //if (RuntimeInfo.OS == RuntimeInfo.Platform.Windows) Process.Start("explorer.exe", GetFullPath(string.Empty));
        }

        public override Stream GetStream(string path, FileAccess access = FileAccess.Read, FileMode mode = FileMode.OpenOrCreate)
        {
            if (path.StartsWith("http"))
                return null;
            return base.GetStream(path, access, mode);
        }
    }
}