using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public static GameController gameController { get; private set; }

    public static int X_BOUND = 13;
    public static int Y_BOUND = 7;
    public static int SOME_RANDOM_OFFSET = 5;
    public static int X_LIM = X_BOUND + SOME_RANDOM_OFFSET;
    public static int Y_LIM = Y_BOUND + SOME_RANDOM_OFFSET;

    [SerializeField] private GameObject enemyPrefab;
    
    [SerializeField] private List<GameObject> towers;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap tilemap;
    // private List<GameObject> enemies;
    
    private int numEnemies;
    private int wave;

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

        wave = 0;
        numEnemies = 0;
    }
    
    void Update()
    {
        if (numEnemies == 0)
        {
            StartNextWave();
        }
    }

    void StartNextWave()
    {
        this.transform.position = grid.GetCellCenterWorld(new Vector3Int(
                -X_BOUND,
                Y_BOUND,
                0
            ));
        Vector3 pos;
        wave++;
        for(int i = 0; i < Mathf.Pow(wave, 2); i++)
        {
            int newX = Random.Range(0, SOME_RANDOM_OFFSET * 2) + X_BOUND;
            int newY = Random.Range(0, SOME_RANDOM_OFFSET * 2) + Y_BOUND;
            if (newX > X_LIM) newX = -X_BOUND - (newX - X_LIM);
            if (newY > Y_LIM) newY = -Y_BOUND - (newY - Y_LIM);
            Vector3Int temp = new Vector3Int(newX, newY, 0);
            pos = grid.GetCellCenterWorld(temp);
            Debug.Log($"Spawning at {temp}");
            GameObject.Instantiate(enemyPrefab, pos, Quaternion.identity);
            numEnemies++;
        }
    }

    public void OnEnemyDie()
    {
        numEnemies--;
    }

    public List<GameObject> GetTowers() { return towers; }
    public Grid GetGrid() { return grid; }
    public Tilemap GetTilemap() { return tilemap; }
}
