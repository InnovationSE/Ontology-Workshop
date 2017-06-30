using RomanticWeb.Entities;
using RomanticWeb.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSOnt
{
    [Class("http://www.schneider-electric.com/Buildings#AutomationServer")]
    public interface IAS : INode
    {
        [Collection("http://www.schneider-electric.com/Buildings#controls")]
        ICollection<IController> Controls { get; set; }
    }
}