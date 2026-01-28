using Projektp3.Funobj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektp3
{
    public class Pathfinder
    {
        public FunObject[,] map;
        FunObject startTile;
        FunObject endTile;
        FunObject currentTile;
        List<FunObject> closedList = new List<FunObject>();
        List<FunObject> openList = new List<FunObject>();

        public Pathfinder(Map map)
        {
            this.map=new FunObject[map.x, map.y];
            this.map = map.map;
        }
        public List<FunObject> SearchPath(FunObject startTile, FunObject endTile)
        {
            this.startTile = startTile;
            this.endTile = endTile;


            for (int i = 0; i <  map.GetLength(0); i++)
            {
               
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j].total = 0;
                    map[i, j].heuristic = 0;
                }
            }
            bool canSearch = true;
            if (map[startTile.x, startTile.y].ID == 'W')
            {

                Console.WriteLine(startTile.x+"=x y= "+startTile.y+" "+map[startTile.x, startTile.y].ID);
                Console.WriteLine("The start tile is non walkable. Choose a different value than: " + startTile.ToString());
                canSearch = false;
            }
            if (map[endTile.x, endTile.y].ID == 'W')
            {
                Console.WriteLine("The end tile is non walkable. Choose a different value than: " + endTile.ToString());
                canSearch = false;
            }

            if (canSearch)
            {
                openList.Add(startTile);
                currentTile = new FunObject(-1, -1);

                while (openList.Count != 0)
                {
                    currentTile = GetTileWithLowestTotal(openList);

                    if (currentTile.x == endTile.x && currentTile.y == endTile.y)
                    {
                        break;
                    }
                    else
                    {
                        openList.Remove(currentTile);

                        closedList.Add(currentTile);

                        List<FunObject> adjacentTiles = GetAdjacentTiles(currentTile);
                        foreach (FunObject adjacentTile in adjacentTiles)
                        {
                            if (!openList.Contains(adjacentTile))
                            {
                                if (!closedList.Contains(adjacentTile))
                                {
                                    openList.Add(adjacentTile);

                                    FunObject tile = map[adjacentTile.x, adjacentTile.y];

                                    tile.cost = map[currentTile.x, currentTile.y].cost + 1;

                                    tile.heuristic = ManhattanDistance(adjacentTile);
                                    tile.total = tile.cost + tile.heuristic;


                                }
                            }
                        }
                    }
                }
            }

            return ShowPath();
        }

        public List<FunObject> ShowPath()
        {
            bool startFound = false;

            FunObject currentTile = endTile;
            List<FunObject> pathTiles = new List<FunObject>();

            while (startFound == false)
            {
                List<FunObject> adjacentTiles = GetAdjacentTiles(currentTile);

                foreach (FunObject adjacentTile in adjacentTiles)
                {
                    if (adjacentTile.x == startTile.x && adjacentTile.y == startTile.y)
                        startFound = true;

                    if (closedList.Contains(adjacentTile) || openList.Contains(adjacentTile))
                    {
                        if (map[(int)adjacentTile.x, (int)adjacentTile.y].cost <= map[(int)currentTile.x, (int)currentTile.y].cost
                            && map[(int)adjacentTile.x, (int)adjacentTile.y].cost > 0)
                        {
                            currentTile = adjacentTile;

                            pathTiles.Add(adjacentTile);

                            break;
                        }
                    }
                }
            }

            return pathTiles;
        }
        public int ManhattanDistance(FunObject adjacentTile)
        {
            int manhattan = Math.Abs((int)(endTile.x - adjacentTile.x)) +  Math.Abs((int)(endTile.y - adjacentTile.y));
            return manhattan;
        }
        public List<FunObject> GetAdjacentTiles(FunObject currentTile)
        {
            List<FunObject> adjacentTiles = new List<FunObject>();
            FunObject adjacentTile;

            adjacentTile = new FunObject(currentTile.x, currentTile.y + 1);
            if (adjacentTile.y < map.GetLength(1) && map[adjacentTile.x, adjacentTile.y].ID != 'W')
                adjacentTiles.Add(adjacentTile);

            adjacentTile = new FunObject(currentTile.x, currentTile.y - 1);
            if (adjacentTile.y >= 0 && map[adjacentTile.x, adjacentTile.y].ID != 'W')
                adjacentTiles.Add(adjacentTile);

            adjacentTile = new FunObject(currentTile.x + 1, currentTile.y);
            if (adjacentTile.x < map.GetLength(0) && map[adjacentTile.x, adjacentTile.y].ID != 'W')
                adjacentTiles.Add(adjacentTile);

            adjacentTile = new FunObject(currentTile.x - 1, currentTile.y);
            if (adjacentTile.x >= 0 && map[adjacentTile.x, adjacentTile.y].ID != 'W')
                adjacentTiles.Add(adjacentTile);

            return adjacentTiles;
        }

        public FunObject GetTileWithLowestTotal(List<FunObject> openList)
        {
            FunObject tileWithLowestTotal = new FunObject(-1, -1);
            int lowestTotal = int.MaxValue;

            foreach (FunObject openTile in openList)
            {
                if (map[openTile.x, openTile.y].total <= lowestTotal)
                {
                    lowestTotal = map[openTile.x, openTile.y].total;
                    tileWithLowestTotal = new FunObject(openTile.x, openTile.y);
                }
            }

            return tileWithLowestTotal;
        }
    }
}

