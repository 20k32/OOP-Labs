using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal interface ICloneable<T>
    {
        void Clone(out T value);
    }
}
