using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Projektp3;
using Projektp3.Funobj;

namespace Projektp3
{ 
    public class MapData
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public char[][] Grid { get; set; }
        public List<EnemyData> Enemies { get; set; } = new List<EnemyData>();
        public List<MovingWallData> MovingWalls { get; set; } = new List<MovingWallData>();
        public Position PlayerPosition { get; set; }
        public Position ExitPosition { get; set; }
        public List<Position> PatrolPoints { get; set; } = new List<Position>();
    }

    public class EnemyData
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int? PatrolRow { get; set; }
        public int? PatrolColumn { get; set; }
    }

    public class MovingWallData
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }

    public class Position
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }
    public static class Mapfile
    {
        public static List<Enemy> enemies = new List<Enemy>();
        public static List<MovingWalls> movingWalls = new List<MovingWalls>();
        public static Player player;
        public static FunObject exit;

        public static FunObject[,] FileMap(string filePath)
        {           
            string extension = Path.GetExtension(filePath).ToLower();

            if (extension == ".json")
            {
                return LoadFromJson(filePath);
            }
            else
            {
                return LoadFromText(filePath);
            }
        }

        private static FunObject[,] LoadFromText(string filePath)
        {
            enemies.Clear();
            movingWalls.Clear();
            String[] lines = File.ReadAllLines(filePath);
            string firstLine = lines[0];
            int rows = lines.Length;
            int cols = firstLine.Length;
            var toP = new List<int>();

            FunObject[,] map = new FunObject[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                string line = lines[i];
                for (int j = 0; j < cols; j++)
                {
                    char c = line[j];
                    switch (c)
                    {
                        case '-':
                            map[i, j] = new Walls(i, j, '-');
                            break;
                        case '|':
                            map[i, j] = new Walls(i, j, '|');
                            break;
                        case '"':
                            map[i, j] = new Walls(i, j, '"');
                            break;
                        case '.':
                            map[i, j] = new Walls(i, j, '.');
                            break;
                        case ':':
                            map[i, j] = new Walls(i, j, ':');
                            break;
                        case 'T':
                            map[i, j] = new Trap(i, j);
                            break;
                        case 'P':
                            map[i, j] = new Player(i, j);
                            player = new Player(i, j);
                            break;
                        case 'E':
                            map[i, j] = new Enemy(i, j);
                            enemies.Add(new Enemy(i, j));
                            break;
                        case 'I':
                            map[i, j] = new FunObject(i, j, 'D', 'I');
                            break;
                        case 'L':
                            map[i, j] = new FunObject(i, j, 'D', 'L');
                            exit = new FunObject(i, j);
                            break;
                        case '1':
                            toP.Add(i);
                            toP.Add(j);
                            map[i, j] = new FunObject(i, j);
                            break;
                        case '2':
                            toP.Add(i);
                            toP.Add(j);
                            map[i, j] = new FunObject(i, j);
                            break;
                        case 'B':
                            map[i, j] = new MovingWalls(i, j);
                            break;
                        default:
                            map[i, j] = new FunObject(i, j);
                            break;
                    }
                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                if (toP.Count >= 2)
                {
                    enemies[i].pd = toP[0];
                    enemies[i].kd = toP[1];
                    toP.RemoveAt(0);
                    toP.RemoveAt(0);
                }
            }

            return map;
        }

        private static FunObject[,] LoadFromJson(string filePath)
        {
            enemies.Clear();
            
                        
            string jsonContent = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // This handles both camelCase and PascalCase
            };

            MapData mapData = JsonSerializer.Deserialize<MapData>(jsonContent, options);


            if (mapData == null)
            {
                throw new Exception("Failed to deserialize JSON file");
            }
            int rows = mapData.Rows;
            int cols = mapData.Columns;
            FunObject[,] map = new FunObject[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    char c = mapData.Grid[i][j];
                    map[i, j] = CreateMapObject(i, j, c);
                }
            }
      
            for (int i = 0; i < mapData.Enemies.Count; i++)
            {
                var enemyData = mapData.Enemies[i];
                var enemy = new Enemy(enemyData.Row, enemyData.Column);

                if (enemyData.PatrolRow.HasValue && enemyData.PatrolColumn.HasValue)
                {
                    enemy.pd = enemyData.PatrolRow.Value;
                    enemy.kd = enemyData.PatrolColumn.Value;
                }
                else if (i < mapData.PatrolPoints.Count)
                {
                    enemy.pd = mapData.PatrolPoints[i].Row;
                    enemy.kd = mapData.PatrolPoints[i].Column;
                }

                enemies.Add(enemy);
                map[enemyData.Row, enemyData.Column] = enemy;
            }



            if (mapData.PlayerPosition != null)
            {
                player = new Player(mapData.PlayerPosition.Row, mapData.PlayerPosition.Column);
                map[mapData.PlayerPosition.Row, mapData.PlayerPosition.Column] = player;
            }

            if (mapData.ExitPosition != null)
            {
                exit = new FunObject(mapData.ExitPosition.Row, mapData.ExitPosition.Column, 'D', 'L');
                map[mapData.ExitPosition.Row, mapData.ExitPosition.Column] = exit;
            }

            return map;
        }

        private static FunObject CreateMapObject(int row, int col, char type)
        {
            switch (type)
            {
                case '-':
                case '|':
                case '"':
                case '.':
                case ':':
                    return new Walls(row, col, type);
                case 'T':
                    return new Trap(row, col);
                case 'P':
                    return new Player(row, col);
                case 'E':
                    return new Enemy(row, col);
                case 'I':
                    return new FunObject(row, col, 'D', 'I');
                case 'L':
                    return new FunObject(row, col, 'D', 'L');
                case 'B':
                    return new MovingWalls(row, col);
                default:
                    return new FunObject(row, col);
            }
        }

        public static void ConvertTextToJson(string textFilePath, string jsonFilePath)
        {
            string[] lines = File.ReadAllLines(textFilePath);
            int rows = lines.Length;
            int cols = lines[0].Length;

            MapData mapData = new MapData
            {
                Rows = rows,
                Columns = cols,
                Grid = new char[rows][]
            };

            var patrolPoints = new List<Position>();

            for (int i = 0; i < rows; i++)
            {
                mapData.Grid[i] = new char[cols];
                for (int j = 0; j < cols; j++)
                {
                    char c = lines[i][j];
                    mapData.Grid[i][j] = c;

                    switch (c)
                    {
                        case 'P':
                            mapData.PlayerPosition = new Position { Row = i, Column = j };
                            break;
                        case 'E':
                            mapData.Enemies.Add(new EnemyData { Row = i, Column = j });
                            break;
                        case 'L':
                            mapData.ExitPosition = new Position { Row = i, Column = j };
                            break;
                        case '1':
                        case '2':
                            patrolPoints.Add(new Position { Row = i, Column = j });
                            break;
                        case 'B':
                            mapData.MovingWalls.Add(new MovingWallData { Row = i, Column = j });
                            break;
                    }
                }
            }


            for (int i = 0; i < mapData.Enemies.Count && i < patrolPoints.Count; i++)
            {
                mapData.Enemies[i].PatrolRow = patrolPoints[i].Row;
                mapData.Enemies[i].PatrolColumn = patrolPoints[i].Column;
            }

            for (int i = mapData.Enemies.Count; i < patrolPoints.Count; i++)
            {
                mapData.PatrolPoints.Add(patrolPoints[i]);
            }
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string json = JsonSerializer.Serialize(mapData, options);
            File.WriteAllText(jsonFilePath, json);
        }

        public static List<Enemy> EnemiesList => enemies;

        public static Player GetPlayer() => player;
    }


}

