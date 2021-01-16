using Razorwing.Framework.Configuration;

namespace Razorwing.Framework.Localisation
{
    /// <summary>
    /// An <see cref="IBindable{T}"/> which has its value set depending on the current locale of the <see cref="LocalisationManager"/>.
    /// </summary>
    public interface ILocalisedBindableString : IBindable<string>
    {
        /// <summary>
        /// Sets the original, un-localised text.
        /// </summary>
        LocalisedString Text { set; }

        object[] Args { get; set; }
    }

}
