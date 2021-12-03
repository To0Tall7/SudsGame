using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] GameObject genericEnemyPrefab;
    [SerializeField] GameObject exclamationMarkPrefab;
    float spawnRangeX = 23.0f;
    int enemyCount;
    float timeDelay = 2.0f;
    int waveNumber = 0;
    bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (gameManager.gameOver)
        {
            StopAllCoroutines();//BUG: Exclamation mark stays on screen if game over occurs before virus spawns.
        }*/

        enemyCount = FindObjectsOfType<Enemy>().Length;//Get a number of all current enemies in the scene.
        if (enemyCount == 0 && !gameManager.gameOver && !isSpawning)
        {
            waveNumber++;
            timeDelay = timeDelay - (waveNumber / 5) * .1f;
            if (timeDelay <= 0)
            {
                timeDelay = .2f;//Min time delay.
            }
            StartCoroutine(SpawnGenericEnemy(waveNumber, timeDelay));
        }
    }

    /*void SpawnGenericEnemy(int amount)//This spawns the lowest tier enemy.
    {
        for (int i = 1; i <= amount; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-spawnRangeX, spawnRangeX), -1.0f);
            Instantiate(genericEnemyPrefab, spawnPosition, genericEnemyPrefab.transform.rotation);
        }
    }*/

    IEnumerator SpawnGenericEnemy(int amount, float delay)
    {
        isSpawning = true;//Put in Update's if statement if not firing in time.
        for (int i = 1; i <= amount; i++)
        {
            if (!gameManager.gameOver)//Error thrown for enemy's NullReferenceException
            {
                Vector2 spawnPosition = new Vector2(Random.Range(-spawnRangeX, spawnRangeX), -1.0f);
                GameObject exclMark = Instantiate(exclamationMarkPrefab, spawnPosition, exclamationMarkPrefab.transform.rotation);//Instantiate an exclamation mark in the spot the virus will spawn.
                yield return new WaitForSeconds(1.0f);
                Destroy(exclMark);//Destroy the exclamation mark.
                Instantiate(genericEnemyPrefab, spawnPosition, genericEnemyPrefab.transform.rotation);//Spawn the virus.
                if (i != amount)//If not spawning the last enemy, place a delay between this spawn and the next.
                {
                    yield return new WaitForSeconds(delay);
                }
            }
        }
        isSpawning = false;
    }
}