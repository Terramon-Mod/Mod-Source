using System.Collections.Generic;
using System.Globalization;
using Razorwing.Framework.Configuration;
using Razorwing.Framework.IO.Stores;
using Razorwing.Framework.Bindables;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Razorwing.Framework.Localisation
{
    public partial class LocalisationManager
    {
        private readonly List<LocaleMapping> locales = new List<LocaleMapping>();

        private readonly Bindable<bool> preferUnicode;
        private readonly Bindable<string> configLocale;
        private readonly Bindable<IResourceStore<string>> currentStorage = new Bindable<IResourceStore<string>>();

        public LocalisationManager(Bindable<string> locale)
        {
            preferUnicode = new Bindable<bool>(true);
            configLocale = locale;
            configLocale.BindValueChanged(updateLocale);
        }

        public void AddLanguage(string language, IResourceStore<string> storage)
        {
            var loc = locales.Find(l => l.Name == language);
            if (loc != null)
                locales.Remove(loc);
            locales.Add(new LocaleMapping { Name = language, Storage = storage });
            configLocale.TriggerChange();
        }

        /// <summary>
        /// Creates an <see cref="ILocalisedBindableString"/> which automatically updates its text according to information provided in <see cref="ILocalisedBindableString.Text"/>.
        /// </summary>
        /// <returns>The <see cref="ILocalisedBindableString"/>.</returns>
        public ILocalisedBindableString GetLocalisedString(LocalisedString original) => new LocalisedBindableString(original, currentStorage, preferUnicode);

        private void updateLocale(string newVal)
        {
            if (locales.Count == 0)
                return;

            var validLocale = locales.Find(l => l.Name == newVal);

            if (validLocale == null)
            {
                var culture = string.IsNullOrEmpty(newVal) ? CultureInfo.CurrentCulture : new CultureInfo(newVal);

                for (var c = culture; !EqualityComparer<CultureInfo>.Default.Equals(c, CultureInfo.InvariantCulture); c = c.Parent)
                {
                    validLocale = locales.Find(l => l.Name == c.Name);
                    if (validLocale != null)
                        break;
                }

                if(validLocale == null)
                    validLocale = locales[0];
            }

            if (validLocale.Name != newVal)
                configLocale.Value = validLocale.Name;
            else
                currentStorage.Value = validLocale.Storage;
        }

        private class LocaleMapping
        {
            public string Name;
            public IResourceStore<string> Storage;
        }
    }
}
