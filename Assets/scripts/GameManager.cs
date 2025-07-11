using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro; // Assuming you are using TextMeshPro for UI text  
public class GameManager : MonoBehaviour
{
    private GameObject gameManager;
    private int CurrentLevelIndex = 0; // Variable to keep track of the current level index
    private string[] sceneNames = { "MainMenu", "level 1", "level 2", "level 3", "YouWin", "GameOver" }; // Array of scene names to load
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int enemyCount = 0; // Variable to keep track of the number of enemies
    private int lives = 3; // Variable to keep track of the player's lives    
    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager");
            DontDestroyOnLoad(gameManager); // Prevent the GameManager from being destroyed when loading a new scene
        }
        else
        {
            Destroy(gameObject); // If a GameManager already exists, destroy this instance to avoid duplicates
        }

    }
    void Start()
    {
        PersistentUI ui = UnityEngine.Object.FindFirstObjectByType<PersistentUI>();
        if (ui != null)
        {
            ui.ConnectToGameManager(this);
        }
    }

    public void SetLives(int lives)
    {
        Debug.Log("Setting lives to: " + lives); // Log the new number of lives
        this.lives = lives; // Set the number of lives to the specified value
        if (lives <= 0)
        {
            this.lives = 3; // Reset lives to 3 if they are zero or less
           // this.CurrentLevelIndex = 1; // Reset the current level index to 0 (MainMenu)
            ChangeLevel(5); // If lives are zero, change to the GameOver scene
        }
    }

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name; // Get the name of the currently active scene
    }
    public int GetLives()
    {
        Debug.Log("Current lives: " + this.lives); // Log the current number of lives
        return this.lives; // Return the current number of lives
    }
    public void SetEnemyCount(int enemyCount)
    {
        Debug.Log("Setting enemy count to: " + enemyCount); // Log the new enemy count
        this.enemyCount = enemyCount; // Set the enemy count to the specified value
    }

    public int GetEnemyCount()
    {
        Debug.Log("Current enemy count: " + this.enemyCount); // Log the current enemy count
        return this.enemyCount; // Return the current enemy count
    }
    public void ChangeLevel()
    {
        Debug.LogWarning("ChangeLevel called with CurrentLevelIndex: " + this.CurrentLevelIndex); // Log the current level index
        int nextLevel = this.CurrentLevelIndex + 1;
        Debug.LogWarning("Changing to next level: " + nextLevel); // Log the next level index
        this.ChangeLevel(nextLevel); // Call the overloaded ChangeLevel method with the next level index
    }

    public void ChangeLevel(int levelIndex)
    {

        if (levelIndex < sceneNames.Length && levelIndex >= 0)
        {
            Debug.LogWarning("Changing to level index: " + levelIndex); // Log the level index being changed to
            this.CurrentLevelIndex = levelIndex; // Update the current level index
            Debug.LogWarning("CurrentLevelIndex: " + this.CurrentLevelIndex);
            SceneManager.LoadScene(sceneNames[levelIndex]); // Load the scene with the specified index
        }
        else
        {
            throw new ArgumentOutOfRangeException("levelIndex", "Level index is out of range: " + levelIndex); // Throw an exception if the index is out of range
        }
    }

    public void ChangeLevel(string sceneName)
    {
        for (int i = 0; i < sceneNames.Length; i++)
        {
            if (sceneNames[i] == sceneName)
            {
                SceneManager.LoadScene(sceneName); // Load the scene with the specified index
                break;
            }
        }
        throw new ArgumentException("Scene name not found in the list: " + sceneName);

    }

    public void CheckWinCondition()
    {
        if (enemyCount <= 0 && sceneNames[CurrentLevelIndex] != "level 3")
        {
            ChangeLevel(); // Change to the next level if all enemies are defeated
        }
        else if (enemyCount <= 0)
        {
            ChangeLevel(4); // If all levels are completed, return to the main menu
           
        }
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
        Destroy(gameManager); // Destroy the GameManager instance to reset the game state
    }

    // Update is called once per frame
    void Update()
    {

    }
}