using Lab1.GameAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1;

internal interface IMappable<T>
{
    void Map(out T entity);
}
