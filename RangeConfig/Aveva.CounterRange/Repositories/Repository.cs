using System.Collections.Generic;
using System.Linq;
using Aveva.Core.Database;
using Aveva.Core.Database.Filters;
using Aveva.CounterRange.Enums;
using Aveva.CounterRange.Models;

namespace Aveva.CounterRange.Repositories
{
    /// <summary>
    ///     Class Repository.
    /// </summary>
    /// <seealso cref="Aveva.CounterRange.Repositories.IRepository" />
    public class Repository : IRepository
    {
        /// <summary>
        ///     The and or att
        /// </summary>
        private readonly DbAttribute andOrAtt = DbAttribute.GetDbAttribute(":ConditionComposer");

        /// <summary>
        ///     The attribute and class elem type
        /// </summary>
        private readonly DbElementType attributeAndClassElemType = DbElementType.GetElementType(":AttributeAndClass");

        /// <summary>
        ///     The attribute att
        /// </summary>
        private readonly DbAttribute attributeAtt = DbAttribute.GetDbAttribute(":AttClassRef");

        /// <summary>
        ///     The attribute URI att
        /// </summary>
        private readonly DbAttribute attributeUriAtt = DbAttribute.GetDbAttribute(":AttributeURI");

        /// <summary>
        ///     The class URI att
        /// </summary>
        private readonly DbAttribute classUriAtt = DbAttribute.GetDbAttribute(":ClassURI");

        /// <summary>
        ///     The comparison operator att
        /// </summary>
        private readonly DbAttribute comparisonOperatorAtt = DbAttribute.GetDbAttribute(":ComparisonOperator");

        /// <summary>
        ///     The configuration name att
        /// </summary>
        private readonly DbAttribute configNameAtt = DbAttribute.GetDbAttribute(":ConfigName");

        /// <summary>
        ///     The counter range configuration elem type
        /// </summary>
        private readonly DbElementType counterRangeConfigElemType = DbElementType.GetElementType(":CounterRangeConfig");

        /// <summary>
        ///     The grouped att
        /// </summary>
        private readonly DbAttribute groupedAtt = DbAttribute.GetDbAttribute(":Grouped");

        /// <summary>
        ///     The maximum att
        /// </summary>
        private readonly DbAttribute maxAtt = DbAttribute.GetDbAttribute(":MaximumVal");

        /// <summary>
        ///     The minimum att
        /// </summary>
        private readonly DbAttribute minAtt = DbAttribute.GetDbAttribute(":MinimumVal");

        /// <summary>
        ///     The range rule elem type
        /// </summary>
        private readonly DbElementType rangeRuleElemType = DbElementType.GetElementType(":RangeRule");

        /// <summary>
        ///     The rule condition elem type
        /// </summary>
        private readonly DbElementType ruleConditionElemType = DbElementType.GetElementType(":RuleCondition");

        /// <summary>
        ///     The status in group att
        /// </summary>
        private readonly DbAttribute statusInGroupAtt = DbAttribute.GetDbAttribute(":StatusInGroup");

        /// <summary>
        ///     The value att
        /// </summary>
        private readonly DbAttribute valueAtt = DbAttribute.GetDbAttribute(":Value");

        /// <summary>
        ///     Gets the range configs.
        /// </summary>
        /// <returns>IEnumerable&lt;RangeConfig&gt;.</returns>
        public IEnumerable<RangeConfig> GetRangeConfigs()
        {
            var retVal = new List<RangeConfig>();

            var engineeringWorld = MDB.CurrentMDB.GetFirstWorld(DbType.Engineering);
            var actualTypeFilter = new ActualTypeFilter(counterRangeConfigElemType);
            var col = new DBElementCollection(engineeringWorld, actualTypeFilter);

            foreach (DbElement dbElement in col)
                if (dbElement.IsValid)
                {
                    var displayName = string.Empty;
                    var classUri = string.Empty;
                    var config = new RangeConfig();
                    config.Id = GetRefAsId(dbElement);
                    if (dbElement.GetValidString(configNameAtt, ref displayName)) config.DisplayName = displayName;
                    if (dbElement.GetValidString(classUriAtt, ref classUri)) config.ClassUri = classUri;
                    retVal.Add(config);
                }

            return retVal;
        }

        /// <summary>
        ///     Gets the rules for configuration.
        /// </summary>
        /// <param name="rangeConfigId">The range configuration identifier.</param>
        /// <returns>IEnumerable&lt;Rule&gt;.</returns>
        public IEnumerable<Rule> GetRulesForConfig(string rangeConfigId)
        {
            var retVal = new List<Rule>();
            var rangeConfigElement = DbElement.GetElement(rangeConfigId);

            if (rangeConfigElement.IsValid)
            {
                var rangeRuleElements = rangeConfigElement.Members(rangeRuleElemType);
                var i = 0;

                foreach (var rangeRuleElement in rangeRuleElements)
                    if (rangeRuleElement.GetActualType() == rangeRuleElemType) // due to corrupt db
                    {
                        var min = 0;
                        var max = 0;
                        var rule = new Rule();
                        rule.Id = GetRefAsId(rangeRuleElement);
                        rule.Sequence = i;
                        if (rangeRuleElement.GetValidInteger(minAtt, ref min)) rule.MinValue = min;
                        if (rangeRuleElement.GetValidInteger(maxAtt, ref max)) rule.MaxValue = max;
                        rule.Conditions = GetRuleConditions(rangeRuleElement);

                        retVal.Add(rule);
                        i++;
                    }
            }

            return retVal;
        }

        /// <summary>
        ///     Saves the specified range configuration.
        /// </summary>
        /// <param name="rangeConfig">The range configuration.</param>
        /// <param name="rules">The rules.</param>
        public void Save(RangeConfig rangeConfig, IEnumerable<Rule> rules)
        {
            var rangeConfigElement = GetRangeConfigElement(rangeConfig);

            if (rangeConfigElement.IsValid)
            {
                SetStringValue(rangeConfig.DisplayName, configNameAtt, rangeConfigElement);
                SetStringValue(rangeConfig.ClassUri, classUriAtt, rangeConfigElement);

                var attClassDict = SaveAttClassElements(rules, rangeConfigElement);
                var rulesSorted = rules.OrderBy(r => r.Sequence);

                foreach (var rule in rulesSorted)
                {
                    var rangeRuleElement = GetRangeRuleElement(rule, rangeConfigElement);
                    rangeRuleElement.SetAttribute(minAtt, rule.MinValue);
                    rangeRuleElement.SetAttribute(maxAtt, rule.MaxValue);

                    var conditionsSorted = rule.Conditions.OrderBy(c => c.Sequence);

                    foreach (var condition in rule.Conditions)
                    {
                        var conditionElement = GetConditionElement(condition, rangeRuleElement);
                        conditionElement.SetAttribute(andOrAtt, (int) condition.AndOr);
                        if (!string.IsNullOrEmpty(condition.AttributeUri) &&
                            attClassDict.ContainsKey(condition.AttributeUri))
                            conditionElement.SetAttribute(attributeAtt, attClassDict[condition.AttributeUri]);
                        conditionElement.SetAttribute(comparisonOperatorAtt, (int) condition.ComparisonOperator);
                        SetStringValue(condition.Value, valueAtt, conditionElement);
                        conditionElement.SetAttribute(groupedAtt, condition.IsGrouped);
                        conditionElement.SetAttribute(statusInGroupAtt, (int) condition.StatusInGroup);
                    }
                }
            }
        }

        /// <summary>
        ///     Gets the rule conditions.
        /// </summary>
        /// <param name="rangeRuleElement">The range rule element.</param>
        /// <returns>List&lt;Condition&gt;.</returns>
        private List<Condition> GetRuleConditions(DbElement rangeRuleElement)
        {
            var retVal = new List<Condition>();

            var conditionElements = rangeRuleElement.Members();
            var i = 0;

            foreach (var conditionElement in conditionElements)
                if (conditionElement.IsValid)
                {
                    var andOr = 0;
                    var attribute = string.Empty;
                    var comparisonOperator = 0;
                    var value = string.Empty;
                    var isGrouped = false;
                    var statusInGroup = 0;
                    var attributeAndClassElement = DbElement.GetElement();
                    var condition = new Condition();

                    condition.Id = GetRefAsId(conditionElement);
                    condition.Sequence = i;
                    if (conditionElement.GetValidInteger(andOrAtt, ref andOr))
                        condition.AndOr = (ConditionComposer) andOr;
                    if (conditionElement.GetValidRef(attributeAtt, ref attributeAndClassElement) &&
                        attributeAndClassElement.IsValid &&
                        attributeAndClassElement.GetValidString(attributeUriAtt, ref attribute))
                        condition.AttributeUri = attribute;
                    if (conditionElement.GetValidInteger(comparisonOperatorAtt, ref comparisonOperator))
                        condition.ComparisonOperator = (ComparisonOperator) comparisonOperator;
                    if (conditionElement.GetValidString(valueAtt, ref value)) condition.Value = value;
                    if (conditionElement.GetValidBool(groupedAtt, ref isGrouped)) condition.IsGrouped = isGrouped;
                    if (conditionElement.GetValidInteger(statusInGroupAtt, ref statusInGroup))
                        condition.StatusInGroup = (ConditionSequenceInGroup) statusInGroup;

                    retVal.Add(condition);
                    i++;
                }

            return retVal;
        }

        /// <summary>
        ///     Saves the att class elements.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <param name="rangeConfigElement">The range configuration element.</param>
        /// <returns>Dictionary&lt;System.String, DbElement&gt;.</returns>
        private Dictionary<string, DbElement> SaveAttClassElements(IEnumerable<Rule> rules,
            DbElement rangeConfigElement)
        {
            var retVal = new Dictionary<string, DbElement>();
            var allAttributes = new List<string>();

            foreach (var rule in rules)
            {
                var toConcat = rule.Conditions.Where(c => !string.IsNullOrEmpty(c.AttributeUri))
                    .Select(c => c.AttributeUri);
                allAttributes.AddRange(toConcat);
            }

            var distinctAttributes = allAttributes.Distinct().ToList();
            var attributeAndClassElements = rangeConfigElement.Members(attributeAndClassElemType);

            foreach (var attributeAndClassElement in attributeAndClassElements)
                if (attributeAndClassElement.IsValid &&
                    attributeAndClassElement.GetActualType() == attributeAndClassElemType) // due to corrupt db
                {
                    var attributeUri = string.Empty;
                    if (attributeAndClassElement.GetValidString(attributeUriAtt, ref attributeUri))
                    {
                        if (distinctAttributes.Contains(attributeUri))
                            retVal.Add(attributeUri, attributeAndClassElement); // update
                        else
                            attributeAndClassElement.Delete(); // delete
                    }
                }

            // create
            foreach (var distinctAttribute in distinctAttributes)
                if (!retVal.ContainsKey(distinctAttribute))
                {
                    var attributeAndClassElement = rangeConfigElement.CreateLast(attributeAndClassElemType);
                    attributeAndClassElement.SetAttributeValue(attributeUriAtt, distinctAttribute);
                    retVal.Add(distinctAttribute, attributeAndClassElement);
                }

            return retVal;
        }

        /// <summary>
        ///     Gets the range configuration element.
        /// </summary>
        /// <param name="rangeConfig">The range configuration.</param>
        /// <returns>DbElement.</returns>
        private DbElement GetRangeConfigElement(RangeConfig rangeConfig)
        {
            DbElement rangeConfigElement;
            if (string.IsNullOrEmpty(rangeConfig.Id))
            {
                var worldElement = MDB.CurrentMDB.GetFirstWorld(DbType.Engineering); //TODO: needs to change
                var engWorldElement = worldElement.FirstMember(DbElementTypeInstance.ENGWLD);
                if (engWorldElement.IsNull) engWorldElement = worldElement.CreateFirst(DbElementTypeInstance.ENGWLD);
                var engGroupElement = engWorldElement.FirstMember(DbElementTypeInstance.ENGGRP);
                if (engGroupElement.IsNull) engGroupElement = engWorldElement.CreateFirst(DbElementTypeInstance.ENGGRP);

                rangeConfigElement = engGroupElement.CreateLast(counterRangeConfigElemType);
            }
            else
            {
                rangeConfigElement = DbElement.GetElement(rangeConfig.Id);
            }

            return rangeConfigElement;
        }

        /// <summary>
        ///     Gets the range rule element.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <param name="rangeConfigElement">The range configuration element.</param>
        /// <returns>DbElement.</returns>
        private DbElement GetRangeRuleElement(Rule rule, DbElement rangeConfigElement)
        {
            DbElement rangeRuleElement;
            if (string.IsNullOrEmpty(rule.Id))
            {
                rangeRuleElement = rangeConfigElement.CreateFirst(rangeRuleElemType);
            }
            else
            {
                rangeRuleElement = DbElement.GetElement(rule.Id);
                rangeRuleElement.InsertAfterLast(rangeConfigElement);
            }

            return rangeRuleElement;
        }

        /// <summary>
        ///     Gets the condition element.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="rangeRuleElement">The range rule element.</param>
        /// <returns>DbElement.</returns>
        private DbElement GetConditionElement(Condition condition, DbElement rangeRuleElement)
        {
            DbElement conditionElement;
            if (string.IsNullOrEmpty(condition.Id))
            {
                conditionElement = rangeRuleElement.CreateLast(ruleConditionElemType);
            }
            else
            {
                conditionElement = DbElement.GetElement(condition.Id);
                conditionElement.InsertAfterLast(rangeRuleElement);
            }

            return conditionElement;
        }

        /// <summary>
        ///     Gets the reference as identifier.
        /// </summary>
        /// <param name="dbElement">The database element.</param>
        /// <returns>System.String.</returns>
        private static string GetRefAsId(DbElement dbElement)
        {
            return "=" + dbElement.RefNo()[0] + "/" + dbElement.RefNo()[1];
        }

        /// <summary>
        ///     Sets the string value.
        /// </summary>
        /// <param name="strValue">The string value.</param>
        /// <param name="att">The att.</param>
        /// <param name="conditionElement">The condition element.</param>
        private void SetStringValue(string strValue, DbAttribute att, DbElement conditionElement)
        {
            if (!string.IsNullOrEmpty(strValue)) conditionElement.SetAttributeValue(att, strValue);
        }
    }
}