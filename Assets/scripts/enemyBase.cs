using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private float health = 100f; // Health of the enemy
   private GameManager gameManager; // Reference to the GameManager

    private void Start()
    {
        Debug.LogWarning("EnemyBase Start method called"); // Log when the Start method is called
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Find the GameManager in the scene
        int enemyCount = gameManager.GetEnemyCount(); // Get the current enemy count from GameManager
        gameManager.SetEnemyCount(enemyCount + 1); // Increment enemy count in GameManager
       
        
    }
    public void TakeDamage(float damageAmount) 
    {
        Debug.LogWarning("taking damege:" + damageAmount); // Log the damage amount
        health -= damageAmount; // Reduce health by the damage amount
        if (health <= 0f) 
        {
            Destroy(gameObject); // Call Die method if health is zero or less
            int enemyCount = gameManager.GetEnemyCount();
            gameManager.SetEnemyCount(enemyCount - 1);
            gameManager.CheckWinCondition(); // Check win condition after enemy is destroyed
        }
    }
}
