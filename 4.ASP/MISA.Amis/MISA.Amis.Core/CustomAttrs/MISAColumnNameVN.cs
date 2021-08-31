using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Amis.Core.CustomAttrs
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MISAColumnNameVN:Attribute
    {
        public MISAColumnNameVN(string name, int width)
        {
            Name = name;
            Width = width;
        }
        public string Name { get; set; }
        public int Width { get; set; }
    }
}
