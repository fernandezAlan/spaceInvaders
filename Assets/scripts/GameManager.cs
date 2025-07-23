using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro; // Assuming you are using TextMeshPro for UI text  
public class GameManager : MonoBehaviour
{
    private GameObject gameManager;
    private int CurrentLevelIndex = 0; // Variable to keep track of the current level index
    private string[] sceneNames = { "MainMenu", "level 1", "level 2", "level 3", "level 4", "level 5", "YouWin", "GameOver" }; // Array of scene names to load
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int enemyCount = 0; // Variable to keep track of the number of enemies
    private int lives = 3; // Variable to keep track of the player's lives
    private PersistentUI persistentUI; // Reference to the PersistentUI script
    public GameObject playerPrefab;
    public EnemyStatsSO[] enemyStatsByLevel;
    private int currentPoints = 0;
    private int totalPoints = 0;
    private int TotalDamageRecived = 0; // Variable to keep track of the total damage received by the player    
    public float invulnerabilityDuration = 2f;
    private bool isInvulnerable = false;
    public Rigidbody heroRigidBody;
    public Collider heroCollider;
    public Renderer heroRenderer;
    public void RespawnPlayer()
    {
        Vector3 spawnPosition = new Vector3(-0.69f, 0.3f, -8.8f); // Coordenadas del punto de aparición
        GameObject newHero = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        Hero heroScript = newHero.GetComponent<Hero>();
        if (heroScript != null)
        {
            heroScript.SetInvulnerable(invulnerabilityDuration); // Activar invulnerabilidad
        }

    }
    public EnemyStatsSO GetCurrentEnemyStats()
    {
        int level = Mathf.Clamp(CurrentLevelIndex -1, 0, enemyStatsByLevel.Length - 1);
        return enemyStatsByLevel[level];
    }
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
        this.lives = lives; // Set the number of lives to the specified value
        if (lives <= 0)
        {
            this.lives = 3; // Reset lives to 3 if they are zero or less
           // this.CurrentLevelIndex = 1; // Reset the current level index to 0 (MainMenu)
            ChangeLevel(7); // If lives are zero, change to the GameOver scene
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
        this.enemyCount = enemyCount; // Set the enemy count to the specified value
    }

    public int GetEnemyCount()
    {
        return this.enemyCount; // Return the current enemy count
    }
    public void ChangeLevel()
    {
        int nextLevel = this.CurrentLevelIndex + 1;
        this.ChangeLevel(nextLevel); // Call the overloaded ChangeLevel method with the next level index
    }

    public void ChangeLevel(int levelIndex)
    {

        if (levelIndex < sceneNames.Length && levelIndex >= 0)
        {
            this.CurrentLevelIndex = levelIndex; // Update the current level index
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
        if (enemyCount <= 0 && sceneNames[CurrentLevelIndex] != "level 5")
        {
            LevelCompleted(); // Call LevelCompleted to update scores
            persistentUI = GameObject.Find("HUD").GetComponent<PersistentUI>();
            persistentUI.ShowLevelCompleteText(); // Show level complete text
            Invoke(nameof(ChangeLevel), 2f); // Espera 2 segundo y ejecuta
        }
        else if (enemyCount <= 0)
        {
            Invoke(nameof(ChangeToWinView), 2f);
        }
    }

    private void ChangeToWinView()
    {
        ChangeLevel(6); // Redirige manualmente al nivel final
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
        Destroy(gameManager); // Destroy the GameManager instance to reset the game state
    }
    // SCORE MANAGEMENT
    public void AddPoints(int points)
    {
        currentPoints += points;
    }

    public void LevelCompleted()
    {
        totalPoints += currentPoints;
        currentPoints = 0;
    }

    public int GetPointsValue()
    {
        return totalPoints + currentPoints; // por si se pierde antes de sumar
    }

    public int GetCurrentPoints()
    {
        return currentPoints;
    }

    public void ResetPoints()
    {
        currentPoints = 0;
        totalPoints = 0;
    }

    //TOTAL DAMEGE RECEIVED
    public void AddDamage(int damage)
    {
        TotalDamageRecived += damage; // Increment the total damage received by the specified amount
    }

    public int GetTotalDamageRecived()
    {
        return TotalDamageRecived; // Return the total damage received
    }
    // Update is called once per frame
    void Update()
    {

    }
}