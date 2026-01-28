using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projektp3.Funobj;

namespace Projektp3
{
    public class MovingWalls : Walls
    {
        public int sx { get; set; }
        public int sy { get; set; }
        public MovingWalls(int x, int y) : base(x, y)
        {           
            this.ID='B';
            this.Setchar('B');
        }
        public MovingWalls(int x, int y, char T) : base(x, y, T)
        {
            this.ID='B';
            this.Setchar('B');
        }
        public void SetSxSy(int x, int y)
        {
            this.sx = x;
            this.sy = y;
        }
    }
}
