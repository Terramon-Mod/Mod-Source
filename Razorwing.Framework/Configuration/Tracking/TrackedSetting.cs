using System;
using Razorwing.Framework.Bindables;

namespace Razorwing.Framework.Configuration.Tracking
{
    /// <summary>
    /// A singular tracked setting.
    /// </summary>
    /// <typeparam name="TValue">The type of the tracked value.</typeparam>
    public abstract class TrackedSetting<TValue> : ITrackedSetting
    {
        public event Action<SettingDescription> SettingChanged;

        private readonly object setting;
        private readonly Func<TValue, SettingDescription> generateDescription;

        private Bindable<TValue> bindable;

        /// <summary>
        /// Constructs a new <see cref="TrackedSetting{TValue}"/>.
        /// </summary>
        /// <param name="setting">The config setting to be tracked.</param>
        /// <param name="generateDescription">A function that generates the description for the setting, invoked every time the value changes.</param>
        protected TrackedSetting(object setting, Func<TValue, SettingDescription> generateDescription)
        {
            this.setting = setting;
            this.generateDescription = generateDescription;
        }

        public void LoadFrom<TLookup>(ConfigManager<TLookup> configManager)
            where TLookup : struct, Enum
        {
            bindable = configManager.GetBindable<TValue>((TLookup)setting);
            bindable.ValueChanged += displaySetting;
        }

        public void Unload()
        {
            bindable.ValueChanged -= displaySetting;
        }

        private void displaySetting(TValue NewValue) => SettingChanged?.Invoke(generateDescription(NewValue));
    }
}
