using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Enums
{
    public enum FileStatus
    {
        [EnumMember(Value = "PENDING")]
        Pending,

        [EnumMember(Value = "REVIEWING")]
        Reviewing,

        [EnumMember(Value = "APPROVED")]
        Approved,

        [EnumMember(Value = "REQUEST_CHANGE")]
        RequestChange
    }
}
