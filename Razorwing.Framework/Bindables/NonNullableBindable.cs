using System;
using Razorwing.Framework.Configuration;

namespace Razorwing.Framework.Bindables
{
    public class NonNullableBindable<T> : Bindable<T>
        where T : class
    {
        public NonNullableBindable(T defaultValue)
        {
            if (defaultValue == null)
                throw new ArgumentNullException(nameof(defaultValue));

            Value = Default = defaultValue;
        }

        public override T Value
        {
            get => base.Value;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Value), $"Cannot set {nameof(Value)} of a {nameof(NonNullableBindable<T>)} to null.");

                base.Value = value;
            }
        }
    }
}
