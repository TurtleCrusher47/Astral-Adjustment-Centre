using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using RandomR = UnityEngine.Random;
using Graphs;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using System;
using static UnityEditor.FilePathAttribute;
using System.Drawing;
using Unity.VisualScripting;
using System.IO;
using static UnityEngine.GraphicsBuffer;

public class Generator3D : MonoBehaviour {
    enum CellType {
        None,
        Room,
        Hallway,
        Stairs
    }

    class Room {
        public BoundsInt bounds;

        public Room(Vector3Int location, Vector3Int size) {
            bounds = new BoundsInt(location, size);
        }

        public static bool Intersect(Room a, Room b) {
            return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
                || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y)
                || (a.bounds.position.z >= (b.bounds.position.z + b.bounds.size.z)) || ((a.bounds.position.z + a.bounds.size.z) <= b.bounds.position.z));
        }
    }

    [SerializeField]
    int seed;
    [SerializeField]
    Vector3Int size;
    [SerializeField]
    int roomCount;
    [SerializeField]
    Vector3Int roomMaxSize;
    [SerializeField]
    Vector3Int roomMinSize;
    [SerializeField]
    GameObject floorPrefab;
    [SerializeField]
    GameObject ceilingPrefab;
    [SerializeField]
    GameObject wallPrefab;
    [SerializeField]
    GameObject wallDoorPrefab;
    [SerializeField]
    GameObject hallwayPrefab;
    [SerializeField]
    GameObject stairPrefab;

    Random random;
    Grid3D<CellType> grid;
    List<Room> rooms;
    Delaunay3D delaunay;
    HashSet<Prim.Edge> selectedEdges;
    List<List<Vector3Int>> pathList;

    void Start()
    {
        pathList = new List<List<Vector3Int>>();
        ChangeSeed();
        InitializeMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnityEngine.SceneManagement.Scene s = SceneManager.GetActiveScene();
            foreach(GameObject go in s.GetRootGameObjects())
            {
                if (go.name.Contains("cube") || go.name.Contains("Brick"))
                {
                    Destroy(go);
                }
            }

            InitializeMap();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeSeed();
        }
    }

    private void ChangeSeed()
    {
        seed = RandomR.Range(10000000, 99999999);
    }

    private void InitializeMap()
    {
        random = new Random(seed);
        grid = new Grid3D<CellType>(size, Vector3Int.zero);
        rooms = new List<Room>();

        PlaceRooms();
        Triangulate();
        CreateHallways();
        PathfindHallways();
        DeleteWalls();
    }

    void PlaceRooms() {
        for (int i = 0; i < roomCount; i++) {

            Vector3Int roomSize = new Vector3Int(
                random.Next(roomMinSize.x + 1, roomMaxSize.x + 1),
                random.Next(roomMinSize.y + 1, roomMaxSize.y + 1),
                random.Next(roomMinSize.z + 1, roomMaxSize.z + 1)
            );

            Vector3Int location = new Vector3Int(
                random.Next(0, size.x),
                random.Next(0, size.y),
                random.Next(0, size.z)
            );

            bool add = true;
            Room newRoom = new Room(location, roomSize);
            Room buffer = new Room(location + new Vector3Int(-1, 0, -1), roomSize + new Vector3Int(2, 0, 2));

            foreach (var room in rooms) {
                if (Room.Intersect(room, buffer)) {
                    add = false;
                    break;
                }
            }

            if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= size.x
                || newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= size.y
                || newRoom.bounds.zMin < 0 || newRoom.bounds.zMax >= size.z) {
                add = false;
            }

            if (add) {
                rooms.Add(newRoom);
                PlaceRoom(newRoom.bounds.position, newRoom.bounds.size);

                foreach (var pos in newRoom.bounds.allPositionsWithin) {
                    grid[pos] = CellType.Room;
                }
            }
        }
    }

    void Triangulate() {
        List<Vertex> vertices = new List<Vertex>();

        foreach (var room in rooms) {
            vertices.Add(new Vertex<Room>((Vector3)room.bounds.position + ((Vector3)room.bounds.size) / 2, room));
        }

        delaunay = Delaunay3D.Triangulate(vertices);
    }

    void CreateHallways() {
        List<Prim.Edge> edges = new List<Prim.Edge>();

        foreach (var edge in delaunay.Edges) {
            edges.Add(new Prim.Edge(edge.U, edge.V));
        }

        List<Prim.Edge> minimumSpanningTree = Prim.MinimumSpanningTree(edges, edges[0].U);

        selectedEdges = new HashSet<Prim.Edge>(minimumSpanningTree);
        var remainingEdges = new HashSet<Prim.Edge>(edges);
        remainingEdges.ExceptWith(selectedEdges);

        foreach (var edge in remainingEdges) {
            if (random.NextDouble() < 0.5) {
                selectedEdges.Add(edge);
            }
        }
    }

    void PathfindHallways() {
        DungeonPathfinder3D aStar = new DungeonPathfinder3D(size);

        foreach (var edge in selectedEdges) {
            var startRoom = (edge.U as Vertex<Room>).Item;
            var endRoom = (edge.V as Vertex<Room>).Item;

            var startPosf = startRoom.bounds.center;
            var endPosf = endRoom.bounds.center;
            var startPos = new Vector3Int((int)startPosf.x, (int)(startPosf.y - 0.5f), (int)startPosf.z);
            var endPos = new Vector3Int((int)endPosf.x, (int)(endPosf.y - 0.5f), (int)endPosf.z);

            var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder3D.Node a, DungeonPathfinder3D.Node b) => {
                var pathCost = new DungeonPathfinder3D.PathCost();

                var delta = b.Position - a.Position;

                if (delta.y == 0) {
                    //flat hallway
                    pathCost.cost = Vector3Int.Distance(b.Position, endPos);    //heuristic

                    if (grid[b.Position] == CellType.Stairs) {
                        return pathCost;
                    } else if (grid[b.Position] == CellType.Room) {
                        pathCost.cost += 5;
                    } else if (grid[b.Position] == CellType.None) {
                        pathCost.cost += 1;
                    }

                    pathCost.traversable = true;
                } else {
                    //staircase
                    if ((grid[a.Position] != CellType.None && grid[a.Position] != CellType.Hallway)
                        || (grid[b.Position] != CellType.None && grid[b.Position] != CellType.Hallway)) return pathCost;

                    pathCost.cost = 100 + Vector3Int.Distance(b.Position, endPos);    //base cost + heuristic

                    int xDir = Mathf.Clamp(delta.x, -1, 1);
                    int zDir = Mathf.Clamp(delta.z, -1, 1);
                    Vector3Int verticalOffset = new Vector3Int(0, delta.y, 0);
                    Vector3Int horizontalOffset = new Vector3Int(xDir, 0, zDir);

                    if (!grid.InBounds(a.Position + verticalOffset)
                        || !grid.InBounds(a.Position + horizontalOffset)
                        || !grid.InBounds(a.Position + verticalOffset + horizontalOffset)) {
                        return pathCost;
                    }

                    if (grid[a.Position + horizontalOffset] != CellType.None
                        || grid[a.Position + horizontalOffset * 2] != CellType.None
                        || grid[a.Position + verticalOffset + horizontalOffset] != CellType.None
                        || grid[a.Position + verticalOffset + horizontalOffset * 2] != CellType.None) {
                        return pathCost;
                    }

                    pathCost.traversable = true;
                    pathCost.isStairs = true;
                }

                return pathCost;
            });

            if (path != null) {

                pathList.Add(path);

                for (int i = 0; i < path.Count; i++) {
                    var current = path[i];

                    if (grid[current] == CellType.None) {
                        grid[current] = CellType.Hallway;
                    }

                    if (i > 0) {
                        var prev = path[i - 1];

                        var delta = current - prev;

                        if (delta.y != 0) {
                            int xDir = Mathf.Clamp(delta.x, -1, 1);
                            int zDir = Mathf.Clamp(delta.z, -1, 1);
                            Vector3Int verticalOffset = new Vector3Int(0, delta.y, 0);
                            Vector3Int horizontalOffset = new Vector3Int(xDir, 0, zDir);
                            
                            grid[prev + horizontalOffset] = CellType.Stairs;
                            grid[prev + horizontalOffset * 2] = CellType.Stairs;
                            grid[prev + verticalOffset + horizontalOffset] = CellType.Stairs;
                            grid[prev + verticalOffset + horizontalOffset * 2] = CellType.Stairs;

                            // spawn stairs
                            // stair landing
                            // stair
                            Vector3 tilePos1 = prev + (Vector3)horizontalOffset * 1.5f + new Vector3(0.5f, -1.5f, 0.5f);
                            Vector3 tilePos2 = prev + verticalOffset + (Vector3)horizontalOffset * 1.5f + new Vector3(0.5f, -1.5f, 0.5f);
                            // going up
                            if (delta.y > 0)
                            {
                                if (delta.x > 0)
                                {
                                    SpawnTileWithRotation(stairPrefab, tilePos1, -90);
                                }
                                else if (delta.x < 0)
                                {
                                    SpawnTileWithRotation(stairPrefab, tilePos1, 90);
                                }
                                else if (delta.z > 0)
                                {
                                    SpawnTileWithRotation(stairPrefab, tilePos1, 180);
                                }
                                else if (delta.z < 0)
                                {
                                    SpawnTileWithRotation(stairPrefab, tilePos1, 0);
                                }
                            }
                            // goind down
                            else if (delta.y < 0)
                            {
                                if (delta.x > 0)
                                {
                                    SpawnTileWithRotation(stairPrefab, tilePos2, 90);
                                }
                                else if (delta.x < 0)
                                {
                                    SpawnTileWithRotation(stairPrefab, tilePos2, -90);
                                }
                                else if (delta.z > 0)
                                {
                                    SpawnTileWithRotation(stairPrefab, tilePos2, 0);
                                }
                                else if (delta.z < 0)
                                {
                                    SpawnTileWithRotation(stairPrefab, tilePos2, 180);
                                }
                            }
                        }
                    }
                }

                List<Vector3Int> tempList = new List<Vector3Int>();
                foreach (var pos in path) 
                {
                    Debug.Log(pos);
                    if (tempList.Count == 0)
                    {
                        bool check = true;
                        foreach (var p in tempList)
                        {
                            if (pos == p)
                            {
                                check = false;
                            }
                        }
                        if (grid[pos] == CellType.Hallway && check)
                        {
                            PlaceHallway(pos);
                            tempList.Add(pos);
                        }
                    }
                    else
                    {
                        if (grid[pos] == CellType.Hallway)
                        {
                            PlaceHallway(pos);
                            tempList.Add(pos);
                        }
                    }
                }
            }
        }
    }

    void DeleteWalls()
    {
        foreach (var path in pathList)
        {
            if (path != null)
            {
                Vector3 offset = new Vector3(0.25f, -1.0f, 0.5f);

                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(path[i] + offset, path[i + 1] + offset, UnityEngine.Color.blue, 1000, false);
                    RaycastHit[] hitList = Physics.RaycastAll(path[i] + offset, path[i + 1] - path[i], (path[i] - path[i + 1]).magnitude);
                    foreach (RaycastHit hit in hitList)
                    {
                        if (hit.transform.CompareTag("MapWallTile"))
                        {
                            if (hit.transform.parent == null)
                            {
                                SpawnTileWithRotation(wallDoorPrefab, hit.transform.position, hit.transform.eulerAngles.y);
                            }
                            Destroy(hit.transform.gameObject);
                        }
                    }
                }
                Debug.DrawLine(path[0] + offset, path[1] + offset, UnityEngine.Color.green, 1000, false);
                Debug.DrawLine(path[path.Count - 2] + offset, path[path.Count - 1] + offset, UnityEngine.Color.red, 1000, false);
            }
        }
    }

    void PlaceRoom(Vector3Int location, Vector3Int size) {
        //PlaceCube(location, size, redMaterial);

        for (int j = 0; j < size.x; j++)
        {
            for (int k = 0; k < size.y; k++)
            {
                for (int l = 0; l < size.z; l++)
                {
                    Vector3 tileOffset = new Vector3(j + 0.5f, k - 0.5f, l + 0.5f);
                    // spawn floor
                    if (k == 0)
                    {
                        Instantiate(floorPrefab, location + tileOffset, Quaternion.identity);
                    }
                    // spawn ceiling
                    else if (k == size.y - 1)
                    {
                        Instantiate(ceilingPrefab, location + tileOffset, Quaternion.identity);
                    }
                    // spawn walls
                    if (k < size.y - 1)
                    {
                        if (j == 0)
                        {
                            SpawnTileWithRotation(wallPrefab, location + tileOffset + new Vector3(-0.5f, 0, 0), 90);
                        }
                        else if (j == size.x - 1)
                        {
                            SpawnTileWithRotation(wallPrefab, location + tileOffset + new Vector3(0.5f, 0, 0), -90);
                        }
                        if (l == 0)
                        {
                            SpawnTileWithRotation(wallPrefab, location + tileOffset + new Vector3(0, 0, -0.5f), 0);
                        }
                        else if (l == size.z - 1)
                        {
                            SpawnTileWithRotation(wallPrefab, location + tileOffset + new Vector3(0, 0, 0.5f), 180);
                        }
                    }
                }
            }
        }
    }

    void PlaceHallway(Vector3Int curr) {
        Vector3 tileOffset = new Vector3(0.5f, -1.5f, 0.5f);
        // spawn floor
        Instantiate(hallwayPrefab, curr + tileOffset, Quaternion.identity);
    }

    void SpawnTileWithRotation(GameObject go, Vector3 location, float angle)
    {
        GameObject obj = Instantiate(go, location, Quaternion.identity);
        obj.transform.eulerAngles = new Vector3(0, angle, 0);
    }
}
