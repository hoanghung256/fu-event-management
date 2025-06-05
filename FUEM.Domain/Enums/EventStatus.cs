using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Enums
{
    public enum EventStatus
    {
        [EnumMember(Value = "PENDING")]
        Pending,

        [EnumMember(Value = "APPROVED")]
        Approved,

        [EnumMember(Value = "REJECTED")]
        Rejected,

        [EnumMember(Value = "ON_GOING")]
        OnGoing,

        [EnumMember(Value = "CANCEL")]
        Cancel,

        [EnumMember(Value = "END")]
        End,
    }
}
