using System;
using System.Collections.Generic;

namespace Razorwing.Framework.Configuration.Tracking
{
    public class TrackedSettings : List<ITrackedSetting>
    {
        public event Action<SettingDescription> SettingChanged;

        public void LoadFrom<TLookup>(ConfigManager<TLookup> configManager)
            where TLookup : struct, Enum
        {
            foreach (var value in this)
            {
                value.LoadFrom(configManager);
                value.SettingChanged += d => SettingChanged?.Invoke(d);
            }
        }

        public void Unload()
        {
            foreach (var value in this)
                value.Unload();
        }
    }
}
