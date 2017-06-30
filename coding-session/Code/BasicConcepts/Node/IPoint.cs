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
    [Class("http://www.schneider-electric.com/Buildings#Point")]
    public interface IPoint : INode
    {
        [Property("http://www.schneider-electric.com/Buildings#hasType")]
        string Type { get; set; }

        [Collection("http://www.schneider-electric.com/Buildings#hasUnit")]
        string Unit { get; set; }
    }
}