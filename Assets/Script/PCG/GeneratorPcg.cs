using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class GeneratorPcg : MonoBehaviour
{
    [SerializeField] private Tilemap ground;
    [SerializeField] private Tilemap fackWall;
    [SerializeField] private Tilemap wall;
    [SerializeField] private RuleTile fackWallTile;

    [FormerlySerializedAs("wallKTile")] [SerializeField]
    private Tile wallTile;

    [SerializeField] private List<TileAndWight> tiles;
    [SerializeField] private Vector3Int startPosition = Vector3Int.zero;
    [SerializeField] private int heightMax;
    [SerializeField] private int widthMax;
    [SerializeField] private GameObject escalier;
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject spawnerEnemy;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private List<SpawnEnnemie> _spawnEnnemies;

    [Header("Drunckard Settings")] [SerializeField]
    private int lMin;

    [SerializeField] private int lMax;
    [SerializeField] private int iterMax;
    [SerializeField] private int nbTilesMax;

    [Header("Seed Random")] [SerializeField]
    private bool isRandSeed;

    [SerializeField] private string seedHex;

    [Header("Game of Life Setting")] [SerializeField]
    private int fillIteration;

    //[SerializeField] private int deadLimitMax;
    //[SerializeField] private int deadLimitMin;
    [SerializeField] private int aliveLimit;

    private GameObject currentEscalier;
    public GameObject CurrentEscalier => currentEscalier;

    private Vector2Int[] _directions = new[]
    {
        Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
    };

    private BoundsInt _barrier;

    [Serializable]
    private struct TileAndWight
    {
        public Tile tile;
        public float wight;

        public TileAndWight(Tile tileSetting, float wightSetting)
        {
            tile = tileSetting;
            wight = wightSetting;
        }
    }

    public void StartGeneration()
    {
        var goodGenerate = false;
        do
        {
            goodGenerate = Generate();
            Debug.Log($"Generate : {goodGenerate}");
        } while (!goodGenerate);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        SetBarrier();
    }

    private void OnDrawGizmos()
    {
        SetBarrier();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_barrier.center, _barrier.size);
    }

    private void SetBarrier()
    {
        var size = new Vector3Int(widthMax, heightMax, 0);
        //Vector3Int boundsPosition = new Vector3Int(startPosition.x + widthMax / 2, startPosition.y + heightMax / 2, 0);
        var boundsPosition = new Vector3Int(0, 0, 0) - size / 2;

        _barrier = new BoundsInt(boundsPosition, size);
    }

    private bool Generate()
    {
        if (isRandSeed)
        {
            long ticks = DateTime.Now.Ticks;
            var seedInt = (uint)(ticks & 0xFFFFFFFF); // Force à rester dans 32 bits
            seedHex = seedInt.ToString("X"); // En hexadécimal
        }
        try
        {
            uint parsedSeed = Convert.ToUInt32(seedHex, 16); // base 16
            var seedInt = parsedSeed;
            Random.InitState((int)seedInt);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Seed hex invalide: {seedHex} — {e.Message}");
            return false;
        }


        ResetTilemap();

        GenerateDrunckard();

        GenerateFillWithLife();

        GenerateWall();

        var exitIsPlaced = false;

        var tryForPlaced = 0;

        do
        {
            var randx = Random.Range(_barrier.xMin, _barrier.xMax);
            var randy = Random.Range(_barrier.yMin, _barrier.yMax);

            var gridPos = new Vector3Int(randx, randy, 0);

            if (ground.HasTile(gridPos) && AllNeighborsHaveTilesForExitDoor(gridPos))
            {
                currentEscalier = Instantiate(escalier, gridPos, Quaternion.identity, grid.transform);
                exitIsPlaced = true;
            }
            tryForPlaced++;
            if (tryForPlaced > 100)
            {
                return false;
            }
        } while (!exitIsPlaced);

        PlaceSpawnPointEnemy();

        return true;
    }

    private void PlaceSpawnPointEnemy()
    {
        GameObject spawner = null;
        for (var i = 0; i < 3; i++)
        {
            var spawnsIsPlaced = false;
            var tryForPlaced = 0;
            
            do
            {
                var randx = Random.Range(_barrier.xMin, _barrier.xMax);
                var randy = Random.Range(_barrier.yMin, _barrier.yMax);

                var gridPos = new Vector3Int(randx, randy, 0);

                if (ground.HasTile(gridPos) && AllNeighborsHaveTilesForExitDoor(gridPos))
                {
                    if (spawner == null || gridPos == new Vector3Int((int)spawner.transform.position.x,(int)spawner.transform.position.y) || currentEscalier.transform.position == spawner.transform.position)
                    {
                        spawner = Instantiate(spawnerEnemy, gridPos, Quaternion.identity, grid.transform);
                        _spawnEnnemies.Add(spawner.GetComponent<SpawnEnnemie>()); 
                    }
                    spawnsIsPlaced = true;
                }
                tryForPlaced++;
                if (tryForPlaced > 100)
                {
                    break;
                }
            } while (!spawnsIsPlaced);
        }
        foreach (var spawnEnnemy in _spawnEnnemies)
        {
            spawnEnnemy.SetEnemyManager(_enemyManager);
        }
    }

    private bool AllNeighborsHaveTilesForSpawnerEnemy(Vector3Int center)
    {
        foreach (var dir in GenerateNeighbours(new Vector2Int(-3,-3),new Vector2Int(3,3)))
        {
            Vector3Int neighbor = center + dir;
            if (!ground.HasTile(neighbor))
                return false;
        }

        return true;
    }
    
    private bool AllNeighborsHaveTilesForExitDoor(Vector3Int center)
    {
        foreach (var dir in GenerateNeighbours(new Vector2Int(-1,-1),new Vector2Int(5,1)))
        {
            Vector3Int neighbor = center + dir;
            if (!ground.HasTile(neighbor))
                return false;
        }

        return true;
    }

    private List<Vector3Int> GenerateNeighbours(Vector2Int posMin, Vector2Int posMax)
    {
        var neighbours = new List<Vector3Int>();
        for (var x = posMin.x; x <= posMax.x; x++)
        {
            for (var y = posMin.y; y <= posMax.y; y++)
            {
                if (x == 0 && y == 0)
                {
                   continue;
                }
                neighbours.Add(new Vector3Int(x, y));
            }
        }
        return neighbours;
    }

    private void ResetTilemap()
    {
        ground.ClearAllTiles();
        wall.ClearAllTiles();
        fackWall.ClearAllTiles();
        if (currentEscalier != null)
        {
            Destroy(currentEscalier);
        }
        if (_spawnEnnemies.Count > 0)
        {
            foreach (var spawnEnnemy in _spawnEnnemies)
            {
                Destroy(spawnEnnemy.gameObject);
            }
        }
    }

    private void GenerateDrunckard()
    {
        var itercount = 0;
        var tileCount = 0;
        var position = startPosition;
        AddTile(position, ref tileCount);
        while (tileCount < nbTilesMax && itercount < iterMax)
        {
            var direction = _directions[Random.Range(0, _directions.Length)];
            var currentPathLength = Random.Range(lMin, lMax);
            var futurePosition = position + currentPathLength * new Vector3Int(direction.x, direction.y, 0);
            if (IsInBounds(futurePosition))
            {
                for (var i = 0; i < currentPathLength; i++)
                {
                    if (tileCount >= nbTilesMax)
                        break;
                    position += new Vector3Int(direction.x, direction.y, 0);
                    AddTile(position, ref tileCount);
                }
                itercount++;
            }
        }

        DateTime maintenant = DateTime.Now;
        string heure = maintenant.ToString("HH:mm:ss");
        Debug.Log("Tile " + tileCount + " / Iteration " + itercount + " / Time " + heure + " / Seed : " + seedHex);
    }

    private void GenerateFillWithLife()
    {
        var aliveCells = new List<Vector3Int>();
        var deadCells = new List<Vector3Int>();

        for (var nbIter = 0; nbIter < fillIteration; nbIter++)
        {
            aliveCells.Clear();
            deadCells.Clear();

            for (var x = _barrier.xMin; x < _barrier.xMax; x++)
            {
                for (var y = _barrier.yMin; y < _barrier.yMax; y++)
                {

                    //Debug.Log($"x={x} : y = {y} : {map.GetTile(new Vector3Int(x,y))}");

                    var countAlive = 0;
                    var position = new Vector3Int(x, y);
                    var isAlive = ground.HasTile(position);

                    foreach (var neighbour in GenerateNeighbours(new Vector2Int(-1,-1), new Vector2Int(1,1)))
                    {
                        if (IsInBounds(position + neighbour))
                        {
                            if (ground.GetTile(position + neighbour) != null)
                            {
                                countAlive++;
                            }
                        }
                    }

                    if (isAlive)
                    {
                        //rien
                    }
                    else
                    {
                        if (aliveLimit + nbIter > 8)
                        {
                            continue;
                        }
                        if (countAlive >= aliveLimit + nbIter)
                        {

                            aliveCells.Add(position);

                        }
                        else
                        {
                            deadCells.Add(position);
                        }
                    }
                }
            }

            foreach (var aliveCell in aliveCells)
            {
                if (!ground.HasTile(aliveCell))
                {
                    ground.SetTile(aliveCell, GetRandomTile());
                }
            }
            foreach (var deadCell in deadCells)
            {
                if (ground.HasTile(deadCell))
                {
                    ground.SetTile(deadCell, null);
                }
            }
            //Debug.Log($"Game Of Life : Fill iteration {nbIter}");
        }
    }

    private void GenerateWall()
    {
        for (var x = _barrier.xMin; x < _barrier.xMax; x++)
        {
            for (var y = _barrier.yMin; y < _barrier.yMax; y++)
            {
                if (ground.HasTile(new Vector3Int(x, y)))
                {
                    fackWall.SetTile(new Vector3Int(x, y), fackWallTile);
                    foreach (var neighbour in GenerateNeighbours(new Vector2Int(-1,-1), new Vector2Int(1,1)))
                    {
                        if (!ground.HasTile(new Vector3Int(x, y) + neighbour))
                        {
                            fackWall.SetTile(new Vector3Int(x, y) + neighbour, fackWallTile);
                            wall.SetTile(new Vector3Int(x, y) + neighbour, wallTile);
                        }
                    }
                }
            }
        }
    }

    private Tile GetRandomTile()
    {
        var localTiles = tiles.OrderByDescending(t => t.wight);

        var allWight = localTiles.Sum(tile => tile.wight);

        var tileRandom = Random.Range(0, allWight);

        var wightForRand = 0f;
        foreach (var tile in localTiles)
        {
            if (tile.wight + wightForRand >= tileRandom)
            {
                return tile.tile;
            }
            wightForRand += tile.wight;
        }
        Debug.LogError("Tile No Generate");
        return null;
    }

    private void AddTile(Vector3Int position, ref int count)
    {
        if (ground.HasTile(position))
            return;

        ground.SetTile(position, GetRandomTile());
        count++;
    }

    private bool IsInBounds(Vector3Int futurePosition)
    {
        return (_barrier.xMin <= futurePosition.x && _barrier.xMax > futurePosition.x &&
                _barrier.yMin <= futurePosition.y && _barrier.yMax > futurePosition.y);
    }
}
