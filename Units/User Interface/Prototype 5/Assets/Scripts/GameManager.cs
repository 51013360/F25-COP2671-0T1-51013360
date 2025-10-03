using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Initialize variables
    public List<GameObject> targets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public GameObject titleScreen;
    private int score;
    private float spawnRate = 1.0f;
    public bool isGameActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnTarget()
    {
        // While game is active
        while (isGameActive)
        {
            // Spawn rate
            yield return new WaitForSeconds(spawnRate);

            // Choose random object of the 4 prefabs
            int index = Random.Range(0, targets.Count);

            // Spawn them in game
            Instantiate(targets[index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        // Update score text
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        // Activate restart button and game over text when game is over
        restartButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);

        // Set isGameActive to false
        isGameActive = false;
    }

    public void RestartGame()
    {
        // Restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty)
    {
        // Set isGameActive to true
        isGameActive = true;

        // Reset score
        score = 0;

        // Spawn rate changes based on chosen difficulty
        spawnRate /= difficulty;

        // Start spawning and updating score
        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        
        // Deactivate title screen
        titleScreen.gameObject.SetActive(false);
    }
}
