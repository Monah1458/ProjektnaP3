using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektp3.Funobj
{
    public class Enemy : FunObject 
    {
        public List<FunObject> PathList;
        public List<FunObject> DistractionPathList;
        public int pd = 0;
        public int kd = 0;
        public bool toD = false;
        public Enemy(int x, int y) : base(x, y)
        {
            Setchar('E');
            ID = 'E';
            PathList = new List<FunObject>();
            DistractionPathList = new List<FunObject>();
        }
        public Enemy(int x, int y, char E) : base(x, y)
        {
            Setchar(E);
            ID = 'E';
            PathList = new List<FunObject>();
            DistractionPathList = new List<FunObject>();
        }
    }

}
