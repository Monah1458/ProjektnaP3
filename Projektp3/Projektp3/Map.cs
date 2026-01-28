using Microsoft.VisualBasic.FileIO;
using Projektp3.Funobj;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Projektp3
{
    public class Map : IMap
    {
        public FunObject[,] map;
        public List<Enemy> listEnemies = new List<Enemy>();
        public List<FunObject> listABombs = new List<FunObject>();
        public FunObject enter;
        public FunObject exit;
        public Player player {  get; set; }
        public int x { get; private set; }
        public int y { get; private set; }
        public Map(string mapfile)
        {
            listEnemies.Clear();
            listABombs.Clear();    
            map=Mapfile.FileMap(mapfile);
            listEnemies=Mapfile.EnemiesList;
            player=Mapfile.player;
            exit=Mapfile.exit;

        }
        public Map(int x, int y)
        {
            this.x = x;
            this.y = y;
            map=new FunObject[x,y];


            Random rand = new Random(62346);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j <y; j++)
                {
                    if (rand.Next(0, 100) < 25)
                    {

                        this.map[i, j]= new Walls(i, j);
                    }
                    else
                        this.map[i, j]= new FunObject(i, j);

                }
            }
        }
        public void PrintMap()
        {

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j].GetChar());
                }
                Console.WriteLine();
            }
        }
        public void AddToMap<T>(T t) where T : FunObject
        {

            map[t.y, t.x]=t;
        }
        public void MoveLeft<T>(T t) where T : FunObject
        {
            map[t.y, t.x]=new FunObject(t.x, t.x);
            map[t.y, t.x-1]=t;
            t.MoveLeft();
        }

        public void MoveRight<T>(T t) where T : FunObject
        {
            map[t.y, t.x]=new FunObject(t.x, t.y);
            map[t.y, t.x+1]=t;
            t.MoveRight();
        }
        public void MoveUp<T>(T t) where T : FunObject
        {
            map[t.y, t.x]=new FunObject(t.x, t.y);
            map[t.y-1, t.x]=t;
            t.MoveUp();
        }
        public void MoveDown<T>(T t) where T : FunObject
        {

            map[t.y,t.x]=new FunObject(t.x, t.y);
            map[t.y+1, t.x]=t;
            t.MoveDown();
        }
        public void MoveTo<T>(T t, FunObject z) where T : FunObject
        {
            var temp = map[z.x, z.y];
            map[z.x, z.y] = t;
            map[t.x, t.y] = temp;
            t.SetXY(z.x, z.y);
            temp.SetXY(t.x, t.y);
        }
        public void MoveTo(int x,int y, FunObject z) 
        {
            (map[x, y], map[z.x,z.y]) = (map[z.x, z.y], map[x, y]);
            map[x, y].SetXY(x, y);
            map[z.x, z.y].SetXY(z.x, z.y);
        }
        public void MoveTo(int x, int y, int sx, int sy)
        {
            var tmp = map[sx, sy];       
            map[sx, sy]=map[x,y];
            map[x, y]=tmp;
            map[x, y].SetXY(x, y);
            map[sx, sy].SetXY(sx, sy);
            

        }

        public void DelObject<T>(T t) where T : FunObject
        {
            if (map[t.y, t.x]==player) 
                MapAction.GameOver();
            map[t.y, t.x]=new FunObject(t.x, t.y);
        }
        public void DelObject(int x, int y)
        {
            if (y==player.x&&x==player.y) 
                MapAction.GameOver();
            map[y, x]=new FunObject(x, y);
        }

        public char GetChar(int x, int y)
        {
            return this.map[y, x].GetChar();
            
        }
        public char GetID(int x, int y)
        {
            return this.map[y, x].ID;
        }




    }
}