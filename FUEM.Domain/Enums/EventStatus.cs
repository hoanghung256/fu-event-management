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
        PENDING,
        APPROVED,
        REJECTED,
        ON_GOING,
        CANCEL,
        END,
    }
}
