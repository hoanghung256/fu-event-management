using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Enums
{
    public enum Role
    {
        [EnumMember(Value = "ADMIN")]
        Admin,

        [EnumMember(Value = "STUDENT")]
        Student,

        [EnumMember(Value = "Club")]
        Club
    }
}
