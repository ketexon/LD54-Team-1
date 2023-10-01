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
    
    [SerializeField] private List<Placeable> placeables;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap tilemap;

    public IReadOnlyList<Placeable> Placeables => placeables;

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

    void Start()
    {
        StartNextWave();
    }

    void Update()
    {
        string plantStr = "";
        foreach(var plant in ResourceManager.Instance.Seeds)
        {
            plantStr += "{plant.key.name}: {plant.value}\n";
        }

        farmUI.transform.Find("Resources").GetComponent<TMP_Text>().text = 
            $"Metal: {ResourceManager.Instance.Metal}\nEnergy: {ResourceManager.Instance.Energy}";
    }

    void OnEnable()
    {
        inputReader.SpawnWaveEvent += StartNextWave;
    }

    void OnDisable()
    {
        inputReader.SpawnWaveEvent -= StartNextWave;
    }

    public void OnPlaceableDie(Placeable placeable)
    {
        placeables.Remove(placeable);
    }

    void StartFarming()
    {
        if (gameState == GameState.Defending)
        {
            gameState = GameState.Farming;
            farmUI.SetActive(true);
        }
    }

    public void StartNextWave()
    {
        if (gameState == GameState.Farming)
        {
            gameState = GameState.EnemiesSpawning;
            farmUI.SetActive(false);
            StartCoroutine(SpawnEnemies());
        }
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
    public Tilemap GetTilemap() { return tilemap; }
}
