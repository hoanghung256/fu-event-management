using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Common
{
    public class PathHelper
    {
        public static string ToFirebasePath(string path)
        {
            return path.Replace("\\", "/");
        }
    }
}
