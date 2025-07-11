using UnityEngine;
using System.Collections;
public class EnemyBase : Ship
{

    //variables
   private GameManager gameManager; // Reference to the GameManager
    float nextFireTime = 0f;
    public GameObject enemyBullet; // Reference to the bullet prefab

   
    //funciones
    protected override void Start()
    {
        base.Start(); // Call the base class Start method to initialize audio and sprite renderer
        if (healthBar != null) { 
        healthBar.desactiveHealthBar(); // Hide health bar at the start
        }
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Find the GameManager in the scene
        int enemyCount = gameManager.GetEnemyCount(); // Get the current enemy count from GameManager
        gameManager.SetEnemyCount(enemyCount + 1); // Increment enemy count in GameManager

    }
    public void DestroyShip() {
        Destroy(gameObject); // Call Die method if health is zero or less
        int enemyCount = gameManager.GetEnemyCount();
        gameManager.SetEnemyCount(enemyCount - 1);
        gameManager.CheckWinCondition(); // Check win condition after enemy is destroyed
    }
   
  

    protected virtual void Update()
    {
        // Check if it's time to fire
        if (Time.time >= nextFireTime)
        {
            FireBullet(); // Call method to fire enemy bullet
            nextFireTime = Time.time + 2f; // Set next fire time (2 seconds later)
        }
    }
}
