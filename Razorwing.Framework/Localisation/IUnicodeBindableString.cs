using Razorwing.Framework.Bindables;
using Razorwing.Framework.Configuration;

namespace Razorwing.Framework.Localisation
{
    /// <summary>
    /// An <see cref="IBindable{T}"/> which has its value set based on the user's unicode preference.
    /// </summary>
    public interface IUnicodeBindableString : IBindable<string>
    {
        /// <summary>
        /// The text to use if unicode can be displayed. Can be null, in which case <see cref="NonUnicodeText"/> will be used.
        /// </summary>
        string UnicodeText
        {
            set;
        }

        /// <summary>
        /// The text to use if unicode should not be displayed. Can be null, in which case <see cref="UnicodeText"/> will be used.
        /// </summary>
        string NonUnicodeText
        {
            set;
        }
    }
}
