using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapPathfinder : MonoBehaviour
{
    Hashtable _obstacles;
    Node _start, _end; 
    public int maxStepsPathFinding = 5000;

    public Tilemap map;
    public Tile defaultTile;
    public Camera cam;
    
    public float tileSizeX;
    public float tileSizeY;
    public float spaceBetweenTiles;
    
    void Start()
    {
        _obstacles = new Hashtable();
        _start = new Node { Coord = int2.zero, Parent = int2.zero, GScore = int.MaxValue, HScore = int.MaxValue };
        _end = new Node { Coord = int2.zero, Parent = int2.zero, GScore = int.MaxValue, HScore = int.MaxValue };
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            PlaceStart();
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
        {
            PlaceEnd();
        }

        if (Input.GetMouseButtonDown(0) &&
            !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
        {
            PlaceObstacle();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearTiles();

            float startTime = Time.realtimeSinceStartup;

            FindPath();

            float endTime = Time.realtimeSinceStartup;
            Debug.Log(endTime - startTime);
        }

    }

    void ClearTiles()
    {
        map.ClearAllTiles();

        // Учитываем размеры тайлов и отступы для стартового тайла
        Vector3Int _startPos = new Vector3Int(_start.Coord.x * (int)tileSizeX + (int)spaceBetweenTiles, _start.Coord.y * (int)tileSizeY + (int)spaceBetweenTiles, 0);
        map.SetTile(_startPos, defaultTile);
        map.SetTileFlags(_startPos, TileFlags.None);
        map.SetColor(_startPos, Color.green);

        // Учитываем размеры тайлов и отступы для конечного тайла
        Vector3Int _endPos = new Vector3Int(_end.Coord.x * (int)tileSizeX + (int)spaceBetweenTiles, _end.Coord.y * (int)tileSizeY + (int)spaceBetweenTiles, 0);
        map.SetTile(_endPos, defaultTile);
        map.SetTileFlags(_endPos, TileFlags.None);
        map.SetColor(_endPos, Color.red);

        foreach (int2 o in _obstacles.Keys)
        {
            // Учитываем размеры тайлов и отступы для препятствий
            Vector3Int obstaclePos = new Vector3Int(o.x * (int)tileSizeX + (int)spaceBetweenTiles, o.y * (int)tileSizeY + (int)spaceBetweenTiles, 0);
            map.SetTile(obstaclePos, defaultTile);
            map.SetTileFlags(obstaclePos, TileFlags.None);
            map.SetColor(obstaclePos, Color.black);
        }
    }

    void PlaceStart()
    {
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mouseCell = map.WorldToCell(mouseWorldPos);

        int2 coord = new int2 { 
            x = Mathf.RoundToInt(mouseCell.x / tileSizeX),
            y = Mathf.RoundToInt(mouseCell.y / tileSizeY)
        };

        if (!_obstacles.ContainsKey(coord) && !coord.Equals(_end.Coord))
        {
            _start.Coord = coord;
            Vector3Int tilePos = new Vector3Int((int) (coord.x * tileSizeX), (int) (coord.y * tileSizeY), 0);
            map.SetTile(tilePos, defaultTile);
            map.SetTileFlags(tilePos, TileFlags.None);
            map.SetColor(tilePos, Color.green);
        }
    }

    void PlaceEnd()
    {
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mouseCell = map.WorldToCell(mouseWorldPos);

        // Учитываем размеры тайлов и отступы
        int2 coord = new int2 { 
            x = Mathf.RoundToInt(mouseCell.x / tileSizeX),
            y = Mathf.RoundToInt(mouseCell.y / tileSizeY)
        };

        if (!_obstacles.ContainsKey(coord) && !coord.Equals(_start.Coord))
        {
            _end.Coord = coord;
            // Учитываем размеры тайлов и отступы
            Vector3Int tilePos = new Vector3Int((int) (coord.x * tileSizeX), (int) (coord.y * tileSizeY), 0);
            map.SetTile(tilePos, defaultTile);
            map.SetTileFlags(tilePos, TileFlags.None);
            map.SetColor(tilePos, Color.red);
        }
    }

    void PlaceObstacle()
    {
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mouseCell = map.WorldToCell(mouseWorldPos);

        // Учитываем размеры тайлов и отступы
        int2 coord = new int2 { 
            x = Mathf.RoundToInt(mouseCell.x / tileSizeX),
            y = Mathf.RoundToInt(mouseCell.y / tileSizeY)
        };

        if (_obstacles.ContainsKey(coord))
        {
            // Учитываем размеры тайлов и отступы
            Vector3Int tilePos = new Vector3Int((int) (coord.x * tileSizeX), (int) (coord.y * tileSizeY), 0);
            map.SetTile(tilePos, null);
            _obstacles.Remove(coord);
        }
        else if (!coord.Equals(_start.Coord) && !coord.Equals(_end.Coord))
        {
            _obstacles.Add(coord, true);
            // Учитываем размеры тайлов и отступы
            Vector3Int tilePos = new Vector3Int((int) (coord.x * tileSizeX), (int) (coord.y * tileSizeY), 0);
            map.SetTile(tilePos, defaultTile);
            map.SetTileFlags(tilePos, TileFlags.None);
            map.SetColor(tilePos, Color.black);
        }
    }

    public void FindPath()
    {
        NativeHashMap<int2, bool> isObstacle = new(_obstacles.Count, Allocator.TempJob);
        NativeHashMap<int2, Node> nodes = new(maxStepsPathFinding, Allocator.TempJob);
        NativeHashMap<int2, Node> openSet = new(maxStepsPathFinding, Allocator.TempJob);
        NativeArray<int2> offsets = new(6, Allocator.TempJob);

        foreach (int2 o in _obstacles.Keys)
        {
            isObstacle.Add(o, true);
        }

        AStarHex aStar = new()
        {
            IsObstacle = isObstacle,
            Offsets = offsets,
            Nodes = nodes,
            OpenSet = openSet,
            Start = _start,
            End = _end,
            SafeGuard = maxStepsPathFinding
        };

        JobHandle handle = aStar.Schedule();
        handle.Complete();
        
        NativeArray<Node> nodeArray = nodes.GetValueArray(Allocator.TempJob);
        for (int i = 0; i < nodeArray.Length; i++)
        {
            Vector3Int currentNode = new Vector3Int(nodeArray[i].Coord.x * (int)tileSizeX + (int)spaceBetweenTiles, nodeArray[i].Coord.y * (int)tileSizeY + (int)spaceBetweenTiles, 0);

            if (_start.Coord.Equals(nodeArray[i].Coord) ||
                _end.Coord.Equals(nodeArray[i].Coord) ||
                isObstacle.ContainsKey(nodeArray[i].Coord)) continue;
            map.SetTile(currentNode, defaultTile);
            map.SetTileFlags(currentNode, TileFlags.None);
            map.SetColor(currentNode, Color.white);
        }
        
        if (nodes.ContainsKey(_end.Coord))
        {
            int2 currentCoord = _end.Coord;

            while (!currentCoord.Equals(_start.Coord))
            {
                currentCoord = nodes[currentCoord].Parent;
                Vector3Int currentTile = new(currentCoord.x, currentCoord.y, 0);

                map.SetTile(currentTile, defaultTile);
                map.SetTileFlags(currentTile, TileFlags.None);
                map.SetColor(currentTile, Color.green);
            }
        }

        nodeArray.Dispose();
        nodes.Dispose();
        openSet.Dispose();
        isObstacle.Dispose();
        offsets.Dispose();
    }

    public struct AStarHex : IJob
    {
        public NativeHashMap<int2, bool> IsObstacle;
        public NativeHashMap<int2, Node> Nodes;
        public NativeHashMap<int2, Node> OpenSet;
        public NativeArray<int2> Offsets;

        public Node Start;
        public Node End;

        public int SafeGuard;

        public void Execute()
        {
            Node current = Start;
            current.GScore = 0;
            current.HScore = HexDistance(current.Coord, End.Coord);
            OpenSet.TryAdd(current.Coord, current);

            // Смещения для соседей в гексагональной сетке flat-top
            Offsets[0] = new int2(0, 1);
            Offsets[1] = new int2(1, 0);
            Offsets[2] = new int2(1, -1);
            Offsets[3] = new int2(0, -1);
            Offsets[4] = new int2(-1, 0);
            Offsets[5] = new int2(-1, 1);

            int counter = 0;

            do
            {
                current = OpenSet[ClosestNode()];
                Nodes.TryAdd(current.Coord, current);

                foreach (var t in Offsets)
                {
                    int2 neighborCoord = current.Coord + t;

                    if (Nodes.ContainsKey(neighborCoord) ||
                        IsObstacle.ContainsKey(neighborCoord)) continue;
            
                    Node neighbour = new()
                    {
                        Coord = neighborCoord,
                        Parent = current.Coord,
                        GScore = current.GScore + HexDistance(current.Coord, neighborCoord), 
                        HScore = HexDistance(neighborCoord, End.Coord) 
                    };

                    if (OpenSet.ContainsKey(neighbour.Coord) && neighbour.GScore < OpenSet[neighbour.Coord].GScore)
                    {
                        OpenSet[neighbour.Coord] = neighbour;
                    }
                    else if (!OpenSet.ContainsKey(neighbour.Coord))
                    {
                        OpenSet.TryAdd(neighbour.Coord, neighbour);
                    }
                }

                OpenSet.Remove(current.Coord);
                counter++;

                if (counter > SafeGuard)
                    break;

            } while (OpenSet.Count() != 0 && !current.Coord.Equals(End.Coord));
        }


        private int HexDistance(int2 coordA, int2 coordB)
        {
            int2 delta = math.abs(coordA - coordB);
            return math.max(delta.x, delta.y) + (math.min(delta.x, delta.y) + 1) / 2; 
        }

        private int2 ClosestNode()
        {
            Node result = new Node();
            int fScore = int.MaxValue;

            NativeArray<Node> nodeArray = OpenSet.GetValueArray(Allocator.Temp);

            for (int i = 0; i < nodeArray.Length; i++)
            {
                if (nodeArray[i].GScore + nodeArray[i].HScore < fScore)
                {
                    result = nodeArray[i];
                    fScore = nodeArray[i].GScore + nodeArray[i].HScore;
                }
            }

            nodeArray.Dispose();
            return result.Coord;
        }
    }
    
    public struct Node
    {
        public int2 Coord;
        public int2 Parent;
        public int GScore;
        public int HScore;
    }
}