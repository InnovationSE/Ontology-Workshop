using RomanticWeb.Entities;
using RomanticWeb.Mapping.Attributes;
using RomanticWeb.Mapping.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSOnt
{
    public interface INode : IEntity
    {
        [Property("http://www.schneider-electric.com/Buildings#hasUID")]
        string UID { get; set; }

        [Property("http://www.schneider-electric.com/Buildings#hasName")]
        string Name { get; set; }
    }
}