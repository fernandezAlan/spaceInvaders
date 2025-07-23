using UnityEngine;
using System.Collections;
public class EnemyBase : Ship
{

    //variables
    private GameManager gameManager; // Reference to the GameManager
    float nextFireTime = 0f;
    public GameObject enemyBullet; // Reference to the bullet prefab
    public bool isActive = false;
    public Vector3 basePosition;
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
  
    public void DestroyShip(bool explosion) {
        Destroy(gameObject); // Call Die method if health is zero or less
        int enemyCount = gameManager.GetEnemyCount();
        gameManager.SetEnemyCount(enemyCount - 1);
        if(explosion)StartSplosion();
        gameManager.CheckWinCondition(); // Check win condition after enemy is destroyed
    }
    public virtual void ActiveShip()
    {
        isActive = true;
    }


    protected virtual void Update()
    {
        // Check if it's time to fire
        if (Time.time >= nextFireTime && isActive)
        {
           float nextFireDelay = Random.Range(3f, 8f);
            FireBullet(); // Call method to fire enemy bullet
            nextFireTime = Time.time + nextFireDelay; // Set next fire time (2 seconds later)
        }
    }
}
