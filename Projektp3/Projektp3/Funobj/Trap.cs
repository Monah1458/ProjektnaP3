using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektp3.Funobj
{
    class Trap : FunObject
    {
        public Trap(int x, int y) : base(x, y)
        {
            ID = 'T';
            Setchar('T');
        }
        public Trap(int x, int y, char T) : base(x, y)
        {
            Setchar(T);
            ID = T;
        }

    }
}
