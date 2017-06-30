using RomanticWeb.Entities;
using RomanticWeb.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSOnt
{
    [Class("http://www.schneider-electric.com/Buildings#EnterpriseServer")]
    public interface IES : INode
    {
        [Collection("http://www.schneider-electric.com/Buildings#monitors")]
        ICollection<IAS> Monitors { get; set; }
    }
}