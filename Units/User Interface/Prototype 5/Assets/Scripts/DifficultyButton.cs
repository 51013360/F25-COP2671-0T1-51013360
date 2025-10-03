using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    // Initialize variables
    private Button button;
    private GameManager gameManager;
    public int difficulty;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get button component
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void SetDifficulty()
    {
        // Let player know their chosen difficulty
        Debug.Log(gameObject.name + " was clicked");
        
        // Start the game based on difficulty
        gameManager.StartGame(difficulty);
    }
}
