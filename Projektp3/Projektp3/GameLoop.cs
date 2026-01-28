using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektp3
{
    public static class GameLoop
    {
        public static int m = 0;
        public static bool changeMap=false;
        public static bool moveBack = false;
        private static readonly string[] mapsFiles = {
            
            "mapsLevel1.json",
            "mapsLevel2.json",
            "TextFile2.txt"
            };
        public static void GameSetup(Map map){
            MapAction.EnemyStartPath(map);
            
        }
        public static void GameStart() 
        { 
            Map map = new Map(mapsFiles[m]);
            GameSetup(map);
            do
            {
                if (changeMap)
                {
                    map= new Map(mapsFiles[m]);

                    GameSetup(map);
                    changeMap=false;
                    Console.Clear();
                    map.PrintMap();
                    if (m==0) {
                        MapAction.MovePlayer(map,map.exit.x, map.exit.y-1);
                        moveBack = false;
                    }
                }
                Console.Clear();
                map.PrintMap();              
                Thread.Sleep(100);
                MapAction.MoveEnemes(map);
                MapAction.BombLog(map);
                PlayerInput.KeyPlayerInput(map);

                if (m==mapsFiles.Count()) MapAction.GameWon();
            } while (true);

        }
        
        

    }
}
