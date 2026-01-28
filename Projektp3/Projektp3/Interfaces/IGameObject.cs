using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektp3
{
    public interface IGameObject
    {    
        int x { get; set; }
        int y { get; set; }
        char def { get; }
        char ID { get; }
   
        void SetXY(int x, int y);
        public char GetChar();

        public void Setchar(char x);

        public void MoveRight();

        public void MoveLeft();

        public void MoveUp();

        public void MoveDown();  

    }
}
