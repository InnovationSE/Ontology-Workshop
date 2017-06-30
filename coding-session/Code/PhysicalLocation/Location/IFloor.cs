using RomanticWeb.Entities;
using RomanticWeb.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSOnt
{
    [Class("http://www.schneider-electric.com/Buildings#Floor")]
    public interface IFloor : ILocation
    {
        [Collection("http://www.schneider-electric.com/Buildings#hasLocationRoom")]
        ICollection<IRoom> HasRoom { get; set; }
    }
}
