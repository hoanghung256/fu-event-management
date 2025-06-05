using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Enums
{
    public enum Gender
    {
        [EnumMember(Value = "MALE")]
        Male,

        [EnumMember(Value = "FEMALE")]
        Female,

        [EnumMember(Value = "OTHER")]
        Other
    }
}
