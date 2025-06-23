using TMPro;
using UnityEngine;
public class PersistentUI : MonoBehaviour
{
    private static PersistentUI instance;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI LevelText;
    private GameManager gameManager; // Reference to the GameManager
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Find the GameManager in the scene
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // <- persiste entre escenas
        }
        else
        {
            Destroy(gameObject); // <- evita duplicados
        }
    }
    void Update()
    {
        
        livesText.text = "x" + gameManager.GetLives();
        LevelText.text = gameManager.GetCurrentSceneName(); // Display the current level index
    }
}
