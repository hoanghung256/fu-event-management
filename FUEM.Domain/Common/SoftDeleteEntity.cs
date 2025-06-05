using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Common
{
    public class SoftDeleteEntity
    {
        Boolean IsDelete { get; set; } = false;
        DateTime? DeletedAt { get; set; }
    }
}
