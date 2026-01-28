using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Projektp3
{
    static class PlayerInput
    {

        static public void KeyPlayerInput(Map map) 
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            ConsoleKey key = keyInfo.Key;            
            switch (key)
            {
                case ConsoleKey.A:
                    MapAction.PlayerMove(map, map.player.x, map.player.y-1, "Left");
                    break;

                case ConsoleKey.W:
                    MapAction.PlayerMove(map, map.player.x-1, map.player.y, "Up");
                    break;
                case ConsoleKey.D:
                    MapAction.PlayerMove(map, map.player.x, map.player.y+1, "Right");
                    break;
                case ConsoleKey.S:
                    MapAction.PlayerMove(map, map.player.x+1, map.player.y, "Down");
                    break;
                case ConsoleKey.T:
                    if(map.GetID(map.player.y+1,map.player.x)=='C')
                        MapAction.SetDistr(map);
                    break;
                case ConsoleKey.U:
                    MapAction.TriggerDistr(map);
                    break;
                case ConsoleKey.B:
                    if (map.GetID(map.player.y+1, map.player.x)=='C')
                        MapAction.PlantBomb(map);
                    break;
                
                default:
                    break;
            }

        }

    }
}
