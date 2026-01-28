using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektp3.Funobj
{
    public class Player : FunObject
    {
        public Player(int x, int y) : base(x, y)
        {
            ID = 'P';
            Setchar('P');
        }
        public Player(int x, int y, char P) : base(x, y)
        {
            ID = 'P';
            Setchar(P);
        }


    }
}
