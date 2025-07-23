using TMPro;
using UnityEngine;
using System.Collections;
public class PersistentUI : MonoBehaviour
{
    private static PersistentUI instance;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI CurrentScore;
    public TextMeshProUGUI LevelCompleteText; 
    private GameManager gameManager; // Reference to the GameManager
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Find the GameManager in the scene
        LevelCompleteText.gameObject.SetActive(false);// Hide the level complete text at the start

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
    public void ConnectToGameManager(GameManager gm)
    {
        this.gameManager = gm;
    }

    IEnumerator ShowLevelCompleteBriefly()
    {
        LevelCompleteText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        LevelCompleteText.gameObject.SetActive(false);
    }

   public void ShowLevelCompleteText()
    {
       
        StartCoroutine(ShowLevelCompleteBriefly());
    }
    void Update()
    {
        
        livesText.text = "x" + gameManager.GetLives();
        LevelText.text = gameManager.GetCurrentSceneName(); // Display the current level index
        CurrentScore.text = "Score: " + gameManager.GetCurrentPoints(); // Display the current score
    }
}
