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
    private Animator VirusAnimator;
    // private bool VirusIsRunning, VirusIsJumping, VirusIsKicking, VirusIsDead, VirusIsLanding, VirusIsSpawning = false;
    // private bool IsRunning, IsJumping, IsPunching, IsHurt, IsDead, IsLanding = false;



    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        VirusAnimator = GetComponent<Animator>();
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
            // VirusAnimator.SetBool("isSpawning", true);
            Vector2 spawnPosition = new Vector2(Random.Range(-spawnRangeX, spawnRangeX), -1.0f);
            Instantiate(genericEnemyPrefab, spawnPosition, genericEnemyPrefab.transform.rotation);
            // VirusAnimator.SetBool("isSpawning", false);
        }
    }
}
