using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Mapper;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
internal class MappableAttribute : Attribute
{
    public readonly Type ToType;

    public MappableAttribute(Type toType) 
        => ToType = toType;
}
