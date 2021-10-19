using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameSceneUIHandler : MonoBehaviour
{
    private GameManager gameManager;
    private TextMeshProUGUI livesText;
    private TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] Button restartButton;
    [SerializeField] Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        livesText = GameObject.Find("Lives Text").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UIGameOver()
    {
        DeactivateMainUI();
        ActivateGameOverUI();
    }

    void DeactivateMainUI()
    {
        livesText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }

    void ActivateGameOverUI()
    {
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "Game Over!\n" + "Score: " + gameManager.score;
    }

    public void UpdateLivesUI()
    {
        livesText.text = "Lives: " + gameManager.lives;
    }

    public void UpdateScoreUI()
    {
        scoreText.text = "Score: " + gameManager.score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();//This exits the game irl.
#endif
    }
}
