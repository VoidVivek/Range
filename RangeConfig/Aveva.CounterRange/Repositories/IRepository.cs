using System.Collections.Generic;
using Aveva.CounterRange.Models;

namespace Aveva.CounterRange.Repositories
{
    /// <summary>
    ///     Interface IRepository
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        ///     Gets all range configs.
        /// </summary>
        /// <returns>IEnumerable&lt;RangeConfig&gt;.</returns>
        IEnumerable<RangeConfig> GetRangeConfigs();

        /// <summary>
        ///     Gets the rules for configuration.
        /// </summary>
        /// <param name="rangeConfigId">The range configuration identifier.</param>
        /// <returns>IEnumerable&lt;Rule&gt;.</returns>
        IEnumerable<Rule> GetRulesForConfig(string rangeConfigId);

        /// <summary>
        ///     Saves the specified range configuration.
        /// </summary>
        /// <param name="rangeConfig">The range configuration.</param>
        /// <param name="rules">The rules.</param>
        void Save(RangeConfig rangeConfig, IEnumerable<Rule> rules);
    }
}