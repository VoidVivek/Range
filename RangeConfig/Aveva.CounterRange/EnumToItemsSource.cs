using System;
using System.Linq;
using System.Windows.Markup;

namespace Aveva.CounterRange
{
    /// <summary>
    ///     Class EnumToItemsSource.
    /// </summary>
    /// <seealso cref="System.Windows.Markup.MarkupExtension" />
    public class EnumToItemsSource : MarkupExtension
    {
        /// <summary>
        ///     The type
        /// </summary>
        private readonly Type _type;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnumToItemsSource" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <exception cref="ArgumentException">type</exception>
        public EnumToItemsSource(Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException($"{nameof(type)} must be an enum type");

            _type = type;
        }

        /// <summary>
        ///     When implemented in a derived class, returns an object that is provided as the value of the target property for
        ///     this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(_type)
                .Cast<Enum>()
                .Select(e => new {Value = e, DisplayName = e.Description()});
        }
    }
}