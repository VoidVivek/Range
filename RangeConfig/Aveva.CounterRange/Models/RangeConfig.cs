using System.Collections.Generic;

namespace Aveva.CounterRange.Models
{
    /// <summary>
    ///     Class RangeConfig.
    /// </summary>
    public class RangeConfig
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }

        /// <summary>
        ///     Gets or sets the class URI.
        /// </summary>
        /// <value>The class URI.</value>
        public string ClassUri { get; set; }

        /// <summary>
        ///     Gets or sets the rules.
        /// </summary>
        /// <value>The rules.</value>
        public IEnumerable<Rule> Rules { get; set; }
    }
}