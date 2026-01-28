using Projektp3.Funobj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektp3
{
    internal interface IMap
    {
        
        void PrintMap();
        void AddToMap<T>(T t) where T : FunObject;
       
        void MoveLeft<T>(T t) where T : FunObject;

        void MoveRight<T>(T t) where T : FunObject;
        void MoveUp<T>(T t) where T : FunObject;
        void MoveDown<T>(T t) where T : FunObject;
        void MoveTo<T>(T t, FunObject z) where T : FunObject;
        void MoveTo(int x, int y, FunObject z);
        void MoveTo(int x, int y, int sx, int sy);

        void DelObject<T>(T t) where T : FunObject;
        void DelObject(int x, int y);
        char GetChar(int x, int y);
        char GetID(int x, int y);
    }
}
