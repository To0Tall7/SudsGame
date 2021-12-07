using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] GameObject genericEnemyPrefab;
    float spawnRangeX = 23.0f;
    int enemyCount;
    int waveNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;//Get a number of all current enemies in the scene.
        if (enemyCount == 0 && !gameManager.gameOver)
        {
            waveNumber++;
            SpawnGenericEnemy(waveNumber);
        }
    }

    void SpawnGenericEnemy(int amount)//This spawns the lowest tier enemy.
    {
        for (int i = 1; i <= amount; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-spawnRangeX, spawnRangeX), -1.0f);
            Instantiate(genericEnemyPrefab, spawnPosition, genericEnemyPrefab.transform.rotation);
        }
    }
}
