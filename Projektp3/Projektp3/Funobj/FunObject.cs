using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektp3.Funobj
{
    public class FunObject : IGameObject
    {
        public int k = 0;
        public int p = 0;
        public int x { get; set; }
        public int y { get; set; }
        public char ID { get; set; }
        public char def { get; private set; } = ' ';
        public int cost = 0;
        public int heuristic = 0;
        public int total = 0;
        public FunObject(int x, int y)
        {
            this.x = x;
            this.y = y;
            ID = 'C';
        }
        public FunObject(int x, int y, char c, char id)
        {
            this.x = x;
            this.y = y;
            def = c;
            ID = id;

        }
        public void SetXY(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public char GetChar()
        {
            return def;
        }
        public void Setchar(char x)
        {
            def = x;

        }
        public void MoveRight()
        {
            x++;
        }
        public void MoveLeft()
        {
            x--;
        }
        public void MoveUp()
        {
            y--;
        }
        public void MoveDown()
        {
            y++;
        }
        public override bool Equals(object obj)
        {
            if (obj is FunObject f)
                return f.x == x && f.y == y;

            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }


}
