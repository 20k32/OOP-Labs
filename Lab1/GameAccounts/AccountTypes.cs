using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.GameAccounts
{
    internal sealed class AccountTypes
    {
        public static readonly TypeUnit StandardModeAccount = new(0, "Standard");
        public static readonly TypeUnit HardModeAccount = new(1, "Hard");
        public static readonly TypeUnit ArcadeModeAccount = new(2, "Arcade");
    }
}
