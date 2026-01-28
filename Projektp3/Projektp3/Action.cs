using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Projektp3.Funobj;

namespace Projektp3
{

    static class MapAction 
    {   
        
        static int[] Dist=new int[2];        

        public static void SetDistr(Map map)
        {
            map.AddToMap(new Trap(map.player.y+1, map.player.x,'X'));
            Dist[1]=map.player.x; 
            Dist[0]=map.player.y+1;

            
        }
        public static void TriggerDistr(Map map)
        {
            if(Dist[0]==0&& Dist[1]==0) return;
            for (int i = 0; i < map.listEnemies.Count; i++) 
            map.listEnemies[i].DistractionPathList=PathToDistr(map,i);

            int sh=FindShorter(map);
            map.listEnemies[sh].toD=true;
            map.listEnemies[sh].pd=map.listEnemies[sh].p;
            map.listEnemies[sh].kd=map.listEnemies[sh].k;
            map.listEnemies[sh].p=0;
            map.listEnemies[sh].k=0;

            map.DelObject(Dist[0], Dist[1]);
            Dist[0]=0; Dist[1]=0;
        }

        public static int FindShorter(Map map)
        {
            int s = map.listEnemies[0].DistractionPathList.Count;
            int ind=0;
            for (int i = 1; i < map.listEnemies.Count; i++)
            {
                if (map.listEnemies[i].DistractionPathList.Count<s) { 
                    s=map.listEnemies[i].DistractionPathList.Count;
                    ind=i;
                    }
            }
            return ind;
        }


       public static List<FunObject> PathToDistr(Map map, int i) 
       {
            
            Pathfinder pf=new Pathfinder(map);
            List<FunObject> list = 
                pf.SearchPath(new FunObject(Dist[1], Dist[0]),
                              map.listEnemies[i]);
            list.Add(new FunObject(Dist[1], Dist[0]));            
            list.Insert(0, new FunObject(map.listEnemies[i].x, map.listEnemies[i].y));
            return list;
        }            

        

        public static void EnemyStartPath(Map map)
        {
            for (int i = 0; i < map.listEnemies.Count; i++)
            {
                Pathfinder pf=new Pathfinder(map);
                map.listEnemies[i].PathList=              
                    pf.SearchPath(new FunObject(map.listEnemies[i].pd, map.listEnemies[i].kd),
                                  map.listEnemies[i]);
                map.listEnemies[i].PathList.Add(new FunObject(map.listEnemies[i].pd, map.listEnemies[i].kd));               
                map.listEnemies[i].PathList.Insert(0, new Enemy(map.listEnemies[i].x, map.listEnemies[i].y));                
                map.listEnemies[i].k=0;
                map.listEnemies[i].p=0;
            }
           
        }
        public static void MoveEnemes(Map map)
        {
            if (map.listEnemies.Count==0)
            {
                return;
            }
            for(int i = 0; i<map.listEnemies.Count; i++)
            {

                if (map.listEnemies[i].k==2&&map.listEnemies[i].toD)
                {
                    map.listEnemies[i].toD=false;
                    map.listEnemies[i].p=map.listEnemies[i].pd+1;
                    map.listEnemies[i].k=map.listEnemies[i].kd;
                    map.listEnemies[i].pd=0;
                    map.listEnemies[i].kd=0;
                }

                if (map.listEnemies[i].toD)                                            
                EnemyAlongPath(map, map.listEnemies[i].DistractionPathList,i);

                else EnemyAlongPath(map, map.listEnemies[i].PathList,i);
                
            }
        }
        
        public static void EnemyAlongPath(Map map, List<FunObject> list,int i)
        {
            char next;
            if (map.listEnemies[i].k%2==1)
            {
                next= NextId(map, list, map.listEnemies[i].p-2);
            }else next= NextId(map, list, map.listEnemies[i].p+1);
            switch (next)
            {
                case 'P':
                    GameOver();
                    break;
                case 'E':
                    break;
                case 'T':
                    DelObj(map, map.listEnemies[i].y, map.listEnemies[i].x);
                    break;
                case 'B':
                    break;
                default:
                    if (map.listEnemies[i].k%2==1)
                        map.listEnemies[i].p--;
                    else
                        map.listEnemies[i].p++;
                    if (map.listEnemies[i].p==list.Count) 
                    {
                        map.listEnemies[i].k++;
                        map.listEnemies[i].p--;
                        MovePathEnemy(map, list, map.listEnemies[i].p,i);                   
                    } 
                    else if (map.listEnemies[i].p==0&&map.listEnemies[i].k!=0)
                    {
                        map.listEnemies[i].k++;
                        map.listEnemies[i].p++;
                        MovePathEnemy(map, list, map.listEnemies[i].p,i);
                    }
                    else MovePathEnemy(map, list, map.listEnemies[i].p,i);
                    break;

            }
        }
        public static void MovePathEnemy(Map map, List<FunObject> list, int p,int i)
        {
                
            map.MoveTo(list[p-1].x, list[p-1].y, list[p].x, list[p].y);
            if(map.listEnemies[i].k%2==0)
            {
                map.listEnemies[i].x=list[p].x;
                map.listEnemies[i].y= list[p].y;
            }
            else
            {
                map.listEnemies[i].x=list[p-1].x;
                map.listEnemies[i].y= list[p-1].y;
            }

        }

        public static char NextId(Map map,List<FunObject> list,int i) 
        {
            if (i==list.Count)
            {
                return 'C';
            }
            if (i<=0)
            {
                return 'C';
            }
            return map.GetID(list[i].y, list[i].x);
        }
              
        public static bool PushByPlayer(Map map,string d)
        {
            switch (d) 
            {                
                case "Down":
                    if (map.GetID(map.player.y, map.player.x+2)=='T') DelObj(map,map.player.y, map.player.x+2);
                    if (map.GetID(map.player.y, map.player.x+2)=='C')
                    {
                        map.MoveTo(map.player.x+1, map.player.y,
                                    map.player.x+2, map.player.y);
                    }
                    
                    else return false;
                    break;
                case "Up":
                    if (map.GetID(map.player.y, map.player.x-2)=='T') DelObj(map, map.player.y, map.player.x-2);
                        if (map.GetID(map.player.y, map.player.x-2)=='C')
                    {
                        map.MoveTo(map.player.x-1, map.player.y,
                                    map.player.x-2, map.player.y);
                    }
                    else return false;
                    break;
                case "Left":
                    if (map.GetID(map.player.y-2, map.player.x)=='T') DelObj(map, map.player.y-2, map.player.x);
                        if (map.GetID(map.player.y-2, map.player.x)=='C')
                    {
                        map.MoveTo(map.player.x, map.player.y-1,
                                    map.player.x, map.player.y-2);
                    }
                    else return false;
                    break;
                case "Right":
                    if (map.GetID(map.player.y+2, map.player.x)=='T') DelObj(map, map.player.y+2, map.player.x);
                    if (map.GetID(map.player.y+2, map.player.x)=='C')
                    {
                        map.MoveTo(map.player.x, map.player.y+1,
                                    map.player.x, map.player.y+2);
                    }
                    else return false;
                    break;
                default:
                    break;
            }
            return true;
        }

        public static void PlayerMove(Map map,int x,int y,string d)
        {
            switch (map.GetID(y, x)) {
                case 'W':
                    break;
                case 'O':
                    break;
                case 'E':
                    MapAction.GameOver();
                    break;
                case 'T':
                    MapAction.GameOver();
                    break ;
                case 'I':
                    EnterPreviousMap();
                    break;
                case 'L':
                    EnterNextMap();
                    break;
                case 'B':
                    if (MapAction.PushByPlayer(map, d)) 
                    {
                        MovePlayer(map, x, y);
                    }
                    break;
                default:
                    MovePlayer(map, x, y);             
                    break;

            }

        }
        public static void MovePlayer(Map map, int x, int y)
        {
            map.MoveTo(x, y, map.player.x, map.player.y);
            map.player.x=x; map.player.y=y;
        }
        public static void EnterNextMap()
        {
            GameLoop.m++;
            GameLoop.changeMap=true;
        }
        public static void EnterPreviousMap()
        {
            GameLoop.m--;
            GameLoop.changeMap=true;
            GameLoop.moveBack=true;
        }
        public static void PlantBomb(Map map)
        {
            map.AddToMap(new Trap(map.player.y+1, map.player.x, 'O'));
            map.listABombs.Add(new Trap(map.player.y+1, map.player.x));
        }
        public static void BombLog(Map map)
        {
            if (map.listABombs.Count==0) return;
            for (int i = 0; i < map.listABombs.Count; i++)
            {
                if (map.listABombs[i].k==3) 
                { 
                    TrigerBomb(map, i);               
                }
                else map.listABombs[i].k++;
            }
        }
        public static void TrigerBomb(Map map,int i)
        {
            int x = map.listABombs[i].x;
            int y = map.listABombs[i].y;
            int b=1;
            int left = b;
            int right = b;
            int up = b;
            int down = b;
           
            if (map.GetID(x - 1, y) == 'W') right++;
            if (map.GetID(x + 1, y) == 'W') left++;
            if (map.GetID(x, y - 1) == 'W') down++;
            if (map.GetID(x, y + 1) == 'W') up++;
            

            for (int j = 1; j <= left; j++)
            {
                if (map.GetID(x - j, y) == 'W') break;
                DelObj(map, x - j, y);
            }

            
            for (int j = 1; j <= right; j++)
            {
                if (map.GetID(x + j, y) == 'W') break;
                DelObj(map, x + j, y);
            }

            for (int j = 1; j <= up; j++)
            {
                if (map.GetID(x, y - j) == 'W') break;
                DelObj(map, x, y - j);
            }

            
            for (int j = 1; j <= down; j++)
            {
                if (map.GetID(x, y + j) == 'W') break;
                DelObj(map, x, y + j);
            }

            DelObj(map,x, y);

        }
        public static void DelObj(Map map,int x,int y)
        {
                     
            switch (map.GetID(x,y))
                
            {
                case 'E':
                    for (int i = 0; i <   map.listEnemies.Count; i++)
                    {
                        if (map.listEnemies[i].x==y&&map.listEnemies[i].y==x) 
                        { 
                            map.listEnemies.RemoveAt(i);
                            break;
                        }                        
                    }
                    break;
                case 'P':
                    if (x==map.player.x&&y==map.player.y) MapAction.GameOver();
                    break;
                case 'L':
                    return;
                case 'I':
                    return;
                case 'O':
                    for (int i = 0; i < map.listABombs.Count; i++)
                    {
                        if (map.listABombs[i].x==x&&map.listABombs[i].y==y) 
                        {
                        
                            map.listABombs.RemoveAt(i);
                            break;                          
                        }
                    }
                    
                    break;
                default:
                    break;              
            } 
            map.DelObject(x, y);

        }

        public static void GameOver()
        { 
            
            Console.Clear(); 
            GameOverScreen.Show(false);
                      
        }
        public static void GameWon()
        {

            Console.Clear();
            GameOverScreen.Show(true);

        }

    }

}
        
