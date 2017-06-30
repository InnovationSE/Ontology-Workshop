using RomanticWeb.Entities;
using RomanticWeb.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BMSOnt
{
    public enum Unit
    {
        [Description("http://www.schneider-electric.com/Buildings#A")]
        Ampere,

        [Description("http://www.schneider-electric.com/Buildings#DegreeC")]
        Celsius,

        [Description("http://www.schneider-electric.com/Buildings#DegreeF")]
        Fahrenheit,

        [Description("http://www.schneider-electric.com/Buildings#V")]
        Volt,

        [Description("http://www.schneider-electric.com/Buildings#W")]
        Watt,

        [Description("http://www.schneider-electric.com/Buildings#Wh")]
        Watt_Hour,

        [Description("http://www.schneider-electric.com/Buildings#OtherUnits")]
        OtherUnits
    }
}