using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Enums
{
    /*
     * Usage:
     * FileType.Image.Name => "IMAGE"
     * FileType.Image.Location => "image"
     */
    public sealed record FileType(string Name, string Location)
    {
        public static readonly FileType Image = new("IMAGE", "image");
        public static readonly FileType Document = new("DOCUMENT", "document");

        public static FileType FromName(string name) =>
            name.ToUpper() switch
            {
                "IMAGE" => Image,
                "DOCUMENT" => Document,
                _ => throw new ArgumentException($"Unknown FileType: {name}")
            };
    }
}
