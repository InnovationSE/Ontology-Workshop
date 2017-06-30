using RomanticWeb.Entities;
using RomanticWeb.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSOnt
{
    [Class("http://www.schneider-electric.com/Buildings#Building")]
    public interface IBuilding : ILocation
    {
        [Collection("http://www.schneider-electric.com/Buildings#hasLocationFloor")]
        ICollection<IFloor> HasFloor { get; set; }
    }
}
