using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private GameManager gameManager;
    private void Awake()
    {
        // Si ya hay un MusicManager, destruye este nuevo para evitar duplicados
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void Update()
    {
        string currentScene = gameManager.GetCurrentSceneName();
        if (gameManager && currentScene == "GameOver" || currentScene == "MainMenu" || currentScene == "YouWin")
        {
            Destroy(gameObject); 
        }
    }
}
