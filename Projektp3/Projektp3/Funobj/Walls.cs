using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektp3.Funobj
{
    public class Walls : FunObject
    {
        public Walls(int x, int y) : base(x, y)
        {
            ID = 'W';
            Setchar('W');
        }
        public Walls(int x, int y, char T) : base(x, y)
        {
            Setchar(T);
            ID = 'W';
        }
    }
}
