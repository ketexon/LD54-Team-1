using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public static GameController gameController { get; private set; }

    public static float X_BOUND = 13;
    public static float Y_BOUND = 7;
    public static float SOME_RANDOM_OFFSET = 5;
    public static float X_LIM = X_BOUND + SOME_RANDOM_OFFSET;
    public static float Y_LIM = Y_BOUND + SOME_RANDOM_OFFSET;

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
        Vector3 pos;
        wave++;
        for(int i = 0; i < Mathf.Pow(wave, 2); i++)
        {
            numEnemies++;
            pos = 
            grid.GetCellCenterWorld(
            grid.WorldToCell(new Vector3(
                Random.Range(X_BOUND, X_LIM) * (Random.Range(0, 2) == 1 ? 1 : -1),
                Random.Range(Y_BOUND, Y_LIM) * (Random.Range(0, 2) == 1 ? 1 : -1),
                0f
            )));
            Debug.Log($"Spawning at {pos}");
            GameObject.Instantiate(enemyPrefab, pos, Quaternion.identity);
            this.transform.position = pos;
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
