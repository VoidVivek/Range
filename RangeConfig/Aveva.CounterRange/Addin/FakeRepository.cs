using System;
using System.Collections.Generic;
using Aveva.CounterRange.Repositories;
using Aveva.CounterRange.Models;
using System.Xml.Serialization;

namespace Aveva.CounterRange
{
    public class FakeRepository : IRepository
    {
        public IEnumerable<RangeConfig> GetRangeConfigs()
        {
            var config1 = new RangeConfig() { Id = "1", ClassUri = "ClassUri1", DisplayName = "Range Config 1" };
            var config2 = new RangeConfig() { Id = "2", ClassUri = "ClassUri2", DisplayName = "Range Config 2" };

            return new List<RangeConfig>() { config1, config2 };
        }

        public IEnumerable<Aveva.CounterRange.Models.Rule> GetRulesForConfig(string rangeConfigId)
        {
            if (rangeConfigId == "1")
            {
                return GetConfig1Rules();
            }
            else if (rangeConfigId == "2")
            {
                return GetConfig2Rules();
            }
            else if (string.IsNullOrEmpty(rangeConfigId))
            {
                return new List<Aveva.CounterRange.Models.Rule>();
            }
            else
                throw new NotImplementedException("Only 1 & 2 have been implemented");
        }

        private IEnumerable<Aveva.CounterRange.Models.Rule> GetConfig1Rules()
        {
            return GetRules(1);
        }

        private IEnumerable<Aveva.CounterRange.Models.Rule> GetConfig2Rules()
        {
            return GetRules(2);
        }

        private static IEnumerable<Aveva.CounterRange.Models.Rule> GetRules(int number)
        {
            List<Aveva.CounterRange.Models.Rule> rules = new List<Aveva.CounterRange.Models.Rule>();
            var reader = new XmlSerializer(typeof(List<Aveva.CounterRange.Models.Rule>));
            var path = "Rules" + number + ".xml";
            using (System.IO.StreamReader file = new System.IO.StreamReader(path))
            {
                rules = (List<Aveva.CounterRange.Models.Rule>)reader.Deserialize(file);

                for (int i = rules.Count - 1; i >= 0; i--)
                {
                    var item = rules[i];
                    System.Diagnostics.Debug.WriteLine(item.ToString());
                }

                file.Close();
            }

            return rules;
        }

        public void Save(RangeConfig rangeConfig, IEnumerable<Aveva.CounterRange.Models.Rule> rules)
        {
            var configToSave = rangeConfig;
            var rulesToSave = rules;
        }
    }
}
