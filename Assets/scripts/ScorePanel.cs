using TMPro;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI PointsValue;
    public TextMeshProUGUI DamageAmountRecivedValue;
    public TextMeshProUGUI TotalTime;
    public TextMeshProUGUI ScoreTotal;
    public TextMeshProUGUI BestScore;
    private GameManager gameManager; // Reference to the GameManager
    private GameTimer gameTimer; // Reference to the GameTimer
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Find the GameManager in the scene
        gameTimer = GameObject.Find("GameTimer").GetComponent<GameTimer>(); // Find the GameTimer in the scene
        PointsValue.text =  gameManager.GetPointsValue().ToString();
        DamageAmountRecivedValue.text = gameManager.GetTotalDamageRecived().ToString();
        TotalTime.text = FormatTime(gameTimer.GetElapsedTime());
        ScoreTotal.text = getFinalScore();
        SaveBestScore(); // Save the best score if applicable
        BestScore.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    }
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    private void SaveBestScore()
    {
        int currentScore = int.Parse(getFinalScore()); // Obtener el score final como entero
        float bestScore = PlayerPrefs.GetInt("BestScore", 0); // valor por defecto 0
        if (currentScore > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", currentScore);
            PlayerPrefs.Save(); // fuerza el guardado en disco
        }
    }
    private int getMinutes()
    {
        string[] parts = TotalTime.text.Split(':');
        if (parts.Length != 2)
            return 1; // fallback para evitar error

        int minutes = int.Parse(parts[0]);
        int seconds = int.Parse(parts[1]);

        return minutes + (seconds / 60);
    }
    private string getFinalScore()
    {
        int totalMinutes = getMinutes();
        // Evitar división por cero
        if (totalMinutes <= 0)
            totalMinutes = 1;
        int points = gameManager.GetPointsValue();
        int damageAmountReceived = gameManager.GetTotalDamageRecived();
        int finalScore = (points - damageAmountReceived) / totalMinutes;
        if(finalScore>0) return finalScore.ToString(); // Si el score es positivo, lo retornamos directamente
        else return "0"; // Si es negativo o cero, retornamos 0
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
