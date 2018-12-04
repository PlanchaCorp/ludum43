using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour {
    MessageData level;
    enum Direction { Up, Right, Down, Left }
    bool[,] roomPresence, roomsFilledWithBook;
    int mapWidth, mapHeight;
    int startX, startY, endX, endY;
    int bookCount;
    float roomSide;
    private GameObject[] levelSections;
    private GameObject spawnSection;
    private GameObject[,] rooms;
    private Tilemap carpetTilemap;
    // Use this for initialization

    private void Awake()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        if(controller != null)
        {
            level = controller.GetComponent<Manager>().GetCurrentLevel();
            GameObject.FindGameObjectWithTag("Objectives").GetComponent<ObjectiveManager>().messageData = level;
        } else
        {
            Debug.Log("No GameController");
            level = new MessageData();
            level.sizeX = 5;
            level.SizeY = 5;
            level.minRoomQuantity = 10;
            level.maxRoomQuantity = 20;
            level.Dialog = new List<string>();
        }
       
    }
    void Start () {
        levelSections = Resources.LoadAll<GameObject>("Prefabs/LevelSection");
        spawnSection = Resources.Load<GameObject>("Prefabs/LevelSection/Empty");
        GenerateStructure(level.sizeX, level.SizeY, level.minRoomQuantity, level.maxRoomQuantity, level.Dialog.Count);
        LoadRandomRooms();
        LoadRandomBooks();
        TryTeleportingPlayer();
        CreateEntrances();
        AstarPath.active.Scan();
    }

    void GenerateStructure(int width, int height, int minSize, int maxSize, int bookCountParameter = 0)
    {
        mapWidth = width;
        mapHeight = height;
        bookCount = bookCountParameter;
        rooms = new GameObject[mapWidth, mapHeight];
        int roomDiscovered;
        do
        {
            // Initializing room presence array
            roomPresence = new bool[mapWidth, mapHeight];
            roomsFilledWithBook = new bool[mapWidth, mapHeight];
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    roomPresence[x,y] = false;
                    roomsFilledWithBook[x,y] = false;
                }
            }
            // Selecting starting point
            startX = Random.Range(0, mapWidth);
            startY = Random.Range(0, mapHeight);
            roomPresence[startX, startY] = true;
            // Selecting ending point
            do
            {
                endX = Random.Range(0, 2) * (mapWidth - 1);
                endY = Random.Range(0, 2) * (mapHeight - 1);
            } while (endX == startX && endY == startY);
            // Drunken man algorithm
            roomDiscovered = 1;
            int drunkX = startX, drunkY = startY;
            while ((drunkX != endX || drunkY != endY) && roomDiscovered <= maxSize)
            {
                Direction direction = (Direction)Random.Range(0, 4);
                int x = drunkX, y = drunkY;
                switch (direction)
                {
                    case Direction.Up:
                        y++;
                        break;
                    case Direction.Right:
                        x++;
                        break;
                    case Direction.Down:
                        y--;
                        break;
                    case Direction.Left:
                        x--;
                        break;
                }
                if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
                {
                    drunkX = x;
                    drunkY = y;
                    if (!roomPresence[x,y])
                    {
                        roomPresence[x, y] = true;
                        roomDiscovered++;
                    }
                }
            }
        } while (roomDiscovered < minSize || roomDiscovered > maxSize);
    }

    private void LoadRandomRooms()
    {
        roomSide = levelSections[0].GetComponentsInChildren<Tilemap>()[0].size.x;
        for (int x = 0; x < roomPresence.GetLength(0); x++)
        {
            for (int y = 0; y < roomPresence.GetLength(1); y++)
            {
                if (roomPresence[x,y])
                {
                    GameObject newSection;
                    if (x == startX && y == startY)
                    {
                        newSection = spawnSection;
                    } else
                    {
                        do
                        {
                            newSection = levelSections[Random.Range(0, levelSections.Length)];
                        } while (newSection.name == "Empty");
                    }
                    rooms[x,y] = Instantiate(newSection);
                    rooms[x, y].transform.position = new Vector2(x * roomSide, y * roomSide);
                }
            }
        }
    }

    private void LoadRandomBooks()
    {
        int i = 0;
        while (i < bookCount)
        {
            int randX = Random.Range(0, mapWidth), randY = Random.Range(0, mapHeight);
            if (roomPresence[randX, randY] && !roomsFilledWithBook[randX, randY] && rooms[randX,randY].name != "Empty")
            {
                roomsFilledWithBook[randX, randY] = true;
                GameObject desk = Instantiate(Resources.Load<GameObject>("Prefabs/Desk"));
                foreach (Transform roomElement in rooms[randX, randY].transform)
                {
                    if (roomElement.tag == "DeskSpawner")
                    {
                        desk.transform.position = roomElement.position;
                    }
                }
                i++;
            }
        }
    }

    private void TryTeleportingPlayer()
    {
        GameObject playerAndCamera = GameObject.FindGameObjectWithTag("PlayerAndCamera");
        if (playerAndCamera != null)
        {
            playerAndCamera.transform.position = new Vector3(startX * roomSide, startY * roomSide);
            Transform[] playerChildren = playerAndCamera.GetComponentsInChildren<Transform>();
            for (int i = 0; i < playerChildren.Length; i++)
            {
                playerChildren[i].position = new Vector3(startX * roomSide, startY * roomSide);
            }
        }
    }

    private void CreateEntrances()
    {
        for (int x = 0; x < roomPresence.GetLength(0); x++)
        {
            for (int y = 0; y < roomPresence.GetLength(1); y++)
            {
                bool leftDirectionAvailable = true, rightDirectionAvailable = true, upDirectionAvailable = true, downDirectionAvailable = true;
                if (roomPresence[x,y])
                {
                    Tilemap wallTilemap = rooms[x, y].GetComponentsInChildren<Tilemap>()[1];
                    if (x - 1 >= 0 && roomPresence[x - 1, y])
                    {
                        wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector3(-11, 0, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                        wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector3(-12, 0, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                        wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector3(-11, -1, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                        wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector3(-12, -1, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                        leftDirectionAvailable = false;
                    }
                    if (x + 1 < mapWidth && roomPresence[x + 1, y])
                    {
                        wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector3(10, 0, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                        wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector3(11, 0, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                        wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector3(10, -1, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                        wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector3(11, -1, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                        rightDirectionAvailable = false;
                    }
                    if (y - 1 >= 0 && roomPresence[x, y - 1])
                    {
                        wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector3(0, -11, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                        wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector3(0, -12, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                        wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector3(-1, -11, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                        wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector3(-1, -12, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                        downDirectionAvailable = false;
                    }
                    if (y + 1 < mapHeight && roomPresence[x, y + 1])
                    {
                        wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector3(0, 10, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                        wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector3(0, 11, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                        wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector3(-1, 10, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                        wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector3(-1, 11, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                        upDirectionAvailable = false;
                    }
                    if (x == endX && y == endY)
                    {
                        GameObject carpetTilemapObject = GameObject.Instantiate(new GameObject());
                        carpetTilemapObject.transform.parent = rooms[x, y].gameObject.transform;
                        Tilemap carpetTilemap = carpetTilemapObject.AddComponent<Tilemap>();
                        carpetTilemapObject.name = "CarpetTilemap";
                        carpetTilemapObject.AddComponent<TilemapRenderer>().sortingLayerName = "Ground";
                        carpetTilemapObject.AddComponent<Exit>();
                        carpetTilemapObject.AddComponent<TilemapCollider2D>().isTrigger = true;
                        carpetTilemapObject.transform.localPosition = Vector3.zero;
                        Direction randomDirection;
                        bool directionIsAvailable = false;
                        do
                        {
                            randomDirection = (Direction)Random.Range(0, 4);
                            switch (randomDirection)
                            {
                                case Direction.Up:
                                    directionIsAvailable = upDirectionAvailable;
                                    break;
                                case Direction.Right:
                                    directionIsAvailable = rightDirectionAvailable;
                                    break;
                                case Direction.Down:
                                    directionIsAvailable = downDirectionAvailable;
                                    break;
                                case Direction.Left:
                                    directionIsAvailable = leftDirectionAvailable;
                                    break;
                            }
                        } while (!directionIsAvailable);
                        switch (randomDirection)
                        {
                            case Direction.Up:
                                wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector2(-1, 10)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                                wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector2(0, 10)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                                carpetTilemap.SetTile(wallTilemap.LocalToCell(new Vector2(-1, 10)), Resources.Load<Tile>("Tiles/Carpet_0"));
                                carpetTilemap.SetTile(wallTilemap.LocalToCell(new Vector2(0, 10)), Resources.Load<Tile>("Tiles/Carpet_1"));
                                break;
                            case Direction.Right:
                                wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector2(10, -1)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                                wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector2(10, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                                carpetTilemap.SetTile(wallTilemap.LocalToCell(new Vector2(10, -1)), Resources.Load<Tile>("Tiles/Carpet_V_1"));
                                carpetTilemap.SetTile(wallTilemap.LocalToCell(new Vector2(10, 0)), Resources.Load<Tile>("Tiles/Carpet_V_0"));
                                break;
                            case Direction.Down:
                                wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector2(-1, -11)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                                wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector2(0, -11)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                                carpetTilemap.SetTile(wallTilemap.LocalToCell(new Vector2(-1, -11)), Resources.Load<Tile>("Tiles/Carpet_0"));
                                carpetTilemap.SetTile(wallTilemap.LocalToCell(new Vector2(0, -11)), Resources.Load<Tile>("Tiles/Carpet_1"));
                                break;
                            case Direction.Left:
                                wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector2(-11, -1)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                                wallTilemap.SetTile(wallTilemap.LocalToCell(new Vector2(-11, 0)), (Tile)ScriptableObject.CreateInstance(typeof(Tile)));
                                carpetTilemap.SetTile(wallTilemap.LocalToCell(new Vector2(-11, -1)), Resources.Load<Tile>("Tiles/Carpet_V_1"));
                                carpetTilemap.SetTile(wallTilemap.LocalToCell(new Vector2(-11, 0)), Resources.Load<Tile>("Tiles/Carpet_V_0"));
                                break;
                        }
                    }
                }
            }
        }
    }
}
