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
    public enum Quantity
    {
        [Description("http://www.schneider-electric.com/Buildings#Temperature")]
        Temperature,

        [Description("http://www.schneider-electric.com/Buildings#Humidity")]
        Humidity,

        [Description("http://www.schneider-electric.com/Buildings#Power")]
        Power,

        [Description("http://www.schneider-electric.com/Buildings#Energy")]
        Energy,

        [Description("http://www.schneider-electric.com/Buildings#Voltage")]
        Voltage,

        [Description("http://www.schneider-electric.com/Buildings#Current")]
        Current,

        [Description("http://www.schneider-electric.com/Buildings#Pressure")]
        Pressure,

        [Description("http://www.schneider-electric.com/Buildings#Others")]
        Others
    }
}