using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    private enum GameState
    {
        Defending, Farming
    }

    public static GameController gameController { get; private set; }

    public static int X_BOUND = 13;
    public static int Y_BOUND = 7;
    public static int SOME_RANDOM_OFFSET = 5;
    public static int X_LIM = X_BOUND + SOME_RANDOM_OFFSET;
    public static int Y_LIM = Y_BOUND + SOME_RANDOM_OFFSET;

    [SerializeField] InputReader inputReader;
    [SerializeField] private GameObject enemyPrefab;
    
    [SerializeField] private List<GameObject> towers;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap tilemap;
    
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
        wave = 5;
        numEnemies = 0;
        gameState = GameState.Farming;
    }

    void Start()
    {
        StartNextWave();
    }

    void OnEnable()
    {
        inputReader.SpawnWaveEvent += StartNextWave;
    }

    void OnDisable()
    {
        inputReader.SpawnWaveEvent -= StartNextWave;
    }

    void Update()
    {

    }

    public void OnEnemyDie()
    {
        numEnemies--;
        if (numEnemies == 0)
        {
            gameState = GameState.Farming;
        }
    }

    public void StartNextWave()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        if (gameState == GameState.Farming)
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
        yield return null;
    }

    Vector3Int GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }

    /* Getters */
    public List<GameObject> GetTowers() { return towers; }
    public Grid GetGrid() { return grid; }
    public Tilemap GetTilemap() { return tilemap; }
}