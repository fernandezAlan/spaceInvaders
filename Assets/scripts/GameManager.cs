using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro; // Assuming you are using TextMeshPro for UI text  
public class GameManager : MonoBehaviour
{
    private GameObject gameManager;
    private int CurrentLevelIndex = 0; // Variable to keep track of the current level index
    private string[] sceneNames = { "MainMenu", "level 1", "level 2", "level 3" }; // Array of scene names to load
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (gameManager == null)
        { 
            gameManager = GameObject.Find("GameManager");
            DontDestroyOnLoad(gameManager); // Prevent the GameManager from being destroyed when loading a new scene
        }
        else {             
            Destroy(gameObject); // If a GameManager already exists, destroy this instance to avoid duplicates
        }

    }

    public void ChangeLevel() 
    {
        int nextLevel = this.CurrentLevelIndex + 1; // Get the current level index
        this.ChangeLevel(nextLevel); // Call the overloaded ChangeLevel method with the next level index
    }

    public void ChangeLevel(int levelIndex) 
    {
        if (levelIndex < sceneNames.Length && levelIndex > 0)
        {
            SceneManager.LoadScene(sceneNames[levelIndex]); // Load the scene with the specified index
        }
        else { 
        throw new ArgumentOutOfRangeException("levelIndex", "Level index is out of range: " + levelIndex); // Throw an exception if the index is out of range
        }
    }

    public void ChangeLevel(string sceneName)
    {
        for (int i=0; i<sceneNames.Length;i++) {
            if (sceneNames[i]== sceneName) {
                SceneManager.LoadScene(sceneName); // Load the scene with the specified index
                break;
            }
        }
          throw new ArgumentException("Scene name not found in the list: " + sceneName);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
