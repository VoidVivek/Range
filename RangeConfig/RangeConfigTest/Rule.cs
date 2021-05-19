using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Aveva.CounterRange.ViewModels;
using Aveva.CounterRange.Enums;

namespace RangeConfigTest
{
    public class Rule
    {
        RangeRuleControlViewModel rule;
        Delegate compiled;

        public string MinMax { get; set; }

        public Rule(RangeRuleControlViewModel rule)
        {
            this.rule = rule;
        }


    }
}
