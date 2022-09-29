namespace Terramon.Extensions.ValidationExtensions
{
    public struct Validator<T> where T : class
    {
        public Validator(T item)
        {
            Value = item;
            Valid = true;
        }
        public T Value { get; private set; }
        public bool Valid { get; private set; }

        public Validator<T> Invalidate()
        {
            Valid = false;
            return this;
        }
    }

    public static class ValidatorExtensions
    {
        public static Validator<T> CreateValidator<T>(this T item) where T : class => new Validator<T>(item);
        public static Validator<T> Validate<T>(this T item) where T : class => CreateValidator(item);

        public static bool Result<T>(this Validator<T> validator) where T : class => validator.Valid;

        public static bool IsNull<T>(this T item) where T : class => item == null;
    }

}
