using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    private List<Enemy> enemyList = new List<Enemy>();
    float timeToSpawn;
    int createdEnemies;

    [SerializeField] GameObject EnemyPrefab;
    
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        timeToSpawn = 2;
        createdEnemies = 0;
    }

    private void OnEnable()
    {
        FinishGamePopup.OnRestartGame += ResetGame;
    }
    private void OnDisable()
    {
        FinishGamePopup.OnRestartGame -= ResetGame;
    }
    
    void Update()
    {
        timeToSpawn -= Time.deltaTime;
        if(timeToSpawn <= 0)
        {
            timeToSpawn = 2;
            SpawnAnEnemy();
        }
    }

    /// <summary>
    /// Spawn an enemy after 2 second, it will first check wether there are dead enemies otherwise it will instantiate a new one
    /// </summary>
    private void SpawnAnEnemy()
    {
        Enemy enemy = null;
        List<Enemy> diedEnemies = enemyList.Where(x => !x.gameObject.activeInHierarchy).ToList();
        Vector3 randomPos = new Vector3(Random.Range(-4, 4), Random.Range(-5, 5), 0);
        if (diedEnemies.Count > 0)
        {
            enemy = diedEnemies[Random.Range(0, diedEnemies.Count)];
            enemy.transform.position = randomPos;
            enemy.gameObject.SetActive(true);
        }
        if (enemy == null)
        {
            createdEnemies++;
            GameObject enemyObj = Instantiate(EnemyPrefab, randomPos, Quaternion.identity, transform);
            enemyObj.name = $"Enemy: {createdEnemies}";
            enemy = enemyObj.GetComponent<Enemy>();
            enemyList.Add(enemy);
        }
    }
    /// <summary>
    /// To Restart The Game if the user wants that
    /// </summary>
    public void ResetGame()
    {
        timeToSpawn = 0;
        createdEnemies = enemyList.Count;
        foreach (Enemy enemy in enemyList) 
        {
            enemy.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Returns an enemy to be attacked by player
    /// </summary>
    /// <returns></returns>
    public Enemy PickAnEnemy()
    {
        var CurrentEnemies = enemyList.Where(x => x.gameObject.activeInHierarchy).ToList();
        if(CurrentEnemies.Count > 0)
            return enemyList[Random.Range(0, CurrentEnemies.Count)];
        else return null;
    }
}
