using System;
using Razorwing.Framework.Bindables;

namespace Razorwing.Framework.Configuration.Tracking
{
    /// <summary>
    /// A singular tracked setting.
    /// </summary>
    public interface ITrackedSetting
    {
        /// <summary>
        /// Invoked when this setting has changed.
        /// </summary>
        event Action<SettingDescription> SettingChanged;

        /// <summary>
        /// Loads a <see cref="Bindable{T}"/> into this tracked setting, binding to <see cref="SettingChanged"/>.
        /// </summary>
        /// <param name="configManager">The <see cref="ConfigManager{TLookup}"/> to load from.</param>
        void LoadFrom<TLookup>(ConfigManager<TLookup> configManager)
            where TLookup : struct, Enum;

        /// <summary>
        /// Unloads the <see cref="Bindable{T}"/> from this tracked setting, unbinding from <see cref="SettingChanged"/>.
        /// </summary>
        void Unload();
    }
}
