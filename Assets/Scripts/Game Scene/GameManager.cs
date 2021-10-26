using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variable Declarations
    public int lives = 3;
    public bool gameOver = false;
    public int score = 0;

    private GameSceneUIHandler gameSceneUIHandler;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        gameSceneUIHandler = GameObject.Find("Canvas").GetComponent<GameSceneUIHandler>();
        gameSceneUIHandler.UpdateScoreUI();
        gameSceneUIHandler.UpdateLivesUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GameOver()
    {
        gameOver = true;
        gameSceneUIHandler.UIGameOver();//Activate and deactivate the relevant UI when a game over is triggered.
    }

    public void UpdateLives(int modifier)
    {
        lives += modifier;
        gameSceneUIHandler.UpdateLivesUI();
        if (lives <= 0)
        {
            GameOver();
        }
    }

    public void UpdateScore(int modifier)
    {
        score += modifier;
        gameSceneUIHandler.UpdateScoreUI();
    }
}
