using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class GameController : MonoBehaviour
{
    private enum GameState
    {
        Defending, Farming, EnemiesSpawning
    }

    public static GameController gameController { get; private set; }

    public static int X_BOUND = 13;
    public static int Y_BOUND = 7;
    public static int SOME_RANDOM_OFFSET = 5;
    public static int X_LIM = X_BOUND + SOME_RANDOM_OFFSET;
    public static int Y_LIM = Y_BOUND + SOME_RANDOM_OFFSET;

    [SerializeField] InputReader inputReader;
    [SerializeField] private GameObject enemyPrefab;
    
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap landTilemap;
    [SerializeField] private Tilemap otherTilemap;

    public System.Action WaveEndEvent;

    List<Placeable> placeables = new();
    
    public IReadOnlyList<Placeable> Placeables => placeables;

    List<PlantBuff> plantBuffs = new();
    public IReadOnlyList<PlantBuff> PlantBuffs => plantBuffs;

    public PlantBuff NetPlantBuff { get; private set; } = new();


    [SerializeField] private GameObject farmUI;
    
    private int numEnemies;
    private int wave;
    private GameState gameState;
    private List<Vector3Int> spawnPoints = new List<Vector3Int>();

    void Awake()
    {
        if (gameController != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            gameController = this;
        }

        for (int x = -X_LIM+1; x < X_LIM; x++)
        {
            for (int y = -Y_LIM+1; y < Y_LIM; y++)
            {
                if (
                    (-1 * X_BOUND <= x && x <= X_BOUND) && 
                    (-1 * Y_BOUND <= y && y <= Y_BOUND)
                ) continue;
                spawnPoints.Add(new Vector3Int(x, y, 0));
            }
        }
        wave = 0;
        numEnemies = 0;
        gameState = GameState.Farming;
    }

    void Update()
    {
        string plantStr = "";
        foreach(var plant in ResourceManager.Instance.Seeds)
        {
            plantStr += "{plant.key.name}: {plant.value}\n";
        }
    }

    void OnEnable()
    {
        inputReader.SpawnWaveEvent += StartNextWave;
    }

    void OnDisable()
    {
        inputReader.SpawnWaveEvent -= StartNextWave;
    }

    public void AddPlaceable(Placeable placeable)
    {
        placeables.Add(placeable);

        if(placeable is Plant plant)
        {
            plantBuffs.Add(plant.Stats.Buff);
            NetPlantBuff += plant.Stats.Buff;
        }
    }

    public void OnPlaceableDie(Placeable placeable)
    {
        placeables.Remove(placeable);
        if(placeable is Plant plant)
        {
            plantBuffs.Remove(plant.Stats.Buff);
            NetPlantBuff += -plant.Stats.Buff;
        }
    }

    public void SetLand(Vector3Int loc, Tile tile)
    {
        landTilemap.SetTile(loc, tile);
    }

    public void SetBuilding(Vector3Int loc, Tile tile)
    {
        otherTilemap.SetTile(loc, tile);
    }

    public bool HasFertileLand(Vector3Int loc)
    {
        return landTilemap.HasTile(loc) &&
                landTilemap.GetInstantiatedObject(loc).GetComponent<Land>().status == Land.LandStatus.Fertile;
    }
    
    public bool HasBuilding(Vector3Int loc)
    {
        return otherTilemap.HasTile(loc);
    }

    public bool HasLand(Vector3Int loc)
    {
        Debug.Log($"{loc}, {landTilemap.HasTile(loc)}");
        return landTilemap.HasTile(loc);
    }
    
    void StartFarming()
    {
        if (gameState == GameState.Defending)
        {
            WaveEndEvent?.Invoke();
            gameState = GameState.Farming;
            farmUI.SetActive(true);
        }
    }

    public void StartNextWave()
    {
        Debug.Log("HI");
        if (gameState == GameState.Farming)
        {
            gameState = GameState.EnemiesSpawning;
            farmUI.SetActive(false);
            StartCoroutine(SpawnEnemies());
        }
    }

    public void RemoveLandTile(Vector3 loc)
    {
        landTilemap.SetTile(grid.WorldToCell(loc), null);
    }

    IEnumerator SpawnEnemies()
    {
        Vector3 pos;
        wave++;
        for(int i = 0; i < Mathf.Pow(wave, 2); i++)
        {
            pos = GetRandomSpawnPoint();
            GameObject.Instantiate(enemyPrefab, pos, Quaternion.identity);
            numEnemies++;
            yield return new WaitForSeconds(.59f);
        }
        gameState = GameState.Defending;
    }

    Vector3Int GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }

    public void OnEnemyDie()
    {
        numEnemies--;
        if (numEnemies == 0)
        {
            StartFarming();
        }
    }

    /* Getters */
    public Grid GetGrid() { return grid; }
}
